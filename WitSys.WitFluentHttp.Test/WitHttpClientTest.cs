using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Net;
using System.Threading.Tasks;

namespace WitSys.WitFluentHttp.Test
{
    [TestClass]
    public class WitHttpClientTest
    {
        [TestMethod]
        public void WitHttpClientBasicTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Get, client.Verb);
        }

        [TestMethod]
        public async Task WitHttpClientGetOkTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasicOk());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual<string>("Ok", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        [TestMethod]

        public async Task WitHttpClientGetBadRequestTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasic400());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.AreEqual<string>("BadRequest", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        public async Task WitHttpClientGetUnauthorizedTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasic401());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Unauthorized, response.StatusCode);
            Assert.AreEqual<string>("Unauthorized", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        [TestMethod]
        public async Task WitHttpClientGetForbiddenTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasic403());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.Forbidden, response.StatusCode);
            Assert.AreEqual<string>("Forbidden", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        [TestMethod]
        public async Task WitHttpClientGetNotFoundTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasic404());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.NotFound, response.StatusCode);
            Assert.AreEqual<string>("NotFound", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        [TestMethod]
        public async Task WitHttpClientGetInternalServerErrorTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get(new MockHttpMessageHandlerBasic500());

            WitHttpResponse response = await client.ExecuteAsync();

            Assert.AreEqual<HttpStatusCode>(HttpStatusCode.InternalServerError, response.StatusCode);
            Assert.AreEqual<string>("InternalServerError", response.Content);
            Assert.AreEqual<string>(baseAddress.AbsoluteUri, response.ResquestedUrl);
        }

        [TestMethod]
        public void WitHttpClientDefaultPropertiesTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Get();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Get, client.Verb);
            TestDefaultValues(client);

            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Post();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Post, client.Verb);
            TestDefaultValues(client);

            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Put();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Put, client.Verb);
            TestDefaultValues(client);

            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Patch();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Patch, client.Verb);
            TestDefaultValues(client);

            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .Delete();

            Assert.AreEqual<Uri>(baseAddress, client.BaseAddress);
            Assert.AreEqual<HttpVerb>(HttpVerb.Delete, client.Verb);
            TestDefaultValues(client);
        }

        [TestMethod]
        public void WitHttpClienAccessTokenTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithBearerAccessToken("1234567890abcdefghijklmnopqrstuvwxyz")
                .Get();

            Assert.AreEqual<string>("Bearer 1234567890abcdefghijklmnopqrstuvwxyz", client.AccessToken);
            Assert.IsNull(client.BasicAuthentication);
        }

        [TestMethod]
        public void WitHttpClienBasicAuthenticationTest()
        {
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithBasicAuthentication("sdkcCDRT676778")
                .Get();

            Assert.AreEqual<string>("Basic sdkcCDRT676778", client.BasicAuthentication);
            Assert.IsNull(client.AccessToken);
        }

        [TestMethod]
        public void WitHttpClienBodyTextAndBodyValuesTest()
        {
            string json = "{\"jsonParent\":{\"name\":\"This is My Awesome Name\",\"jsonChild\":{\"name\":\"This is My Awesome Name\"}}}";
            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithContentType(ContentType.ApplicationJson)
                .WithBodyText(json)
                .Post();

            Assert.AreEqual<string>(json, client.BodyText);
            Assert.AreEqual<ContentType>(ContentType.ApplicationJson, client.ContentType);

            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithBodyValue("key1", "Value 1")
                .WithBodyValue("key2", "Value 2")
                .Post();

            Assert.AreEqual<int>(2, client.BodyValues.Count);
            Assert.AreEqual<string>("Value 1", client.BodyValues["key1"]);
            Assert.AreEqual<string>("Value 2", client.BodyValues["key2"]);
        }

        [TestMethod]
        public void WitHttpClienBodyObjectTest()
        {
            string json = "{\"name\":\"This is My Awesome Name\",\"age\":30}";
            object obj = new
            {
                Name = "This is My Awesome Name",
                Age = 30
            };

            Uri baseAddress = new Uri("http://localhost:8080");
            IWitHttpClient client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithContentType(ContentType.ApplicationJson)
                .WithBodyObject(obj)
                .Post();

            Assert.AreEqual<string>(json, client.BodyText);
            Assert.AreEqual<ContentType>(ContentType.ApplicationJson, client.ContentType);

            json = "{\"name\":\"This is My Awesome Name\",\"age\":30,\"address\":{\"street\":\"My Street\",\"number\":200,\"city\":\"Paradise City\"}}";
            obj = new
            {
                Name = "This is My Awesome Name",
                Age = 30,
                Address = new
                {
                    Street = "My Street",
                    Number = 200,
                    City = "Paradise City"
                }
            };

            baseAddress = new Uri("http://localhost:8080");
            client = WitHttpClientFactory.Instance.Create()
                .WithAddress(baseAddress)
                .WithContentType(ContentType.ApplicationJson)
                .WithBodyObject(obj)
                .Post();

            Assert.AreEqual<string>(json, client.BodyText);
            Assert.AreEqual<ContentType>(ContentType.ApplicationJson, client.ContentType);
        }

        private void TestDefaultValues(IWitHttpClient client)
        {
            Assert.IsNull(client.AccessToken);
            Assert.IsNull(client.BodyText);
            Assert.AreEqual<int>(0, client.BodyValues.Count);
            Assert.AreEqual<int>(0, client.Headers.Count);
            Assert.AreEqual<ContentType>(ContentType.Text, client.ContentType);
        }
    }
}