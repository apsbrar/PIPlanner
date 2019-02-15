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
            this.btnLoadSelection = new System.Windows.Forms.Button();
            this.chkDontLoadSuccesors = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this._grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource_main)).BeginInit();
            this.SuspendLayout();
            // 
            // Projects
            // 
            this.Projects.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Projects.FormattingEnabled = true;
            this.Projects.Location = new System.Drawing.Point(80, 15);
            this.Projects.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Projects.Name = "Projects";
            this.Projects.Size = new System.Drawing.Size(265, 24);
            this.Projects.TabIndex = 6;
            this.Projects.SelectedIndexChanged += new System.EventHandler(this.Projects_SelectedIndexChanged);
            // 
            // LabelProjects
            // 
            this.LabelProjects.AutoSize = true;
            this.LabelProjects.Location = new System.Drawing.Point(12, 18);
            this.LabelProjects.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LabelProjects.Name = "LabelProjects";
            this.LabelProjects.Size = new System.Drawing.Size(57, 16);
            this.LabelProjects.TabIndex = 5;
            this.LabelProjects.Text = "Projects";
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAll.Location = new System.Drawing.Point(599, 12);
            this.btnSelectAll.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(100, 28);
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
            this.btnOk.Location = new System.Drawing.Point(491, 407);
            this.btnOk.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(100, 28);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(599, 407);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 28);
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
            this._grid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this._grid.FilterAndSortEnabled = true;
            this._grid.Location = new System.Drawing.Point(16, 48);
            this._grid.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._grid.Name = "_grid";
            this._grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this._grid.ShowEditingIcon = false;
            this._grid.Size = new System.Drawing.Size(683, 324);
            this._grid.TabIndex = 11;
            this._grid.FilterStringChanged += new System.EventHandler<Zuby.ADGV.AdvancedDataGridView.FilterEventArgs>(this._grid_FilterStringChanged);
            // 
            // btnLoadPreviousFilter
            // 
            this.btnLoadPreviousFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadPreviousFilter.Location = new System.Drawing.Point(16, 407);
            this.btnLoadPreviousFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadPreviousFilter.Name = "btnLoadPreviousFilter";
            this.btnLoadPreviousFilter.Size = new System.Drawing.Size(181, 28);
            this.btnLoadPreviousFilter.TabIndex = 12;
            this.btnLoadPreviousFilter.Text = "Load Previous Filter";
            this.btnLoadPreviousFilter.UseVisualStyleBackColor = true;
            this.btnLoadPreviousFilter.Click += new System.EventHandler(this.btnLoadPreviousFilter_Click);
            // 
            // btnLoadSelection
            // 
            this.btnLoadSelection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLoadSelection.Location = new System.Drawing.Point(205, 407);
            this.btnLoadSelection.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnLoadSelection.Name = "btnLoadSelection";
            this.btnLoadSelection.Size = new System.Drawing.Size(199, 28);
            this.btnLoadSelection.TabIndex = 13;
            this.btnLoadSelection.Text = "Load Previous Selection";
            this.btnLoadSelection.UseVisualStyleBackColor = true;
            this.btnLoadSelection.Click += new System.EventHandler(this.btnLoadSelection_Click);
            // 
            // chkDontLoadSuccesors
            // 
            this.chkDontLoadSuccesors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.chkDontLoadSuccesors.AutoSize = true;
            this.chkDontLoadSuccesors.Location = new System.Drawing.Point(464, 380);
            this.chkDontLoadSuccesors.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkDontLoadSuccesors.Name = "chkDontLoadSuccesors";
            this.chkDontLoadSuccesors.Size = new System.Drawing.Size(168, 20);
            this.chkDontLoadSuccesors.TabIndex = 13;
            this.chkDontLoadSuccesors.Text = "Don\'t Show Successors";
            this.chkDontLoadSuccesors.UseVisualStyleBackColor = true;
            // 
            // IterationPicker
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(715, 450);
            this.Controls.Add(this.btnLoadSelection);
            this.Controls.Add(this.chkDontLoadSuccesors);
            this.Controls.Add(this.btnLoadPreviousFilter);
            this.Controls.Add(this._grid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnSelectAll);
            this.Controls.Add(this.Projects);
            this.Controls.Add(this.LabelProjects);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IterationPicker";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PI Planner {0}";
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
        private System.Windows.Forms.Button btnLoadSelection;
        private System.Windows.Forms.CheckBox chkDontLoadSuccesors;
    }
}