namespace Server
{
    public class StepTransitionResult
    {
        public static StepTransitionResult Success()
        {
            return new StepTransitionResult { CanTransition = true };
        }

        public static StepTransitionResult Failure(string error)
        {
            return new StepTransitionResult { CanTransition = false, ErrorMessage = error };
        }

        public bool CanTransition { get; set; }
        public string ErrorMessage { get; set; }
    }
}