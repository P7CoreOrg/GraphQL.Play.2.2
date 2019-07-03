namespace OIDCPipeline.Core.AuthorizationEndpoint
{
    /// <summary>
    /// Validation result for authorize requests
    /// </summary>
    public class AuthorizeRequestValidationResult : ValidationResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeRequestValidationResult"/> class.
        /// </summary>
        /// <param name="request">The request.</param>
        public AuthorizeRequestValidationResult()
        {

            IsError = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AuthorizeRequestValidationResult" /> class.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="error">The error.</param>
        /// <param name="errorDescription">The error description.</param>
        public AuthorizeRequestValidationResult(string error, string errorDescription = null)
        {

            IsError = true;
            Error = error;
            ErrorDescription = errorDescription;
        }



    }
}
