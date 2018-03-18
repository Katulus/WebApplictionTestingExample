using System;
using System.Collections.Generic;
using System.Linq;
using ServerCore.Models;

namespace ServerCore
{
    public interface IWizardSession
    {
        StepTransitionResult Next(Node node);
        StepTransitionResult Back();
        void Cancel();

        IEnumerable<IWizardStep> GetSteps();
    }

    public class WizardSession : IWizardSession
    {
        private static int _currentIndex; 
        private readonly List<IWizardStep> _steps;

        public WizardSession(IWizardStepsProvider stepsProvider)
        {
            if (stepsProvider == null)
                throw new ArgumentNullException(nameof(stepsProvider));

            _steps = stepsProvider.GetWizardSteps().ToList();
        }

        public StepTransitionResult Next(Node node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));

            if (!CanGoForward)
                return StepTransitionResult.Failure("This is the last step");

            StepTransitionResult result = _steps[_currentIndex].Next(node);
            if (result.CanTransition)
            {
                _currentIndex++;
            }
            return result;
        }

        public StepTransitionResult Back()
        {
            if (!CanGoBack)
                return StepTransitionResult.Failure("This is the first step");

            _currentIndex--;
            return StepTransitionResult.Success();
        }

        public void Cancel()
        {
            _currentIndex = 0;
        }

        public IWizardStep CurrentStep
        {
            get { return _steps[_currentIndex]; }
        }

        public IEnumerable<IWizardStep> GetSteps()
        {
            return _steps;
        }

        private bool CanGoForward
        {
            get { return _currentIndex < _steps.Count - 1; }
        }

        private bool CanGoBack
        {
            get { return _currentIndex > 0; }
        }
    }
}