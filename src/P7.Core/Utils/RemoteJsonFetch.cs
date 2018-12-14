using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace P7.Core.Utils
{
    public class WebRequestInit
    {
        public WebHeaderCollection Headers { get; set; }
        public string Accept { get; set; }
    }

    public class RemoteJsonFetch
    {

        public static async Task<string> GetRemoteJsonContentAsync(string url)
        {
            var byteResult = await RemoteFetch.FetchAsync(url, new WebRequestInit() {Accept = "application/json"});
            var sResult = Encoding.UTF8.GetString(byteResult);
            return sResult;
        }

        public static async Task<string> GetRemoteJsonContentAsync(string url, string schema)
        {


            string content;

            content = await GetRemoteJsonContentAsync(url);

            var validateResponse =
                JsonSchemaValidator.Validate(new JsonValidateRequest() {Json = content, Schema = schema});
            if (!validateResponse.Valid)
            {
                throw new Exception(validateResponse.Errors[0].Message);
            }

            return content;
        }
    }
}