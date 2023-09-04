namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;

[Generator]
public class SignalPropertyGenerator : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context) { }

    public void Execute(GeneratorExecutionContext context)
    {
        var project = new Project(context);
        project.Build();
        project.Generate();
    }
}
