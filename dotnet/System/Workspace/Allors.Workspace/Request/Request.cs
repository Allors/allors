namespace Allors.Workspace.Request
{
    using Visitor;

    public class Request : IVisitable
    {
        public InvokeOptions InvokeOptions { get; }

        public IInvocable[] Invocables { get; }

        public Pull[] Pulls { get; set; }


        public Request(IInvocable[] invocables, InvokeOptions invokeOptions)
        {
            this.Invocables = invocables;
            this.InvokeOptions = invokeOptions;
        }

        public Request(IInvocable[] invocables, InvokeOptions invokeOptions, Pull[] pulls)
        {
            this.Invocables = invocables;
            this.InvokeOptions = invokeOptions;
            this.Pulls = pulls;
        }

        public Request(Pull[] pulls) => this.Pulls = pulls;

        public void Accept(IVisitor visitor) => visitor.VisitInvocation(this);
    }
}
