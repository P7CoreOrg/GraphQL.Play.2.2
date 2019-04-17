using System;
using System.Collections.Generic;
using System.Text;
using Utils.Models;

namespace TokenExchange.Contracts.Models
{
    public class ExternalExchangeClientCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string Authority { get; set; }
        public string ExchangeUrl { get; set; }
        public string ExchangeName { get; set; }
        public List<HttpHeader> AdditionalHeaders { get; set; }

    }
}
