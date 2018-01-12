using System.Collections.Generic;
using System.Linq;
using Server.WizardSteps;

namespace Server
{
    public interface IWizardStepsProvider
    {
        IEnumerable<IWizardStep> GetWizardSteps();
    }

    public class WizardStepsProvider : IWizardStepsProvider
    {
        public IEnumerable<IWizardStep> GetWizardSteps()
        {
            // this would normally load steps from some configuration ...
            List<IWizardStep> steps = new List<IWizardStep>
            {
                new DefineNodeWizardStep(),
                new SummaryWizardStep()
            };

            return steps.OrderBy(x => x.StepDefinition.Order);
        }
    }
}