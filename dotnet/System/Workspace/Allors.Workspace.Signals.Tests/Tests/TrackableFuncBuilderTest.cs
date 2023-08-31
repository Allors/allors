namespace Allors.Workspace.Signals.Tests
{
    using System.Linq.Expressions;
    using Data;
    using Domain;

    public class TrackableFuncBuilderTest : Test
    {
        [Test]
        public async Task Roles()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            var reactiveFuncBuilder = workspace.Services.Get<ITrackableFuncBuilder>();

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            Expression<Func<string>> expression = () => c1a.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString.Value;

            var trackableFunc = reactiveFuncBuilder.Build(expression);

            var tracker = new Tracker();

            var result = trackableFunc(tracker);

            Assert.That(tracker.Operands.Count, Is.EqualTo(3));
            Assert.That(tracker.Operands, Does.Contain(c1a.C1C1One2One));
            Assert.That(tracker.Operands, Does.Contain(c1b.C1C1One2One));
            Assert.That(tracker.Operands, Does.Contain(c1c.C1AllorsString));

            Assert.That(result, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task Signal()
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            var reactiveFuncBuilder = workspace.Services.Get<ITrackableFuncBuilder>();
            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();

            var dispatcher = dispatcherBuilder.Build(workspace);

            var valueSignal = dispatcher.CreateValueSignal("Hello");

            Expression<Func<ISignal<string>>> expression = () => valueSignal;

            var trackableFunc = reactiveFuncBuilder.Build(expression);

            var tracker = new Tracker();

            var result = trackableFunc(tracker);

            Assert.That(tracker.Operands.Count, Is.EqualTo(1));
            Assert.That(tracker.Operands, Does.Contain(valueSignal));

            Assert.That(result, Is.EqualTo(valueSignal));
        }


        private class Tracker : IDependency
        {
            public IList<IOperand> Operands { get; } = new List<IOperand>();

            public void Track(IOperand operand)
            {
                this.Operands.Add(operand);
            }
        }
    }
}
