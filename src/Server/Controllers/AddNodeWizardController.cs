using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Server.Models;

namespace Server.Controllers
{
    [RoutePrefix("wizard")]
    public class AddNodeWizardController : ApiController
    {
        private readonly IWizardSession _session;
        private readonly INodeService _nodeService;

        public AddNodeWizardController(IWizardSession session, INodeService nodeService)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));
            if (nodeService == null)
                throw new ArgumentNullException(nameof(nodeService));
            _session = session;

            _nodeService = nodeService;
        }

        [HttpGet]
        [Route("steps")]
        [ResponseType(typeof(IEnumerable<WizardStepDefinition>))]
        public IHttpActionResult GetSteps()
        {
            try
            {
                return Ok(_session.GetSteps().Select(x => x.StepDefinition));
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost]
        [Route("next")]
        [ResponseType(typeof(StepTransitionResult))]
        public IHttpActionResult Next(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            return Ok(_session.Next(node));
        }

        [HttpPost]
        [Route("back")]
        [ResponseType(typeof(StepTransitionResult))]
        public IHttpActionResult Back()
        {
            return Ok(_session.Back());
        }

        [HttpPost]
        [Route("cancel")]
        public IHttpActionResult Cancel()
        {
            _session.Cancel();
            return Ok();
        }

        [HttpPost]
        [Route("add")]
        public IHttpActionResult AddNode(Node node)
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
                return InternalServerError(ex);
            }
        }
    }
}