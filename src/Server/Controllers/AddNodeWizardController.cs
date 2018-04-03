using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace Server.Controllers
{
    [Route("wizard")]
    public class AddNodeWizardController : Controller
    {
        private readonly IWizardSession _session;
        private readonly INodeService _nodeService;

        public AddNodeWizardController(IWizardSession session, INodeService nodeService)
        {
            _session = session ?? throw new ArgumentNullException(nameof(session));
            _nodeService = nodeService ?? throw new ArgumentNullException(nameof(nodeService));
        }

        [HttpGet]
        [Route("steps")]
        [ProducesResponseType(typeof(IEnumerable<WizardStepDefinition>), StatusCodes.Status200OK)]
        public IActionResult GetSteps()
        {
            try
            {
                return Ok(_session.GetSteps().Select(x => x.StepDefinition));
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpPost]
        [Route("next")]
        [ProducesResponseType(typeof(StepTransitionResult), StatusCodes.Status200OK)]
        public IActionResult Next([FromBody] Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return Ok(_session.Next(node));
        }

        [HttpPost]
        [Route("back")]
        [ProducesResponseType(typeof(StepTransitionResult), StatusCodes.Status200OK)]
        public IActionResult Back()
        {
            return Ok(_session.Back());
        }

        [HttpPost]
        [Route("cancel")]
        public IActionResult Cancel()
        {
            _session.Cancel();
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public IActionResult AddNode([FromBody] Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            try
            {
                _nodeService.AddNode(node);
                _session.Cancel();
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }
    }
}