namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    public class SignalPropertyTest : Test
    {
        [Test]
        public void GenerateBasicViewModelTest()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string?>> fullName;

}
";

            var expected =
                @"using System;

namespace Test;

public partial class TestClass
{
    public string FullName
    {
        get => this.fullName.Value?.Value;
        set => if(this.fullName.Value != null) this.fullName.Value.Value = value;
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }
    }
}
