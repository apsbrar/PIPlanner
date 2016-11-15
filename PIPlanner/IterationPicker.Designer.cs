namespace PIPlanner
{
    partial class IterationPicker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IterationPicker));
            this.Projects = new System.Windows.Forms.ComboBox();
            this.LabelProjects = new System.Windows.Forms.Label();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this._grid = new Zuby.ADGV.AdvancedDataGridView();
            this.bindingSource_main = new System.Windows.Forms.BindingSource(this.components);
            this.btnLoadPreviousFilter = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).BeginInit();
            this.SuspendLayout();
            // 
            // Projects
            // 
            this.Projects.FormattingEnabled = true;
            this.Projects.Location = new System.Drawing.Point(60, 12);
            this.Projects.Name = "Projects";
            this.Projects.Size = new System.Drawing.Size(210, 21);
            this.Projects.TabIndex = 6;
            this.Projects.SelectedIndexChanged += new System.EventHandler(this.Projects_SelectedIndexChanged);
            // 
            // LabelProjects
            // 
            this.LabelProjects.AutoSize = true;
            this.LabelProjects.Location = new System.Drawing.Point(9, 15);
            this.LabelProjects.Name = "LabelProjects";
            this.LabelProjects.Size = new System.Drawing.Size(45, 13);
            this.LabelProjects.TabIndex = 5;
            this.LabelProjects.Text = "Projects";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(449, 10);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(75, 23);
            this.btnSelectAll.TabIndex = 8;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Visible = false;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(368, 305);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(449, 305);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // _grid
            // 
            this._grid.AllowUserToAddRows = false;
            this._grid.AllowUserToDeleteRows = false;
            this._grid.AllowUserToOrderColumns = true;
            this._grid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this._grid.FilterAndSortEnabled = true;
            this._grid.Location = new System.Drawing.Point(12, 39);
            this._grid.Name = "_grid";
            this._grid.ReadOnly = true;
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowEditingIcon = false;
            this._grid.Size = new System.Drawing.Size(512, 260);
            this._grid.TabIndex = 11;
            this._grid.FilterStringChanged += new System.EventHandler(this._grid_FilterStringChanged);
            // 
            // btnLoadPreviousFilter
            // 
            this.btnLoadPreviousFilter.Location = new System.Drawing.Point(12, 305);
            this.btnLoadPreviousFilter.Name = "btnLoadPreviousFilter";
            this.btnLoadPreviousFilter.Size = new System.Drawing.Size(136, 23);
            this.btnLoadPreviousFilter.TabIndex = 12;
            this.btnLoadPreviousFilter.Text = "Load Previous Filter";
            this.btnLoadPreviousFilter.UseVisualStyleBackColor = true;
            this.btnLoadPreviousFilter.Click += new System.EventHandler(this.btnLoadPreviousFilter_Click);
            // 
            // IterationPicker
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(536, 340);
            this.Controls.Add(this.btnLoadPreviousFilter);
            this.Controls.Add(this._grid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.Projects);
            this.Controls.Add(this.LabelProjects);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IterationPicker";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Iteration Picker";
            this.Load += new System.EventHandler(this.IterationPicker_Load);
            ((System.ComponentModel.ISupportInitialize)(this._grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Projects;
        private System.Windows.Forms.Label LabelProjects;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private Zuby.ADGV.AdvancedDataGridView _grid;
        private System.Windows.Forms.BindingSource bindingSource_main;
        private System.Windows.Forms.Button btnLoadPreviousFilter;
    }
}