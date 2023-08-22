namespace Allors.Workspace.Signals.Tests
{
    using Data;
    using Domain;

    public class SignalTest : Test
    {
        [Test]
        public async Task SetString()
        {
            await this.Login("jane@example.com");
            
            var workspace = this.Workspace;

            var c1 = workspace.Create<C1>();
            if (!c1.C1AllorsString.CanWrite)
            {
                await workspace.PullAsync(new Pull { Object = c1.Strategy });
            }

            var propertyChanges = new List<string>();

            c1.C1AllorsString.PropertyChanged += (sender, args) =>
            {
                propertyChanges.Add(args.PropertyName);
            };

            c1.C1AllorsString.Value = null;

            Assert.IsEmpty(propertyChanges);

            c1.C1AllorsString.Value = null;

            Assert.IsEmpty(propertyChanges);

            c1.C1AllorsString.Value = "Hello world!";

            Assert.AreEqual(3, propertyChanges.Count);
            Assert.Contains("Value", propertyChanges);
            Assert.Contains("Exist", propertyChanges);
            Assert.Contains("IsModified", propertyChanges);
        }
    }
}
