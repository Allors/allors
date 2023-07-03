namespace Workspace.ViewModels.WinForms.Forms
{
    partial class PersonForm
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PersonForm));
            personFormControllerBindingSource = new BindingSource(this.components);
            toolStrip1 = new ToolStrip();
            loadToolStripButton = new ToolStripButton();
            saveToolStripButton = new ToolStripButton();
            showDialogToolStripButton = new ToolStripButton();
            splitContainer1 = new SplitContainer();
            dataGridView1 = new DataGridView();
            FirstName = new DataGridViewTextBoxColumn();
            firstNameDataGridViewTextBoxColumn = new DataGridViewTextBoxColumn();
            peopleBindingSource = new BindingSource(this.components);
            textBox2 = new TextBox();
            label2 = new Label();
            textBox1 = new TextBox();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)personFormControllerBindingSource).BeginInit();
            toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)peopleBindingSource).BeginInit();
            this.SuspendLayout();
            // 
            // personFormControllerBindingSource
            // 
            personFormControllerBindingSource.DataSource = typeof(Features.PersonFormViewModel);
            // 
            // toolStrip1
            // 
            toolStrip1.ImageScalingSize = new Size(20, 20);
            toolStrip1.Items.AddRange(new ToolStripItem[] { loadToolStripButton, saveToolStripButton, showDialogToolStripButton });
            toolStrip1.Location = new Point(0, 0);
            toolStrip1.Name = "toolStrip1";
            toolStrip1.Size = new Size(1041, 27);
            toolStrip1.TabIndex = 1;
            toolStrip1.Text = "toolStrip1";
            // 
            // loadToolStripButton
            // 
            loadToolStripButton.DataBindings.Add(new Binding("Command", personFormControllerBindingSource, "LoadCommand", true));
            loadToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            loadToolStripButton.Image = (Image)resources.GetObject("loadToolStripButton.Image");
            loadToolStripButton.ImageTransparentColor = Color.Magenta;
            loadToolStripButton.Name = "loadToolStripButton";
            loadToolStripButton.Size = new Size(46, 24);
            loadToolStripButton.Text = "Load";
            // 
            // saveToolStripButton
            // 
            saveToolStripButton.DataBindings.Add(new Binding("Command", personFormControllerBindingSource, "SaveCommand", true));
            saveToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            saveToolStripButton.Image = (Image)resources.GetObject("saveToolStripButton.Image");
            saveToolStripButton.ImageTransparentColor = Color.Magenta;
            saveToolStripButton.Name = "saveToolStripButton";
            saveToolStripButton.Size = new Size(44, 24);
            saveToolStripButton.Text = "Save";
            // 
            // showDialogToolStripButton
            // 
            showDialogToolStripButton.DataBindings.Add(new Binding("Command", personFormControllerBindingSource, "ShowDialogCommand", true));
            showDialogToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            showDialogToolStripButton.Image = (Image)resources.GetObject("showDialogToolStripButton.Image");
            showDialogToolStripButton.ImageTransparentColor = Color.Magenta;
            showDialogToolStripButton.Name = "showDialogToolStripButton";
            showDialogToolStripButton.Size = new Size(98, 24);
            showDialogToolStripButton.Text = "Show Dialog";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 27);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(textBox2);
            splitContainer1.Panel2.Controls.Add(label2);
            splitContainer1.Panel2.Controls.Add(textBox1);
            splitContainer1.Panel2.Controls.Add(label1);
            splitContainer1.Size = new Size(1041, 474);
            splitContainer1.SplitterDistance = 347;
            splitContainer1.TabIndex = 2;
            // 
            // dataGridView1
            // 
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { FirstName, firstNameDataGridViewTextBoxColumn });
            dataGridView1.DataSource = peopleBindingSource;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.RowTemplate.Height = 29;
            dataGridView1.Size = new Size(347, 474);
            dataGridView1.TabIndex = 0;
            // 
            // FirstName
            // 
            FirstName.DataPropertyName = "FirstName";
            FirstName.HeaderText = "FirstName";
            FirstName.MinimumWidth = 6;
            FirstName.Name = "FirstName";
            FirstName.Width = 125;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            firstNameDataGridViewTextBoxColumn.HeaderText = "FirstName";
            firstNameDataGridViewTextBoxColumn.MinimumWidth = 6;
            firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            firstNameDataGridViewTextBoxColumn.Width = 125;
            // 
            // peopleBindingSource
            // 
            peopleBindingSource.DataMember = "People";
            peopleBindingSource.DataSource = personFormControllerBindingSource;
            peopleBindingSource.CurrentChanged += this.peopleBindingSource_CurrentChanged;
            // 
            // textBox2
            // 
            textBox2.DataBindings.Add(new Binding("Text", peopleBindingSource, "FirstName", true));
            textBox2.Location = new Point(347, 143);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(125, 27);
            textBox2.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(123, 146);
            label2.Name = "label2";
            label2.Size = new Size(169, 20);
            label2.TabIndex = 2;
            label2.Text = "First Name (ViewModel)";
            // 
            // textBox1
            // 
            textBox1.DataBindings.Add(new Binding("Text", personFormControllerBindingSource, "Selected.FirstName", true));
            textBox1.Location = new Point(347, 88);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(125, 27);
            textBox1.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(123, 91);
            label1.Name = "label1";
            label1.Size = new Size(126, 20);
            label1.TabIndex = 0;
            label1.Text = "First Name (View)";
            // 
            // PersonForm
            // 
            this.AutoScaleDimensions = new SizeF(8F, 20F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1041, 501);
            this.Controls.Add(splitContainer1);
            this.Controls.Add(toolStrip1);
            this.Name = "PersonForm";
            this.Text = "PersonForm";
            this.DataContextChanged += this.PersonForm_DataContextChanged;
            ((System.ComponentModel.ISupportInitialize)personFormControllerBindingSource).EndInit();
            toolStrip1.ResumeLayout(false);
            toolStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ((System.ComponentModel.ISupportInitialize)peopleBindingSource).EndInit();
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
    }
}
