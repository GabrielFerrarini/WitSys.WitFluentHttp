using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    public interface IWitHttpClient : IFluentInterface
    {
        HttpVerb Verb { get; }
        Uri BaseAddress { get; }
        string AccessToken { get; }
        string BasicAuthentication { get; }
        ContentType ContentType { get; }
        IDictionary<string, string> Headers { get; }
        string BodyText { get; }
        IDictionary<string, string> BodyValues { get; }
        Task<WitHttpResponse> ExecuteAsync();
    }
}
