namespace ServerCore
{
    public class WizardStepDefinition
    {
        public WizardStepDefinition(string id, string controlName, string title, int order)
        {
            Id = id;
            ControlName = controlName;
            Title = title;
            Order = order;
        }

        public string Id { get; }

        public string Title { get; }

        public string ControlName { get; }

        public int Order { get; }
    }
}