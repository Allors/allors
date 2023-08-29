namespace Allors.Workspace.Signals.Tests
{
    using System.Linq.Expressions;
    using Data;
    using Domain;

    public class EffectTest : Test
    {
        [Test]
        public async Task UnitRoles()
        {
            await this.Login("jane@example.com");

            var workspace = this.Workspace;

            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();

            var multiple = false;

            var counter = 0;

            var effect = dispatcher.CreateEffect(v =>
            {
                v.Track(c1a.C1AllorsString);
                if (multiple)
                {
                    v.Track(c1b.C1AllorsString);
                    v.Track(c1c.C1AllorsString);
                }
            }, () =>
            {
                ++counter;
            });

            Assert.That(counter, Is.EqualTo(1));

            c1a.C1AllorsString.Value = "Hello A!";

            Assert.That(counter, Is.EqualTo(2));

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(counter, Is.EqualTo(2));

            multiple = true;

            c1b.C1AllorsString.Value = "Hello again B!";

            Assert.That(counter, Is.EqualTo(3));

            c1b.C1AllorsString.Value = "Hello B!";

            Assert.That(counter, Is.EqualTo(4));
        }
    }
}
