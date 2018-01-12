using System.Web.Http;
using NUnit.Framework;

namespace ServerTests.Controllers
{
    public class ControllerTestBase
    {
        protected static T AssertResponseOfType<T>(IHttpActionResult response) where T : class
        {
            T typedResponse = response as T;
            Assert.That(typedResponse, Is.Not.Null, "Wrong response type returned - {0} instead of {1}.", response.GetType().Name, typeof(T).Name);
            return typedResponse;
        }
    }
}
