using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Http.Description;
using Server.Models;

namespace Server.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class NodesController : ApiController
    {
        private readonly INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            _nodeService = nodeService ?? throw new ArgumentNullException(nameof(nodeService));
        }

        [HttpGet]
        [Route("nodes")]
        [ResponseType(typeof(IEnumerable<Node>))]
        public IHttpActionResult GetNodes()
        {
            return Ok(_nodeService.GetNodes());
        }

        [HttpPost]
        [Route("deleteAll")]
        public IHttpActionResult DeleteAll()
        {
            _nodeService.DeleteAll();
            return Ok();
        }
    }
}