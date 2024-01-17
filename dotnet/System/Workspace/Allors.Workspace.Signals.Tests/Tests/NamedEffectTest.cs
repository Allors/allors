namespace Allors.Workspace.Signals.Tests
{
    using Domain;
    using Task = System.Threading.Tasks.Task;

    public class NamedEffectTest : Test
    {
        [Test]
        public async Task UnitRoles()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();

            var list = new List<string>();

            using var effect = new NamedEffect((name) =>
            {
                list.Add(name);
            }, v =>
            {
                v.Add(c1a.C1AllorsString);
            });

            Assert.That(list, Has.Count.EqualTo(0));

            c1a.C1AllorsString.Value = "Hello A!";

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list, Contains.Item("C1AllorsString"));

            list.Clear();

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(list, Has.Count.EqualTo(0));

            effect.Add(c1b.C1AllorsString, "C1B");
            effect.Add(c1c.C1AllorsString, "C1C");

            c1b.C1AllorsString.Value = "Hello again B!";

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list, Contains.Item("C1B"));

            list.Clear();

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list, Contains.Item("C1B"));
        }

        //[Test]
        //public async Task Dispose()
        //{
        //    await this.Login("jane@example.com");
        //    var workspace = this.Workspace;

        //    var c1a = workspace.Create<C1>();

        //    var counter = 0;

        //    using var effect = new Effect(() =>
        //    {
        //        ++counter;
        //    }, c1a.C1AllorsString);

        //    Assert.That(counter, Is.EqualTo(0));

        //    c1a.C1AllorsString.Value = "Hello A!";

        //    Assert.That(counter, Is.EqualTo(1));

        //    effect.Dispose();

        //    c1a.C1AllorsString.Value = "Hello again A!";

        //    Assert.That(counter, Is.EqualTo(1));
        //}

        [Test]
        public async Task Computed()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;

            var person = workspace.Create<Person>();

            var list = new List<string>();

            var model = new ValueSignal<Person>(person);

            var computed = new ComputedSignal<IUnitRole<string>?>(tracker => model.Track(tracker).Value.FirstName.Track(tracker));

            var value = computed.Value;

            using var computedEffect = new NamedEffect((name) => list.Add(name), v => v.Add(computed, "Computed"));

            Assert.That(list, Has.Count.EqualTo(0));

            person.FirstName.Value += "!";

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list, Contains.Item("Computed"));

            list.Clear();

            person.FirstName.Value += "!";

            Assert.That(list, Has.Count.EqualTo(1));
            Assert.That(list, Contains.Item("Computed"));

            // Missing name
            Assert.Catch<ArgumentNullException>(() =>
            {
                using var effectWithoutName = new NamedEffect((name) => list.Add(name), v => v.Add(computed));
            });
        }
    }
}
