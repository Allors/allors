namespace Allors.Workspace.Mvvm.Generator;

using Microsoft.CodeAnalysis;

[Generator]
public class SignalPropertyGenerator : IIncrementalGenerator
{
    public Configuration Configuration { get; private set; }

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var configuration = context.AnalyzerConfigOptionsProvider;
        var compilation = context.CompilationProvider;
        var combined = compilation.Combine(configuration);

        context.RegisterSourceOutput(combined, (sourceProductionContext, pair) =>
        {
            var compilation = pair.Left;
            var analyzerConfigOptions = pair.Right;

            analyzerConfigOptions.GlobalOptions.TryGetValue("build_property.signalssourcegenerator_oneffect", out var onEffect);
            var configuration = new Configuration(onEffect);

            //System.Diagnostics.Debugger.Launch();

            var project = new Project(sourceProductionContext, compilation, configuration);
            project.Build();
            project.Generate();
        });
    }
}
