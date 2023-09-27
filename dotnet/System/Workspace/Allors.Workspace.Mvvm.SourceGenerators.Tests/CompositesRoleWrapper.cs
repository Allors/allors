namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    using Signals;

    public class CompositesRoleWrapper : ICompositesRoleWrapper<PersonViewModel> 
    {
        public IEnumerable<PersonViewModel> Value { get; set; }
    }
}
