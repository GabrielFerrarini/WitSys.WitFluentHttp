using System;
using System.Net.Http;

namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// Entry point for fluent methods
    /// </summary>
    public interface IAddress : IFluentInterface
    {
        /// <summary>
        /// Chaining method accepting an Uri object
        /// </summary>
        /// <param name="baseAddress">The Uri for the request address</param>
        /// <returns>The caller object</returns>
        IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(Uri baseAddress);

        /// <summary>
        /// Chaining method accepting an string value
        /// </summary>
        /// <param name="baseAddress">The String for the request address</param>
        /// <returns>The caller object</returns>
        IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(string baseAddress);
    }

    /// <summary>
    /// Fluent interface
    /// </summary>
    public interface IAuthOrHeaderOrContentOrBodyOrVerb : IHeaderOrContentOrBodyOrVerb
    {
        /// <summary>
        /// Chaining method accepting an string value containing the basic authentication value
        /// </summary>
        /// <param name="basicAuthentication">The basic authentication value</param>
        /// <returns>The caller object</returns>
        IHeaderOrContentOrBodyOrVerb WithBasicAuthentication(string basicAuthentication);

        /// <summary>
        /// Chaining method accepting an string value containing the token value
        /// </summary>
        /// <param name="accessToken">The token value</param>
        /// <returns>The caller object</returns>
        IHeaderOrContentOrBodyOrVerb WithBearerAccessToken(string accessToken);
    }

    /// <summary>
    /// Fluent interface
    /// </summary>
    public interface IHeaderOrContentOrBodyOrVerb : IBodyValueOrVerb
    {
        /// <summary>
        /// Chaining method accepting header values to be sent with the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The caller object</returns>
        IHeaderOrContentOrBodyOrVerb WithHeader(string key, string value);

        /// <summary>
        /// Chaining method accepting the ContentType of the value being sent with the request
        /// </summary>
        /// <param name="contentType">The ContentType value</param>
        /// <returns>The caller object</returns>
        IBodyTextOrVerb WithContentType(ContentType contentType);
    }

    /// <summary>
    /// Fluent interface
    /// </summary>
    public interface IBodyValueOrVerb : IVerb
    {
        /// <summary>
        /// Chaining method accepting the body values to be sent with the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The caller object</returns>
        IBodyValueOrVerb WithBodyValue(string key, string value);
    }

    /// <summary>
    /// Fluent interface
    /// </summary>
    public interface IBodyTextOrVerb : IVerb
    {
        /// <summary>
        /// Chaining method accepting an object to be parsed and sent with the request
        /// </summary>
        /// <param name="bodyObject">The object to be sent</param>
        /// <returns>The caller object</returns>
        IVerb WithBodyObject(object bodyObject);

        /// <summary>
        /// Chaining method accepting a body text value to be sent with the request
        /// </summary>
        /// <param name="bodyText">The body text to be sent</param>
        /// <returns>The caller object</returns>
        IVerb WithBodyText(string bodyText);
    }

    /// <summary>
    /// Fluent interface
    /// </summary>
    public interface IVerb : IFluentInterface
    {
        /// <summary>
        /// Executes the post
        /// </summary>
        /// <returns>The caller object</returns>
        IWitHttpClient Post();

        /// <summary>
        /// /// Executes the post
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        IWitHttpClient Post(HttpMessageHandler httpMessageHandler);

        /// <summary>
        /// Executes the get
        /// </summary>
        /// <returns>The caller object</returns>
        IWitHttpClient Get();

        /// <summary>
        /// /// Executes the get
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        IWitHttpClient Get(HttpMessageHandler httpMessageHandler);

        /// <summary>
        /// Executes the put
        /// </summary>
        /// <returns>The caller object</returns>
        IWitHttpClient Put();

        /// <summary>
        /// /// Executes the put
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        IWitHttpClient Put(HttpMessageHandler httpMessageHandler);

        /// <summary>
        /// Executes the patch
        /// </summary>
        /// <returns>The caller object</returns>
        IWitHttpClient Patch();

        /// <summary>
        /// /// Executes the patch
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        IWitHttpClient Patch(HttpMessageHandler httpMessageHandler);

        /// <summary>
        /// Executes the delete
        /// </summary>
        /// <returns>The caller object</returns>
        IWitHttpClient Delete();

        /// <summary>
        /// /// Executes the delete
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        IWitHttpClient Delete(HttpMessageHandler httpMessageHandler);
    }
}