using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// The basic interface to expose request information
    /// </summary>
    public interface IWitHttpClient : IFluentInterface
    {
        /// <summary>
        /// The HTTP verb of the request
        /// </summary>
        HttpVerb Verb { get; }
        /// <summary>
        /// The base assdress of the request
        /// </summary>
        Uri BaseAddress { get; }
        /// <summary>
        /// The access token sent with the request, if required
        /// </summary>
        string AccessToken { get; }
        /// <summary>
        /// The basic authentication info sent with the request, if required
        /// </summary>
        string BasicAuthentication { get; }
        /// <summary>
        /// The type of the content sent with the request, if required
        /// </summary>
        ContentType ContentType { get; }
        /// <summary>
        /// A list of the header values sent with the request. Access token and Basic authentication will also be in here
        /// </summary>
        IDictionary<string, string> Headers { get; }
        /// <summary>
        /// The text representation of the body content sent with the request
        /// </summary>
        string BodyText { get; }
        /// <summary>
        /// A list of body values sent with the request
        /// </summary>
        IDictionary<string, string> BodyValues { get; }
        /// <summary>
        /// The method that executes the specified VERB
        /// </summary>
        Task<WitHttpResponse> ExecuteAsync();
    }
}
