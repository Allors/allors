namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    public class AdapterPropertyTest : Test
    {
        [Test]
        public void GenerateBasicViewModelTest()
        {
            var source = @"
using System;
using Allors.Workspace.Mvvm.Generator;
using Allors.Workspace.Mvvm.Adapters;

namespace Test;

public partial class TestClass
{
    [AdapterProperty] private readonly RoleAdapter<string> testAdapter;

}
";

            var expected =
                @"using System;

namespace Test;

public partial class TestClass
{
    public string TestAdapter
    {
        get => this.testAdapter.Value;
        set => this.testAdapter.Value = value;
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }
    }
}
