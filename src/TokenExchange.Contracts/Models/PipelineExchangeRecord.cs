using Newtonsoft.Json;
using System.Collections.Generic;

namespace TokenExchange.Contracts.Models
{
    public partial class PipelineExchangeRecord
    {
        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("preprocessors")]
        public List<string> Preprocessors { get; set; }
        [JsonProperty("finalExchange")]
        public string FinalExchange { get; set; }
    }
}
