namespace Workspace.WinForms.App.Forms
{
    using ViewModels.Features;

    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.menuStrip1 = new MenuStrip();
            this.peopleManualToolStripMenuItem = new ToolStripMenuItem();
            this.mainFormControllerBindingSource = new BindingSource(this.components);
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)this.mainFormControllerBindingSource).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new Size(20, 20);
            this.menuStrip1.Items.AddRange(new ToolStripItem[] { this.peopleManualToolStripMenuItem });
            this.menuStrip1.Location = new Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new Padding(8, 2, 0, 2);
            this.menuStrip1.Size = new Size(1904, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // peopleManualToolStripMenuItem
            // 
            this.peopleManualToolStripMenuItem.DataBindings.Add(new Binding("Command", this.mainFormControllerBindingSource, "ShowPersonManualCommand", true));
            this.peopleManualToolStripMenuItem.Name = "peopleManualToolStripMenuItem";
            this.peopleManualToolStripMenuItem.Size = new Size(154, 29);
            this.peopleManualToolStripMenuItem.Text = "People (Manual)";
            // 
            // mainFormControllerBindingSource
            // 
            this.mainFormControllerBindingSource.DataSource = typeof(MainFormViewModel);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(10F, 25F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(1904, 1101);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new Padding(4);
            this.Name = "MainForm";
            this.Text = "Allors";
            this.DataContextChanged += this.MainForm_DataContextChanged;
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)this.mainFormControllerBindingSource).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private MenuStrip menuStrip1;
        private ToolStripMenuItem peopleManualToolStripMenuItem;
        private BindingSource mainFormControllerBindingSource;
    }
}
