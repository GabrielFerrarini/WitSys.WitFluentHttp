using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// The fluent object
    /// </summary>
    public class WitHttpClient : IWitHttpClient,
        IAddress,
        IAuthOrHeaderOrContentOrBodyOrVerb,
        IHeaderOrContentOrBodyOrVerb,
        IBodyValueOrVerb,
        IBodyTextOrVerb,
        IVerb
    {
        /// <summary>
        /// The HTTP verb of the request
        /// </summary>
        public HttpVerb Verb { get; private set; }
        /// <summary>
        /// The base assdress of the request
        /// </summary>
        public Uri BaseAddress { get; private set; }
        /// <summary>
        /// The access token sent with the request, if required
        /// </summary>
        public string AccessToken { get; private set; }
        /// <summary>
        /// The basic authentication info sent with the request, if required
        /// </summary>
        public string BasicAuthentication { get; private set; }
        /// <summary>
        /// The type of the content sent with the request, if required
        /// </summary>
        public ContentType ContentType { get; private set; }
        /// <summary>
        /// A list of the header values sent with the request. Access token and Basic authentication will also be in here
        /// </summary>
        public IDictionary<string, string> Headers { get; private set; }
        /// <summary>
        /// The text representation of the body content sent with the request
        /// </summary>
        public string BodyText { get; private set; }
        /// <summary>
        /// A list of body values sent with the request
        /// </summary>
        public IDictionary<string, string> BodyValues { get; private set; }

        private HttpClient client;
        private HttpContent content;

        internal WitHttpClient()
        {
            this.Headers = new Dictionary<string, string>();
            this.BodyValues = new Dictionary<string, string>();
            this.ContentType = ContentType.Text;
        }

        /// <summary>
        /// The method that executes the specified VERB
        /// </summary>
        public async Task<WitHttpResponse> ExecuteAsync()
        {
            WitHttpResponse retVal = null;

            switch (Verb)
            {
                case HttpVerb.Get:
                    retVal = await ExecuteGetAsync();
                    break;
                case HttpVerb.Post:
                    retVal = await ExecutePostAsync();
                    break;
                case HttpVerb.Put:
                    retVal = await ExecutePutAsync();
                    break;
                case HttpVerb.Patch:
                    retVal = await ExecutePatchAsync();
                    break;
                case HttpVerb.Delete:
                    retVal = await ExecuteDeleteAsync();
                    break;
            }

            return retVal;
        }

        private async Task<WitHttpResponse> ExecuteGetAsync()
        {
            HttpResponseMessage result = await client.GetAsync(BaseAddress);

            return await BuildResponse(result);
        }

        private async Task<WitHttpResponse> ExecutePostAsync()
        {
            HttpResponseMessage result = await client.PostAsync(BaseAddress, content);

            return await BuildResponse(result);
        }

        private async Task<WitHttpResponse> ExecutePutAsync()
        {
            HttpResponseMessage result = await client.PutAsync(BaseAddress, content);

            return await BuildResponse(result);
        }

        private async Task<WitHttpResponse> ExecutePatchAsync()
        {
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("PATCH"), BaseAddress) { Content = content };

            HttpResponseMessage result = await client.SendAsync(request);

            return await BuildResponse(result);
        }

        private async Task<WitHttpResponse> ExecuteDeleteAsync()
        {
            HttpResponseMessage result = await client.DeleteAsync(BaseAddress);

            return await BuildResponse(result);
        }

        private async Task<WitHttpResponse> BuildResponse(HttpResponseMessage responseMessage)
        {
            WitHttpResponse retVal = null;

            retVal = await Task.Run(() =>
            {
                return new WitHttpResponse(responseMessage, this.BaseAddress.AbsoluteUri);
            });

            return retVal;
        }

        #region Fluent Interface Implementation
        /// <summary>
        /// Chaining method accepting an Uri object
        /// </summary>
        /// <param name="baseAddress">The Uri for the request address</param>
        /// <returns>The caller object</returns>
        public IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
            return this;
        }

        /// <summary>
        /// Chaining method accepting an string value
        /// </summary>
        /// <param name="baseAddress">The String for the request address</param>
        /// <returns>The caller object</returns>
        public IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(string baseAddress)
        {
            this.BaseAddress = new Uri(baseAddress);
            return this;
        }

        /// <summary>
        /// Chaining method accepting an string value containing the basic authentication value
        /// </summary>
        /// <param name="basicAuthentication">The basic authentication value</param>
        /// <returns>The caller object</returns>
        public IHeaderOrContentOrBodyOrVerb WithBasicAuthentication(string basicAuthentication)
        {
            this.BasicAuthentication = "Basic " + basicAuthentication;
            return this;
        }

        /// <summary>
        /// Chaining method accepting an string value containing the token value
        /// </summary>
        /// <param name="accessToken">The token value</param>
        /// <returns>The caller object</returns>
        public IHeaderOrContentOrBodyOrVerb WithBearerAccessToken(string accessToken)
        {
            this.AccessToken = "Bearer " + accessToken;
            return this;
        }

        /// <summary>
        /// Chaining method accepting header values to be sent with the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The caller object</returns>
        public IHeaderOrContentOrBodyOrVerb WithHeader(string key, string value)
        {
            this.Headers.Add(key, value);
            return this;
        }

        /// <summary>
        /// Chaining method accepting the ContentType of the value being sent with the request
        /// </summary>
        /// <param name="contentType">The ContentType value</param>
        /// <returns>The caller object</returns>
        public IBodyTextOrVerb WithContentType(ContentType contentType)
        {
            this.ContentType = contentType;
            return this;
        }

        /// <summary>
        /// Chaining method accepting the body values to be sent with the request
        /// </summary>
        /// <param name="key">The key</param>
        /// <param name="value">The value</param>
        /// <returns>The caller object</returns>
        public IBodyValueOrVerb WithBodyValue(string key, string value)
        {
            this.BodyValues.Add(key, value);
            return this;
        }

        /// <summary>
        /// Chaining method accepting an object to be parsed and sent with the request
        /// </summary>
        /// <param name="bodyObject">The object to be sent</param>
        /// <returns>The caller object</returns>
        public IVerb WithBodyObject(object bodyObject)
        {
            return this.WithBodyText(JsonConvert.SerializeObject(bodyObject, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        /// <summary>
        /// Chaining method accepting a body text value to be sent with the request
        /// </summary>
        /// <param name="bodyText">The body text to be sent</param>
        /// <returns>The caller object</returns>
        public IVerb WithBodyText(string bodyText)
        {
            this.BodyText = bodyText;
            this.BodyValues.Clear();
            return this;
        }

        /// <summary>
        /// Executes the post
        /// </summary>
        /// <returns>The caller object</returns>
        public IWitHttpClient Post()
        {
            return this.Post(null);
        }

        /// <summary>
        /// /// Executes the post
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        public IWitHttpClient Post(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Post;
            return this.Done(httpMessageHandler);
        }

        /// <summary>
        /// Executes the get
        /// </summary>
        /// <returns>The caller object</returns>
        public IWitHttpClient Get()
        {
            return this.Get(null);
        }

        /// <summary>
        /// /// Executes the get
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        public IWitHttpClient Get(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Get;
            return this.Done(httpMessageHandler);
        }

        /// <summary>
        /// Executes the put
        /// </summary>
        /// <returns>The caller object</returns>
        public IWitHttpClient Put()
        {
            return this.Put(null);
        }

        /// <summary>
        /// /// Executes the put
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        public IWitHttpClient Put(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Put;
            return this.Done(httpMessageHandler);
        }

        /// <summary>
        /// Executes the patch
        /// </summary>
        /// <returns>The caller object</returns>
        public IWitHttpClient Patch()
        {
            return this.Patch(null);
        }

        /// <summary>
        /// /// Executes the patch
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        public IWitHttpClient Patch(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Patch;
            return this.Done(httpMessageHandler);
        }

        /// <summary>
        /// Executes the delete
        /// </summary>
        /// <returns>The caller object</returns>
        public IWitHttpClient Delete()
        {
            return this.Delete(null);
        }

        /// <summary>
        /// /// Executes the delete
        /// </summary>
        /// <param name="httpMessageHandler">A custom HttpMessageHandler object</param>
        /// <returns>The caller object</returns>
        public IWitHttpClient Delete(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Delete;
            return this.Done(httpMessageHandler);
        }

        private IWitHttpClient Done(HttpMessageHandler httpMessageHandler)
        {
            if (httpMessageHandler == null)
                client = new HttpClient();
            else
                client = new HttpClient(httpMessageHandler);

            if (!string.IsNullOrEmpty(this.AccessToken))
            {
                Headers.Add("Authorization", this.AccessToken);
            }
            else if (!string.IsNullOrEmpty(this.BasicAuthentication))
            {
                Headers.Add("Authorization", this.BasicAuthentication);
            }

            if (!string.IsNullOrEmpty(BodyText))
            {
                content = new StringContent(BodyText, Encoding.UTF8, GetContentTypeHeaderValue());
            }
            else if (this.BodyValues.Count > 0)
            {
                content = new FormUrlEncodedContent(BodyValues);
            }

            foreach (KeyValuePair<string, string> pair in Headers)
            {
                client.DefaultRequestHeaders.Add(pair.Key, pair.Value);
            }

            return this;
        }

        private string GetContentTypeHeaderValue()
        {
            string retVal = string.Empty;

            switch (ContentType)
            {
                case ContentType.Text:
                    retVal = "text/plain";
                    break;
                case ContentType.ApplicationJson:
                    retVal = "application/json";
                    break;
                case ContentType.JavaScript:
                    retVal = "application/javascript";
                    break;
                case ContentType.ApplicationXML:
                    retVal = "application/xml";
                    break;
                case ContentType.TextXML:
                    retVal = "text/xml";
                    break;
                case ContentType.HTML:
                    retVal = "text/html";
                    break;
            }

            return retVal;
        }
        #endregion
    }
}
