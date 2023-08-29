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
        public async Task ReactiveFuncBuilder(Implementations implementation)
        {
            await this.Login("jane@example.com");
            var workspace = this.Workspace;
            SelectImplementation(workspace, implementation);

            var reactiveFuncBuilder = workspace.Services.Get<IReactiveFuncBuilder>();
            var dispatcherBuilder = workspace.Services.Get<IDispatcherBuilder>();
            var dispatcher = dispatcherBuilder.Build(workspace);

            var c1a = workspace.Create<C1>();
            var c1b = workspace.Create<C1>();
            var c1c = workspace.Create<C1>();
            var c1d = workspace.Create<C1>();

            if (!c1a.C1C1One2One.CanWrite || !c1b.C1C1One2One.CanWrite || !c1c.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1a.Strategy }, new Pull { Object = c1b.Strategy }, new Pull { Object = c1c.Strategy });
            }

            c1a.C1C1One2One.Value = c1b;
            c1b.C1C1One2One.Value = c1c;
            c1c.C1AllorsString.Value = "Hello";

            Expression<Func<string>> expression = () => c1a.C1C1One2One.Value.C1C1One2One.Value.C1AllorsString.Value;

            var reactiveFunc = reactiveFuncBuilder.Build(expression);

            var calculatedSignal = dispatcher.CreateCalculatedSignal(reactiveFunc);

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello"));

            c1c.C1AllorsString.Value = "Hello Again";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1d.C1AllorsString.Value = "Another Hello";

            Assert.That(calculatedSignal.Value, Is.EqualTo("Hello Again"));

            c1b.C1C1One2One.Value = c1d;

            Assert.That(calculatedSignal.Value, Is.EqualTo("Another Hello"));
        }
    }
}
