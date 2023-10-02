namespace Allors.Workspace.Signals.Tests
{
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

            using var effect = dispatcher.CreateEffect(v =>
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

        [Test]
        
        public async Task Dispose()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            

            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var c1a = workspace.Create<C1>();

            var counter = 0;

            using var effect = dispatcher.CreateEffect(v =>
            {
                v.Track(c1a.C1AllorsString);
            }, () =>
            {
                ++counter;
            });

            Assert.That(counter, Is.EqualTo(1));

            c1a.C1AllorsString.Value = "Hello A!";

            Assert.That(counter, Is.EqualTo(2));

            effect.Dispose();

            c1a.C1AllorsString.Value = "Hello again A!";

            Assert.That(counter, Is.EqualTo(2));
        }

        [Test]
        
        public async Task CombinedDependencies()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            

            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var person = workspace.Create<Person>();

            var counter = 0;

            IValueSignal<Person> model = dispatcher.CreateValueSignal(person);
            IComputedSignal<IUnitRole<string>?> combinedModel = dispatcher.CreateComputedSignal(tracker => model.Track(tracker).Value.FirstName.Track(tracker));
            IEffect combinedModelChanged = dispatcher.CreateEffect(tracker => 
                
                
                combinedModel.Track(tracker)
                
                
                , () => ++counter);

            Assert.That(counter, Is.EqualTo(1));

            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(2));

            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(3));
            
            person.FirstName.Value += "!";

            Assert.That(counter, Is.EqualTo(4));
        }
    }
}
