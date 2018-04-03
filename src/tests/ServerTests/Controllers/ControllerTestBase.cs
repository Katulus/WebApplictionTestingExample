using FluentAssertions;
using Microsoft.AspNetCore.Mvc;

namespace ServerTests.Controllers
{
    public class ControllerTestBase
    {
        protected static T AssertResponseOfType<T>(IActionResult response) where T : class
        {
            return response.Should().BeOfType<T>("Wrong response type returned - {0} instead of {1}.", response.GetType().Name, typeof(T).Name).Subject;
        }
    }
}
