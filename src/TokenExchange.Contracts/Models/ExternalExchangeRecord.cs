using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Utils.Models;

namespace TokenExchange.Contracts.Models
{
    
    public partial class Mint
    {
        [JsonProperty("exchangeUrl")]
        public string ExchangeUrl { get; set; }
    }
    public partial class ExternalExchangeRecord
    {
        [JsonProperty("exchangeName")]
        public string ExchangeName { get; set; }

        [JsonProperty("mintType")]
        public string MintType { get; set; }

        [JsonProperty("selfMint")]
        public Mint SelfMint { get; set; }

        [JsonProperty("passThroughMint")]
        public Mint PassThroughMint { get; set; }

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
