namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    using System.Reflection;
    using System.Text;
    using Generator;
    using Microsoft.CodeAnalysis;
    using Microsoft.CodeAnalysis.CSharp;
    using Microsoft.CodeAnalysis.Text;
    using NUnit.Framework.Internal;
    using VerifyCS = CSharpSourceGeneratorVerifier<Generator.ViewModelGenerator>;

    public class ViewModelGeneratorTests
    {

        [Test]
        public async Task Test1()
        {
            var code = $@"
using System;

namespace Test;

public partial class TestClass
{{
    [AdapterProperty] private readonly RoleAdapter<string> testAdapter;

}}

";
            var generated = $@"
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
            await new VerifyCS.Test
            {
                TestState =
                {
                    Sources = { code },
                    GeneratedSources =
                    {
                        (typeof(ViewModelGenerator), "TestClass.g.cs", SourceText.From(generated, Encoding.UTF8, SourceHashAlgorithm.Sha256)),
                    },
                },
            }.RunAsync();
        }
    }
}
