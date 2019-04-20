using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Models;

namespace TokenExchange.Contracts.Models
{

    public partial class ExternalExchangeHandler
    {
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("clientId")]
        public string ClientId { get; set; }
    }
    public partial class PassThroughHandler
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
    public partial class ExternalExchangeRecord
    {
        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("mintType")]
        public string MintType { get; set; }

        [JsonProperty("externalExchangeHandler")]
        public ExternalExchangeHandler ExternalExchangeHandler { get; set; }

        [JsonProperty("passThroughHandler")]
        public PassThroughHandler PassThroughHandler { get; set; }

        [JsonProperty("oAuth2_client_credentials")]
        public OAuth2ClientCredentials oAuth2_client_credentials { get; set; }

    }

    public class OAuth2ClientCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public List<HttpHeader> AdditionalHeaders { get; set; }

    }
}
