namespace Allors.Workspace.Signals.Tests
{
    using Domain;
    using Task = System.Threading.Tasks.Task;

    public class EffectTest : Test
    {
        [Test]

        public async Task UnitRoles()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();

            var multiple = false;

            var counter = 0;

            using var effect = new Effect(() =>
            {
                ++counter;
            }, c1a.C1AllorsString);

            Assert.That(counter, Is.EqualTo(0));

            c1a.C1AllorsString.Value = "Hello A!";

            Assert.That(counter, Is.EqualTo(1));

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(counter, Is.EqualTo(1));

            effect.Add(c1b.C1AllorsString);
            effect.Add(c1c.C1AllorsString);

            c1b.C1AllorsString.Value = "Hello again B!";

            Assert.That(counter, Is.EqualTo(2));

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(counter, Is.EqualTo(3));
        }

        [Test]

        public async Task Dispose()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var c1a = workspace.Create<C1>();

            var counter = 0;

            using var effect = new Effect(() =>
            {
                ++counter;
            }, c1a.C1AllorsString);

            Assert.That(counter, Is.EqualTo(0));

            c1a.C1AllorsString.Value = "Hello A!";

            Assert.That(counter, Is.EqualTo(1));

            effect.Dispose();

            c1a.C1AllorsString.Value = "Hello again A!";

            Assert.That(counter, Is.EqualTo(1));
        }

        [Test]

        public async Task Computed()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var person = workspace.Create<Person>();

            var counter = 0;

            var model = new ValueSignal<Person>(person);
            var computed = new ComputedSignal<IUnitRole<string>?>(tracker => model.Track(tracker).Value.FirstName.Track(tracker));

            var value = computed.Value;

            using var computedEffect = new Effect(() => ++counter, computed);

            Assert.That(counter, Is.EqualTo(0));

            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(1));

            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(2));

            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(3));
        }
    }
}
