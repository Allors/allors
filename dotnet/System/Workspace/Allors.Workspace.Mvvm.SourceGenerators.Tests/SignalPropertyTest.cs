﻿namespace Allors.Workspace.Mvvm.SourceGenerators.Tests
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
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

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
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string>> firstName;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect firstNameChanged;

    public string FirstName
    {
        get => this.firstName.Value?.Value;
        set { if(this.firstName.Value != null) this.firstName.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.firstNameChanged = dispatcher.CreateEffect(tracker => this.firstName.Track(tracker), () => this.OnPropertyChanged(nameof(FirstName)));
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
        public void ComputedUnitRoleNullableValue()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<IUnitRole<int?>> number;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect numberChanged;

    public int? Number
    {
        get => this.number.Value?.Value;
        set { if(this.number.Value != null) this.number.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.numberChanged = dispatcher.CreateEffect(tracker => this.number.Track(tracker), () => this.OnPropertyChanged(nameof(Number)));
    }

    private void OnDisposeEffects()
    {
        this.numberChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedNullableUnitRole()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<IUnitRole<string>?> firstName;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

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
        this.firstNameChanged = dispatcher.CreateEffect(tracker => this.firstName.Track(tracker), () => this.OnPropertyChanged(nameof(FirstName)));
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
        public void ComputedNullableUnitRoleNullableValue()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<IUnitRole<int?>?> number;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect numberChanged;

    public int? Number
    {
        get => this.number.Value?.Value;
        set { if(this.number.Value != null) this.number.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.numberChanged = dispatcher.CreateEffect(tracker => this.number.Track(tracker), () => this.OnPropertyChanged(nameof(Number)));
    }

    private void OnDisposeEffects()
    {
        this.numberChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedUnitRoleWithDisplayName()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty(""Test Name"")] private readonly IComputedSignal<IUnitRole<string>> firstName;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect firstNameChanged;

    [System.ComponentModel.DisplayName(""Test Name"")]
    public string FirstName
    {
        get => this.firstName.Value?.Value;
        set { if(this.firstName.Value != null) this.firstName.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.firstNameChanged = dispatcher.CreateEffect(tracker => this.firstName.Track(tracker), () => this.OnPropertyChanged(nameof(FirstName)));
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
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

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

        [Test]
        public void ComputedViewModel()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel?> person;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect personChanged;

    public Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel? Person
    {
        get => this.person.Value;
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.personChanged = dispatcher.CreateEffect(tracker => this.person.Track(tracker), () => this.OnPropertyChanged(nameof(Person)));
    }

    private void OnDisposeEffects()
    {
        this.personChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedCompositeRoleWrapper()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<Allors.Workspace.Mvvm.SourceGenerators.Tests.CompositeRoleWrapper?> person;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect personChanged;

    public Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel Person
    {
        get => this.person.Value?.Value;
        set { if(this.person.Value != null) this.person.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.personChanged = dispatcher.CreateEffect(tracker => this.person.Track(tracker), () => this.OnPropertyChanged(nameof(Person)));
    }

    private void OnDisposeEffects()
    {
        this.personChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedICompositeRoleWrapper()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<ICompositeRoleWrapper<Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel>> person;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect personChanged;

    public Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel Person
    {
        get => this.person.Value?.Value;
        set { if(this.person.Value != null) this.person.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.personChanged = dispatcher.CreateEffect(tracker => this.person.Track(tracker), () => this.OnPropertyChanged(nameof(Person)));
    }

    private void OnDisposeEffects()
    {
        this.personChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedCompositesRoleWrapper()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<Allors.Workspace.Mvvm.SourceGenerators.Tests.CompositesRoleWrapper?> people;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect peopleChanged;

    public System.Collections.Generic.IEnumerable<Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel> People
    {
        get => this.people.Value?.Value;
        set { if(this.people.Value != null) this.people.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.peopleChanged = dispatcher.CreateEffect(tracker => this.people.Track(tracker), () => this.OnPropertyChanged(nameof(People)));
    }

    private void OnDisposeEffects()
    {
        this.peopleChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }

        [Test]
        public void ComputedICompositesRoleWrapper()
        {
            var source = @"
using System;
using Allors.Workspace;
using Allors.Workspace.Signals;
using Allors.Workspace.Mvvm.Generator;

namespace Signal.Test;

public partial class TestClass
{
    [SignalProperty] private readonly IComputedSignal<ICompositesRoleWrapper<Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel>> people;

}
";

            var expected =
                @"// <auto-generated/>
#pragma warning disable
#nullable enable
using System;
using Allors.Workspace.Signals;
using Allors.Workspace;

namespace Signal.Test;

public partial class TestClass
{
    private IEffect peopleChanged;

    public System.Collections.Generic.IEnumerable<Allors.Workspace.Mvvm.SourceGenerators.Tests.PersonViewModel> People
    {
        get => this.people.Value?.Value;
        set { if(this.people.Value != null) this.people.Value.Value = value; }
    }

    private void OnInitEffects(IDispatcher dispatcher)
    {
        this.peopleChanged = dispatcher.CreateEffect(tracker => this.people.Track(tracker), () => this.OnPropertyChanged(nameof(People)));
    }

    private void OnDisposeEffects()
    {
        this.peopleChanged?.Dispose();
    }
}
".ReplaceLineEndings("\n");

            string output = GetGeneratedOutput(source).ReplaceLineEndings("\n");

            Assert.NotNull(output);

            Assert.AreEqual(expected, output);
        }
    }
}
