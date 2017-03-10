using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp.Test
{
    public class MockHttpMessageHandlerBasicOk : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent("Ok")
            };

            return await Task.FromResult(responseMessage);
        }
    }

    public class MockHttpMessageHandlerBasic400 : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                Content = new StringContent("BadRequest")
            };

            return await Task.FromResult(responseMessage);
        }
    }

    public class MockHttpMessageHandlerBasic401 : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Unauthorized)
            {
                Content = new StringContent("Unauthorized")
            };

            return await Task.FromResult(responseMessage);
        }
    }

    public class MockHttpMessageHandlerBasic403 : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent("Forbidden")
            };

            return await Task.FromResult(responseMessage);
        }
    }

    public class MockHttpMessageHandlerBasic404 : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent("NotFound")
            };

            return await Task.FromResult(responseMessage);
        }
    }

    public class MockHttpMessageHandlerBasic500 : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent("InternalServerError")
            };

            return await Task.FromResult(responseMessage);
        }
    }
}
