namespace OIDCPipeline.Core
{
    public class IdTokenResponse
    {
        public string access_token { get; set; }
        public string id_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public string expires_at { get; set; }
        public string LoginProvider { get; set; }
    }
}
