namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
{
    public class SignalPropertyTest : Test
    {
        [Test]
        public void ValueSignal()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IValueSignal<string> article;

}
";

            var expected =
                @"using System;
using Allors.Workspace.Signals;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect articleChanged;

    public string Article
    {
        get => this.article.Value;
        set => this.article.Value = value;
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.articleChanged = dispatcher.CreateEffect(tracker => this.article.Track(tracker), () => this.OnPropertyChanged(nameof(Article)));
    }

    private void OnDisposeEffects()
    {
        this.articleChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

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
using Allors.Workspace.Signals;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect firstNameChanged;

    public string? FirstName
    {
        get => this.firstName.Value?.Value;
        set { if(this.firstName.Value != null) this.firstName.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.firstNameChanged = dispatcher.CreateEffect(tracker => this.firstName.Track(tracker).Value?.Track(tracker), () => this.OnPropertyChanged(nameof(FirstName)));
    }

    private void OnDisposeEffects()
    {
        this.firstNameChanged?.Dispose();
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
using Allors.Workspace.Signals;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect fullNameChanged;

    public string? FullName
    {
        get => this.fullName.Value;
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.fullNameChanged = dispatcher.CreateEffect(tracker => this.fullName.Track(tracker), () => this.OnPropertyChanged(nameof(FullName)));
    }

    private void OnDisposeEffects()
    {
        this.fullNameChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }
    }
}
