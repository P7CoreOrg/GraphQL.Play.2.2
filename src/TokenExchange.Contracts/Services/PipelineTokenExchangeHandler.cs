using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQLPlay.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TokenExchange.Contracts.Models;

namespace TokenExchange.Contracts.Services
{
    public class PipelineTokenExchangeHandler : ITokenExchangeHandler
    {
        private PipelineExchangeRecord _pipelineExchangeRecord;
        private PipelineExchangeOptions _settings;
        private IOptionsSnapshot<PipelineExchangeOptions> _optionsSnapshot;
        private ITokenExchangeHandlerPreProcessorStore _tokenExchangeHandlerPreProcessorStore;
        private ISummaryLogger _summaryLogger;
        private ILogger<ExternalExchangeTokenExchangeHandler> _logger;
        private IPipelineTokenExchangeHandlerRouter _pipelineTokenExchangeHandlerRouter;


        public string Name => _pipelineExchangeRecord.ExchangeName;
        public void Configure(PipelineExchangeRecord pipelineExchangeRecord)
        {
            _pipelineExchangeRecord = pipelineExchangeRecord;
            _settings = _optionsSnapshot.Get(_pipelineExchangeRecord.ExchangeName);
        }
        public PipelineTokenExchangeHandler(
            IPipelineTokenExchangeHandlerRouter pipelineTokenExchangeHandlerRouter,
            IOptionsSnapshot<PipelineExchangeOptions> optionsSnapshot,
            ITokenExchangeHandlerPreProcessorStore tokenExchangeHandlerPreProcessorStore,
            ISummaryLogger summaryLogger,
            ILogger<ExternalExchangeTokenExchangeHandler> logger)
        {
            _pipelineTokenExchangeHandlerRouter = pipelineTokenExchangeHandlerRouter;
            _optionsSnapshot = optionsSnapshot;
            _tokenExchangeHandlerPreProcessorStore = tokenExchangeHandlerPreProcessorStore;
            _summaryLogger = summaryLogger;
            _logger = logger;
        }

        public async Task<List<TokenExchangeResponse>> ProcessExchangeAsync(TokenExchangeRequest tokenExchangeRequest)
        {
            if (await _pipelineTokenExchangeHandlerRouter.PipelineTokenExchangeHandlerExistsAsync(_pipelineExchangeRecord
                .FinalExchange))
            {
                Dictionary<string, List<KeyValuePair<string, string>>> mapOpaqueKeyValuePairs =
                    new Dictionary<string, List<KeyValuePair<string, string>>>();
                if (_pipelineExchangeRecord.Preprocessors != null)
                {
                    foreach (var preProcessor in _pipelineExchangeRecord.Preprocessors)
                    {
                        var preProcessorHandler = _tokenExchangeHandlerPreProcessorStore.Get(preProcessor);
                        if (preProcessor == null)
                        {
                            var message = $"The preprocessor:{preProcessor} does not exist!";
                            _logger.LogCritical(message);
                            throw new Exception(message);
                        }

                        _logger.LogInformation($"The preprocessor:{preProcessor} was found!");
                        var opaqueKeyValuePairs = await preProcessorHandler.ProcessAsync(ref tokenExchangeRequest);
                        if (opaqueKeyValuePairs.Any())
                        {
                            mapOpaqueKeyValuePairs.Add(preProcessor, opaqueKeyValuePairs);
                        }
                    }
                }

                _logger.LogInformation($"Forwarding request to finalExchange:{_pipelineExchangeRecord.FinalExchange}!");
                var result = await _pipelineTokenExchangeHandlerRouter.ProcessFinalPipelineExchangeAsync(
                    _pipelineExchangeRecord.FinalExchange,
                    tokenExchangeRequest,
                    mapOpaqueKeyValuePairs);
                _summaryLogger.Add($"pipelineExchange_{_pipelineExchangeRecord.ExchangeName}", "OK");
                return result;
            }
            else
            {
                var message = $"The finalExchange:{_pipelineExchangeRecord.FinalExchange} does not exist!";
                _logger.LogCritical(message);
                _summaryLogger.Add($"pipelineExchange_{_pipelineExchangeRecord.ExchangeName}", "FATAL");
                throw new Exception(message);
            }
        }
        public static void RegisterServices(IServiceCollection services,
            IPipelineExchangeStore pipelineExchangeStore)
        {
            foreach (var exchange in pipelineExchangeStore.GetPipelineExchangeRecordAsync().GetAwaiter().GetResult())
            {
                services.Configure<PipelineExchangeOptions>(exchange.ExchangeName, options =>
                {
                    options.PreProcessors = exchange.Preprocessors;
                });
                services.AddTransient<Lazy<ITokenExchangeHandler>>(serviceProvider =>
                {
                    return new Lazy<ITokenExchangeHandler>(() =>
                    {
                        var tokenExchangeHandler = serviceProvider.GetRequiredService<PipelineTokenExchangeHandler>();
                        tokenExchangeHandler.Configure(exchange);
                        return tokenExchangeHandler;
                    });
                }); 
            }
        }
    }
}