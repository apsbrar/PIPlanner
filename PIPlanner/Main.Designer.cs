namespace PIPlanner
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.lbWorkItems = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this._selectedIterationsGrid = new System.Windows.Forms.DataGridView();
            this.btnRefreshBoard = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this._selectedIterationsGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Iterations";
            // 
            // elementHost1
            // 
            this.elementHost1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.elementHost1.Location = new System.Drawing.Point(0, 0);
            this.elementHost1.Margin = new System.Windows.Forms.Padding(0);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(949, 571);
            this.elementHost1.TabIndex = 13;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = null;
            // 
            // lbWorkItems
            // 
            this.lbWorkItems.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lbWorkItems.FormattingEnabled = true;
            this.lbWorkItems.Location = new System.Drawing.Point(6, 193);
            this.lbWorkItems.Name = "lbWorkItems";
            this.lbWorkItems.Size = new System.Drawing.Size(319, 355);
            this.lbWorkItems.TabIndex = 14;
            this.lbWorkItems.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lbWorkItems_MouseDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Backlog items";
            // 
            // _selectedIterationsGrid
            // 
            this._selectedIterationsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this._selectedIterationsGrid.ColumnHeadersVisible = false;
            this._selectedIterationsGrid.Location = new System.Drawing.Point(6, 21);
            this._selectedIterationsGrid.Name = "_selectedIterationsGrid";
            this._selectedIterationsGrid.ReadOnly = true;
            this._selectedIterationsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._selectedIterationsGrid.Size = new System.Drawing.Size(319, 152);
            this._selectedIterationsGrid.TabIndex = 16;
            // 
            // btnRefreshBoard
            // 
            this.btnRefreshBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnRefreshBoard.Location = new System.Drawing.Point(3, 545);
            this.btnRefreshBoard.Name = "btnRefreshBoard";
            this.btnRefreshBoard.Size = new System.Drawing.Size(322, 23);
            this.btnRefreshBoard.TabIndex = 17;
            this.btnRefreshBoard.Text = "Refresh Board";
            this.btnRefreshBoard.UseVisualStyleBackColor = true;
            this.btnRefreshBoard.Click += new System.EventHandler(this.btnRefreshBoard_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.btnRefreshBoard);
            this.splitContainer1.Panel1.Controls.Add(this._selectedIterationsGrid);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.lbWorkItems);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.elementHost1);
            this.splitContainer1.Size = new System.Drawing.Size(1280, 571);
            this.splitContainer1.SplitterDistance = 327;
            this.splitContainer1.TabIndex = 18;
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1280, 571);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PI Planner";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this._selectedIterationsGrid)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private System.Windows.Forms.ListBox lbWorkItems;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView _selectedIterationsGrid;
        private System.Windows.Forms.Button btnRefreshBoard;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}

