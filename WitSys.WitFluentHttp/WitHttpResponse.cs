using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    /// <summary>
    /// The class type used to parse the http responses in a more convinient way
    /// </summary>
    public class WitHttpResponse
    {
        /// <summary>
        /// The response status code
        /// </summary>
        public HttpStatusCode StatusCode { get; private set; }
        /// <summary>
        /// The parsed response body, read for reading
        /// </summary>
        public string Content { get; private set; }
        /// <summary>
        /// The original HttpResponseMessage for more advanced inspection
        /// </summary>
        public HttpResponseMessage HttpResponseMessage { get; private set; }
        /// <summary>
        /// The original requested url
        /// </summary>
        public string ResquestedUrl { get; private set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="responseMessage"></param>
        /// <param name="requestedUrl"></param>
        public WitHttpResponse(HttpResponseMessage responseMessage, string requestedUrl)
        {
            this.StatusCode = responseMessage.StatusCode;
            this.ResquestedUrl = requestedUrl;
            this.HttpResponseMessage = responseMessage;
            this.Content = GetResponseContent(responseMessage);
        }

        private string GetResponseContent(HttpResponseMessage responseMessage)
        {
            Task<Stream> taskReadStream = GetResponseContentStream(responseMessage);

            taskReadStream.Wait();

            return GetResponseContentFromStream(taskReadStream.Result);
        }

        private Task<Stream> GetResponseContentStream(HttpResponseMessage responseMessage)
        {
            return Task<Stream>.Run(() =>
            {
                if (responseMessage == null || responseMessage.Content == null)
                {
                    return Task<Stream>.Run<Stream>(() =>
                    { return Stream.Null; });
                }
                return responseMessage.Content.ReadAsStreamAsync();
            });
        }

        private string GetResponseContentFromStream(Stream content)
        {
            string retVal = string.Empty;

            if (content != null)
            {
                using (StreamReader reader = new StreamReader(content))
                {
                    Task<string> taskReadContent = Task<string>.Run(() =>
                    {
                        return reader.ReadToEndAsync();
                    });

                    taskReadContent.Wait();
                    retVal = taskReadContent.Result;
                }
            }

            return retVal;
        }
    }
}
