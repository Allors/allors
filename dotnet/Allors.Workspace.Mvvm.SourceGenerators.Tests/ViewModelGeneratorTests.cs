namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    using System.Reflection;
    using Generator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;

    public class ViewModelGeneratorTests
    {

        [Test]
        public void GenerateBasicViewModelTest()
        {
            var source = $@"
using System;
using Allors.Workspace.Mvvm;

namespace Test;

public partial class TestClass
{{
    [AdapterProperty] private readonly RoleAdapter<string> testAdapter;

}}

";
            var expected = $@"
using System;

namespace Test;

public partial class TestClass
{{
    public string TestAdapter
    {{
        get => this.testAdapter.Value;
        set => this.testAdapter.Value = value;
    }}

}}

";

            string output = GetGeneratedOutput(source);

            Assert.NotNull(output);

            Assert.AreEqual(expected, output.Trim());
        }

        private string GetGeneratedOutput(string source)
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

            // TODO: Uncomment this line if you want to fail tests when the injected program isn't valid _before_ running generators
            var compileDiagnostics = compilation.GetDiagnostics();
            Assert.False(compileDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + compileDiagnostics.FirstOrDefault()?.GetMessage());

            ISourceGenerator generator = new ViewModelGenerator();

            var driver = CSharpGeneratorDriver.Create(generator);
            driver.RunGeneratorsAndUpdateCompilation(compilation, out var outputCompilation, out var generateDiagnostics);
            Assert.False(generateDiagnostics.Any(d => d.Severity == DiagnosticSeverity.Error), "Failed: " + generateDiagnostics.FirstOrDefault()?.GetMessage());

            string output = outputCompilation.SyntaxTrees.Last().ToString();

            return output;
        }
    }
}
