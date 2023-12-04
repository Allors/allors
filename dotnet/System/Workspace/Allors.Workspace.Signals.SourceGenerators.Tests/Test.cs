namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    using System.Diagnostics;
    using System.Reflection;
    using Generator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Signals;

    public class Test
    {

        [SignalProperty] private IComputedSignal<IUnitRole<string?>> fullName;

        [SetUp]
        public void TestSetup()
        {
            // trigger the activation of the signal Assembly
            IDispatcher dispatcher = null;
            this.fullName = dispatcher?.CreateComputedSignal<IUnitRole<string?>>((tracker) => null);
        }

        protected string GetGeneratedOutput(string source)
        {
            var syntaxTree = CSharpSyntaxTree.ParseText(source);

            var references = new List<MetadataReference>();
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                if (!assembly.IsDynamic && !string.IsNullOrWhiteSpace(assembly.Location))
                {
                    references.Add(MetadataReference.CreateFromFile(assembly.Location));
                }
            }

            var compilation = CSharpCompilation.Create("foo", new SyntaxTree[] { syntaxTree }, references, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

            // fail if the injected program isn't valid before running generators
            var compileDiagnostics = compilation.GetDiagnostics();
            Assert.That(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), Is.False, "Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

            IIncrementalGenerator generator = new SignalPropertyGenerator();
            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

            // fail if the generation had errors
            Assert.That(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), Is.False, "Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

            string output = outputCompilation.SyntaxTrees.Last().ToString();
            return output;
        }
    }
}
