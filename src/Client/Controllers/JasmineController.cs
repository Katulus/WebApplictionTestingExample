using System;
using System.Web.Mvc;

namespace Client.Controllers
{
    [Route("jasmine")]
    public class JasmineController : Controller
    {
        [Route("run")]
        public ViewResult Run()
        {
            return View("SpecRunner");
        }
    }
}
