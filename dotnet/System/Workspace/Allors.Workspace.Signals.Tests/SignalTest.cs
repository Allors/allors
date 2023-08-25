namespace Allors.Workspace.Signals.Tests
{
    using Data;
    using Domain;

    public class SignalTest : Test
    {
        [Test]
        public async Task Role()
        {
            await this.Login("jane@example.com");

            var workspace = this.Workspace;
        }
    }
}
