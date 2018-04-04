using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    public class NodesController : Controller
    {
        private readonly INodeService _nodeService;

        public NodesController(INodeService nodeService)
        {
            _nodeService = nodeService ?? throw new ArgumentNullException(nameof(nodeService));
        }

        [HttpGet]
        [Route("nodes")]
        [ProducesResponseType(typeof(IEnumerable<Node>), 200)]
        public IActionResult GetNodes()
        {
            return Ok(_nodeService.GetNodes());
        }

        [HttpPost]
        [Route("deleteAll")]
        public IActionResult DeleteAll()
        {
            _nodeService.DeleteAll();
            return Ok();
        }
    }
}