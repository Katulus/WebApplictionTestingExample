using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using Server.Models;

namespace Server.Controllers
{
    public class NodesController : ApiController
    {
        private readonly INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            if (nodeService == null)
                throw new ArgumentNullException(nameof(nodeService));

            _nodeService = nodeService;
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