namespace Workspace.ViewModels.Tests
{
    using Allors.Workspace;
    using Allors.Workspace.Adapters;
    using Allors.Workspace.Domain;
    using Features;
    using Task = Task;

    public class PersonTest : Test
    {
        private Connection connection;

        [SetUp]
        public void Setup()
        {
            this.connection = this.Connect("administrator");
        }

        [Test]
        public async Task FirstName()
        {
            var workspace = this.connection.CreateWorkspace();

            var person = workspace.Create<Person>();

            var personViewModel = new PersonViewModel(person);

            Assert.That(personViewModel.FirstName, Is.Null);

            person.FirstName.Value = "John";

            Assert.That(personViewModel.FirstName, Is.EqualTo("John"));

            personViewModel.FirstName = "Jane";

            Assert.That(person.FirstName.Value, Is.EqualTo("Jane"));
        }
    }
}
