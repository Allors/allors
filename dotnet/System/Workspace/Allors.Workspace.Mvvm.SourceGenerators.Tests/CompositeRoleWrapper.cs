namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    using Signals;

    public class CompositeRoleWrapper : ICompositeRoleWrapper<PersonViewModel> 
    {
        public PersonViewModel Value { get; set; }
    }
}
