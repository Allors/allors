namespace Workspace.WinForms.App.Forms
{
    using System.Windows.Forms;
    using ViewModels.Features;

    public partial class PersonGeneratorForm : Form
    {
        public PersonGeneratorForm(PersonGeneratorFormViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
            this.DataContext = this.ViewModel;
        }

        public PersonGeneratorFormViewModel ViewModel { get; set; }

        private void PersonForm_DataContextChanged(object sender, EventArgs e)
            => this.personFormControllerBindingSource.DataSource = this.DataContext;

        private void peopleBindingSource_CurrentChanged(object sender, EventArgs e)
        {
            var current = (PersonGeneratorViewModel)this.peopleBindingSource.Current;

            this.ViewModel.Selected = current;

            if (this.ViewModel.Selected != current)
            {
                var index = this.peopleBindingSource.List.IndexOf(this.ViewModel.Selected);

                if (index >= 0)
                {
                    this.peopleBindingSource.Position = index;
                    this.peopleBindingSource.CurrencyManager.Refresh();
                }
            }
        }

        private void changeFirstNameButton_Click(object sender, EventArgs e)
        {
            if (this.ViewModel.Selected != null)
            {
                this.ViewModel.Selected.FirstName += "!";
            }
        }
    }
}
