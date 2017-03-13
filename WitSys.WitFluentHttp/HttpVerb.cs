namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// Indicates the HTTP VERB of the request
    /// </summary>
    public enum HttpVerb
    {
        /// <summary>
        /// Performs a GET
        /// </summary>
        Get = 0,
        /// <summary>
        /// Performs a POST
        /// </summary>
        Post = 1,
        /// <summary>
        /// Performs a PUT
        /// </summary>
        Put = 2,
        /// <summary>
        /// Performs a PATCH
        /// </summary>
        Patch = 3,
        /// <summary>
        /// Performs a DELETE
        /// </summary>
        Delete = 4
    }
}
