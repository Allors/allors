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

        private IValueSignal<string>? mySignal;

        [SetUp]
        public void TestSetup()
        {
            // trigger the activation of the signal Assembly
            IDispatcher dispatcher = null;
            this.mySignal = dispatcher?.CreateValueSignal<string>("Hello");
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
            Assert.False(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

            ISourceGenerator generator = new SignalPropertyGenerator();
            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);

            // fail if the generation had errors
            Assert.False(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

            string output = outputCompilation.SyntaxTrees.Last().ToString();
            return output;
        }
    }
}
