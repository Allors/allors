namespace Workspace.WinForms.App.Forms
{
    using ViewModels.Features;

    partial class PersonGeneratorForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonGeneratorForm));
            this.personFormControllerBindingSource = new BindingSource(this.components);
            this.toolStrip1 = new ToolStrip();
            this.loadToolStripButton = new ToolStripButton();
            this.saveToolStripButton = new ToolStripButton();
            this.showDialogToolStripButton = new ToolStripButton();
            this.splitContainer1 = new SplitContainer();
            this.dataGridView1 = new DataGridView();
            this.FirstName = new DataGridViewTextBoxColumn();
            this.firstNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            this.peopleBindingSource = new BindingSource(this.components);
            this.textBox4 = new TextBox();
            this.label4 = new Label();
            this.textBox3 = new TextBox();
            this.label3 = new Label();
            this.textBox2 = new TextBox();
            this.label2 = new Label();
            this.textBox1 = new TextBox();
            this.label1 = new Label();
            this.changeFirstNameButton = new Button();
            ((System.ComponentModel.ISupportInitialize)this.personFormControllerBindingSource).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)this.peopleBindingSource).BeginInit();
            this.SuspendLayout();
            // 
            // personFormControllerBindingSource
            // 
            this.personFormControllerBindingSource.DataSource = typeof(PersonGeneratorFormViewModel);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new Size(20, 20);
            this.toolStrip1.Items.AddRange(new ToolStripItem[] { this.loadToolStripButton, this.saveToolStripButton, this.showDialogToolStripButton });
            this.toolStrip1.Location = new Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new Size(1041, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // loadToolStripButton
            // 
            this.loadToolStripButton.DataBindings.Add(new Binding("Command", this.personFormControllerBindingSource, "LoadCommand", true));
            this.loadToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.loadToolStripButton.Image = (Image)resources.GetObject("loadToolStripButton.Image");
            this.loadToolStripButton.ImageTransparentColor = Color.Magenta;
            this.loadToolStripButton.Name = "loadToolStripButton";
            this.loadToolStripButton.Size = new Size(46, 24);
            this.loadToolStripButton.Text = "Load";
            // 
            // saveToolStripButton
            // 
            this.saveToolStripButton.DataBindings.Add(new Binding("Command", this.personFormControllerBindingSource, "SaveCommand", true));
            this.saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.saveToolStripButton.Image = (Image)resources.GetObject("saveToolStripButton.Image");
            this.saveToolStripButton.ImageTransparentColor = Color.Magenta;
            this.saveToolStripButton.Name = "saveToolStripButton";
            this.saveToolStripButton.Size = new Size(44, 24);
            this.saveToolStripButton.Text = "Save";
            // 
            // showDialogToolStripButton
            // 
            this.showDialogToolStripButton.DataBindings.Add(new Binding("Command", this.personFormControllerBindingSource, "ShowDialogCommand", true));
            this.showDialogToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            this.showDialogToolStripButton.Image = (Image)resources.GetObject("showDialogToolStripButton.Image");
            this.showDialogToolStripButton.ImageTransparentColor = Color.Magenta;
            this.showDialogToolStripButton.Name = "showDialogToolStripButton";
            this.showDialogToolStripButton.Size = new Size(98, 24);
            this.showDialogToolStripButton.Text = "Show Dialog";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.changeFirstNameButton);
            this.splitContainer1.Panel2.Controls.Add(this.textBox4);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.textBox3);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.textBox2);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Size = new Size(1041, 474);
            this.splitContainer1.SplitterDistance = 347;
            this.splitContainer1.TabIndex = 2;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[] { this.FirstName, this.firstNameDataGridViewTextBoxColumn });
            this.dataGridView1.DataSource = this.peopleBindingSource;
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 29;
            this.dataGridView1.Size = new Size(347, 474);
            this.dataGridView1.TabIndex = 0;
            // 
            // FirstName
            // 
            this.FirstName.DataPropertyName = "FirstName";
            this.FirstName.HeaderText = "FirstName";
            this.FirstName.MinimumWidth = 6;
            this.FirstName.Name = "FirstName";
            this.FirstName.Width = 125;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.Width = 125;
            // 
            // peopleBindingSource
            // 
            this.peopleBindingSource.DataMember = "People";
            this.peopleBindingSource.DataSource = this.personFormControllerBindingSource;
            this.peopleBindingSource.CurrentChanged += this.peopleBindingSource_CurrentChanged;
            // 
            // textBox4
            // 
            this.textBox4.DataBindings.Add(new Binding("Text", this.peopleBindingSource, "Greeting", true));
            this.textBox4.Location = new Point(347, 257);
            this.textBox4.Name = "textBox4";
            this.textBox4.Size = new Size(251, 27);
            this.textBox4.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new Point(123, 260);
            this.label4.Name = "label4";
            this.label4.Size = new Size(66, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Greeting";
            // 
            // textBox3
            // 
            this.textBox3.DataBindings.Add(new Binding("Text", this.peopleBindingSource, "FullName", true));
            this.textBox3.Location = new Point(347, 202);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new Size(251, 27);
            this.textBox3.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new Point(123, 205);
            this.label3.Name = "label3";
            this.label3.Size = new Size(76, 20);
            this.label3.TabIndex = 4;
            this.label3.Text = "Full Name";
            // 
            // textBox2
            // 
            this.textBox2.DataBindings.Add(new Binding("Text", this.peopleBindingSource, "FirstName", true));
            this.textBox2.Location = new Point(347, 143);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Size(251, 27);
            this.textBox2.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new Point(123, 146);
            this.label2.Name = "label2";
            this.label2.Size = new Size(169, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "First Name (ViewModel)";
            // 
            // textBox1
            // 
            this.textBox1.DataBindings.Add(new Binding("Text", this.personFormControllerBindingSource, "Selected.FirstName", true));
            this.textBox1.Location = new Point(347, 88);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Size(251, 27);
            this.textBox1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new Point(123, 91);
            this.label1.Name = "label1";
            this.label1.Size = new Size(126, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "First Name (View)";
            // 
            // changeFirstNameButton
            // 
            this.changeFirstNameButton.Location = new Point(347, 357);
            this.changeFirstNameButton.Name = "changeFirstNameButton";
            this.changeFirstNameButton.Size = new Size(251, 29);
            this.changeFirstNameButton.TabIndex = 8;
            this.changeFirstNameButton.Text = "Change First Name";
            this.changeFirstNameButton.UseVisualStyleBackColor = true;
            this.changeFirstNameButton.Click += this.changeFirstNameButton_Click;
            // 
            // PersonGeneratorForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1041, 501);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "PersonGeneratorForm";
            this.Text = "PersonGeneratorForm";
            this.DataContextChanged += this.PersonForm_DataContextChanged;
            ((System.ComponentModel.ISupportInitialize)this.personFormControllerBindingSource).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)this.dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)this.peopleBindingSource).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion
        private BindingSource personFormControllerBindingSource;
        private ToolStrip toolStrip1;
        private ToolStripButton showDialogToolStripButton;
        private SplitContainer splitContainer1;
        private DataGridView dataGridView1;
        private ToolStripButton loadToolStripButton;
        private ToolStripButton saveToolStripButton;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private BindingSource peopleBindingSource;
        private TextBox textBox1;
        private Label label1;
        private TextBox textBox2;
        private Label label2;
        private DataGridViewTextBoxColumn FirstName;
        private DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private TextBox textBox3;
        private Label label3;
        private TextBox textBox4;
        private Label label4;
        private Button changeFirstNameButton;
    }
}
