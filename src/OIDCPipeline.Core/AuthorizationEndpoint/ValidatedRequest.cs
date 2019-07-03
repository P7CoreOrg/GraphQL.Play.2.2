using System.Collections.Specialized;

namespace OIDCPipeline.Core.AuthorizationEndpoint
{
    /// <summary>
    /// Base class for a validate authorize or token request
    /// </summary>
    internal class ValidatedRequest
    {
        /// <summary>
        /// Gets or sets the raw request data
        /// </summary>
        /// <value>
        /// The raw.
        /// </value>
        public NameValueCollection Raw { get; set; }

    }
}
