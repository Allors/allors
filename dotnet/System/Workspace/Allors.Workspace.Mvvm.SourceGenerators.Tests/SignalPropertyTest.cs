namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    public class SignalPropertyTest : Test
    {
        [Test]
        public void ComputedUnitRole()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string?>> firstName;

}
";

            var expected =
                @"using System;

namespace Signal.Test;

public partial class TestClass
{
    public string? FirstName
    {
        get => this.firstName.Value?.Value;
        set { if(this.firstName.Value != null) this.firstName.Value.Value = value; }
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedString()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<string?> fullName;

}
";

            var expected =
                @"using System;

namespace Signal.Test;

public partial class TestClass
{
    public string? FullName
    {
        get => this.fullName.Value;
        set => this.fullName.Value = value;
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }
    }
}
