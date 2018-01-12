namespace Server
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

        public string Id { get; private set; }

        public string Title { get; private set; }

        public string ControlName { get; private set; }

        public int Order { get; private set; }
    }
}