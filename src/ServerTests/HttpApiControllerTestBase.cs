using System;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using Microsoft.Owin.Testing;
using Server;

namespace ServerTests
{
    public abstract class HttpApiControllerTestBase : IDisposable
    {
        protected HttpApiControllerTestBase()
        {
            Server = TestServer.Create<Startup>();
        }

        public void Dispose()
        {
            Server?.Dispose();
        }

        protected TestServer Server { get; private set; }

        protected virtual async Task<HttpResponseMessage> PostAsync<TModel>(string uri, TModel model)
        {
            return await Server.CreateRequest(uri)
                .And(request => request.Content = new ObjectContent(typeof(TModel), model, new JsonMediaTypeFormatter()))
                .PostAsync();
        }

        protected RequestBuilder CreateRequest(string uri)
        {
            return Server.CreateRequest(uri);
        }
    }
}
