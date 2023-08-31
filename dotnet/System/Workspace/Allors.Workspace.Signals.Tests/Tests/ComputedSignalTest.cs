namespace Allors.Workspace.Signals.Tests
{
    using System.Linq.Expressions;
    using Configuration;
    using Data;
    using Domain;

    public class ComputedSignalTest : Test
    {
        [Test]
        [TestCaseSource(nameof(TestImplementations))]
        public async Task Roles(Implementations implementation)
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            SelectImplementation(workspace, implementation);

            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            var calculatedSignal = dispatcher.CreateComputedSignal((tracker) => c1a?
                .C1C1One2One.Track(tracker).Value?
                .C1C1One2One.Track(tracker).Value?
                .C1AllorsString.Track(tracker).Value);

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello"));

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1b.C1C1One2One.Value = c1d;

            Assert.That(calculatedSignal.Value, Is.EqualTo("Another Hello"));
        }

        [Test]
        [TestCaseSource(nameof(TestImplementations))]
        public async Task ValueSignalWithRoles(Implementations implementation)
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            SelectImplementation(workspace, implementation);

            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            var c1e = workspace.Create<C1>();
            var c1f = workspace.Create<C1>();
            var c1g = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            c1e.C1C1One2One.Value = c1f;
            c1f.C1C1One2One.Value = c1g;
            c1g.C1AllorsString.Value = "Hello 2";

            var signal = dispatcher.CreateValueSignal<C1>(null);

            var calculatedSignal = dispatcher.CreateComputedSignal((tracker) => signal.Track(tracker).Value?
                .C1C1One2One.Track(tracker).Value?
                .C1C1One2One.Track(tracker).Value?
                .C1AllorsString.Track(tracker)?.Value);

            Assert.That(calculatedSignal.Value, Is.Null);

            signal.Value = c1a;

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello"));

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1b.C1C1One2One.Value = c1d;

            Assert.That(calculatedSignal.Value, Is.EqualTo("Another Hello"));

            signal.Value = c1e;

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello 2"));
        }

    }
}
