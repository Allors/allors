namespace Allors.Workspace.Signals.Tests
{
    using Domain;
    using Task = System.Threading.Tasks.Task;

    public class ComputedSignalTest : Test
    {
        [Test]

        public async Task Roles()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            var computedSignal = new ComputedSignal<string>((tracker) => c1a?
                .C1C1One2One.Track(tracker).Value?
                .C1C1One2One.Track(tracker).Value?
                .C1AllorsString.Track(tracker).Value);

            Assert.That(computedSignal.Value, Is.EqualTo("Hello"));

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.That(computedSignal.Value, Is.EqualTo("Hello Again"));

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.That(computedSignal.Value, Is.EqualTo("Hello Again"));

            c1b.C1C1One2One.Value = c1d;

            Assert.That(computedSignal.Value, Is.EqualTo("Another Hello"));
        }

        [Test]

        public async Task ValueSignalWithRoles()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

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

            var signal = new ValueSignal<C1>(null);

            var calculatedSignal = new ComputedSignal<string>((tracker) => signal.Track(tracker).Value?
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
