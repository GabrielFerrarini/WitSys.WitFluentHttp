using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    public class WitHttpClient : IWitHttpClient,
        IAddress,
        IAuthOrHeaderOrContentOrBodyOrVerb,
        IHeaderOrContentOrBodyOrVerb,
        IBodyValueOrVerb,
        IBodyTextOrVerb,
        IVerb
    {
        public HttpVerb Verb { get; private set; }
        public Uri BaseAddress { get; private set; }
        public string AccessToken { get; private set; }
        public string BasicAuthentication { get; private set; }
        public ContentType ContentType { get; private set; }
        public IDictionary<string, string> Headers { get; private set; }
        public string BodyText { get; private set; }
        public IDictionary<string, string> BodyValues { get; private set; }

        private HttpClient client;
        private HttpContent content;

        internal WitHttpClient()
        {
            this.Headers = new Dictionary<string, string>();
            this.BodyValues = new Dictionary<string, string>();
            this.ContentType = ContentType.Text;
        }

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
        public IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(Uri baseAddress)
        {
            this.BaseAddress = baseAddress;
            return this;
        }

        public IAuthOrHeaderOrContentOrBodyOrVerb WithAddress(string baseAddress)
        {
            this.BaseAddress = new Uri(baseAddress);
            return this;
        }

        public IHeaderOrContentOrBodyOrVerb WithBasicAuthentication(string basicAuthentication)
        {
            this.BasicAuthentication = "Basic " + basicAuthentication;
            return this;
        }

        public IHeaderOrContentOrBodyOrVerb WithBearerAccessToken(string accessToken)
        {
            this.AccessToken = "Bearer " + accessToken;
            return this;
        }

        public IHeaderOrContentOrBodyOrVerb WithHeader(string key, string value)
        {
            this.Headers.Add(key, value);
            return this;
        }

        public IBodyValueOrVerb WithBodyValue(string key, string value)
        {
            this.BodyValues.Add(key, value);
            return this;
        }

        public IBodyTextOrVerb WithContentType(ContentType contentType)
        {
            this.ContentType = contentType;
            return this;
        }

        public IVerb WithBodyObject(object bodyObject)
        {
            return this.WithBodyText(JsonConvert.SerializeObject(bodyObject, new JsonSerializerSettings() { ContractResolver = new CamelCasePropertyNamesContractResolver() }));
        }

        public IVerb WithBodyText(string bodyText)
        {
            this.BodyText = bodyText;
            this.BodyValues.Clear();
            return this;
        }

        public IWitHttpClient Get()
        {
            return this.Get(null);
        }

        public IWitHttpClient Get(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Get;
            return this.Done(httpMessageHandler);
        }

        public IWitHttpClient Post()
        {
            return this.Post(null);
        }

        public IWitHttpClient Post(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Post;
            return this.Done(httpMessageHandler);
        }

        public IWitHttpClient Put()
        {
            return this.Put(null);
        }

        public IWitHttpClient Put(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Put;
            return this.Done(httpMessageHandler);
        }

        public IWitHttpClient Patch()
        {
            return this.Patch(null);
        }

        public IWitHttpClient Patch(HttpMessageHandler httpMessageHandler)
        {
            this.Verb = HttpVerb.Patch;
            return this.Done(httpMessageHandler);
        }

        public IWitHttpClient Delete()
        {
            return this.Delete(null);
        }

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
