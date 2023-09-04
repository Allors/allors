namespace Workspace.WinForms.App.Services
{
    using Forms;
    using Microsoft.Extensions.DependencyInjection;
    using ViewModels.Features;
    using ViewModels.Services;

    public class MdiService : IMdiService
    {
        public MdiService(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }

        public void Open(Type controllerType)
        {
            var parent = this.ServiceProvider.GetRequiredService<MainForm>();

            if (controllerType == typeof(PersonManualFormViewModel))
            {
                var form = this.ServiceProvider.GetRequiredService<PersonManualForm>();
                form.MdiParent = parent;
                form.Show();
            }

            if (controllerType == typeof(PersonGeneratorFormViewModel))
            {
                var form = this.ServiceProvider.GetRequiredService<PersonGeneratorForm>();
                form.MdiParent = parent;
                form.Show();
            }
        }
    }
}
