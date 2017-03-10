using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp
{
    public class WitHttpResponse
    {
        public HttpStatusCode StatusCode { get; private set; }
        public string Content { get; private set; }
        public HttpResponseMessage HttpResponseMessage { get; private set; }
        public string ResquestedUrl { get; private set; }

        public WitHttpResponse(HttpResponseMessage responseMessage, string requestedUrl)
        {
            this.StatusCode = responseMessage.StatusCode;
            this.ResquestedUrl = requestedUrl;
            this.HttpResponseMessage = responseMessage;
            this.Content = GetResponseStream(responseMessage);
        }

        private string GetResponseStream(HttpResponseMessage responseMessage)
        {
            string retVal = string.Empty;
            Stream content;

            Task<Stream> taskReadStream = Task<Stream>.Run(() =>
            {
                if (responseMessage == null || responseMessage.Content == null)
                {
                    return Task<Stream>.Run<Stream>(() =>
                    { return Stream.Null; });
                }
                return responseMessage.Content.ReadAsStreamAsync();
            });

            taskReadStream.Wait();
            content = taskReadStream.Result;

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
