using System;
using System.Net.Http;

namespace WitSys.WitFluentHttp
{
    public interface IAddress : IFluentInterface
    {
        IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(Uri baseAddress);
        IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(string baseAddress);
    }

    public interface IAuthOrHeaderOrContentOrBodyOrVerb : IHeaderOrContentOrBodyOrVerb
    {
        IHeaderOrContentOrBodyOrVerb WithBasicAuthentication(string basicAuthentication);
        IHeaderOrContentOrBodyOrVerb WithBearerAccessToken(string accessToken);
    }

    public interface IHeaderOrContentOrBodyOrVerb : IBodyValueOrVerb
    {
        IHeaderOrContentOrBodyOrVerb WithHeader(string key, string value);
        IBodyTextOrVerb WithContentType(ContentType contentType);
    }

    public interface IBodyValueOrVerb : IVerb
    {
        IBodyValueOrVerb WithBodyValue(string key, string value);
    }

    public interface IBodyTextOrVerb : IVerb
    {
        IVerb WithBodyObject(object bodyObject);
        IVerb WithBodyText(string bodyText);
    }

    public interface IVerb : IFluentInterface
    {
        IWitHttpClient Post();
        IWitHttpClient Post(HttpMessageHandler httpMessageHandler);
        IWitHttpClient Get();
        IWitHttpClient Get(HttpMessageHandler httpMessageHandler);
        IWitHttpClient Put();
        IWitHttpClient Put(HttpMessageHandler httpMessageHandler);
        IWitHttpClient Patch();
        IWitHttpClient Patch(HttpMessageHandler httpMessageHandler);
        IWitHttpClient Delete();
        IWitHttpClient Delete(HttpMessageHandler httpMessageHandler);
    }
}