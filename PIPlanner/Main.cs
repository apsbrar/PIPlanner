using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace PIPlanner
{
    public partial class Main : Form
    {
        private Tfs tfs;
        Grid _table = new Grid();

        public Main()
        {
            InitializeComponent();

            var scroll = new ScrollViewer();
            scroll.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scroll.Content = _table;
            elementHost1.Child = scroll;
            _table.Background = System.Windows.Media.Brushes.GhostWhite;
            _table.ShowGridLines = true;

            _selectedIterationsGrid.SelectionChanged += _selectedIterationsGrid_SelectionChanged;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                if( ConfigurationManager.AppSettings["TFSUri"] == null)
                {
                    System.Windows.Forms.MessageBox.Show("TFS Uri not set in config file");
                    System.Windows.Forms.Application.Exit();
                }

                string tfsUriStr = ConfigurationManager.AppSettings["TFSUri"].ToString();
                if (!Uri.IsWellFormedUriString(tfsUriStr, UriKind.Absolute))
                {
                    System.Windows.Forms.MessageBox.Show("TFS Uri in config file is not valid");
                    System.Windows.Forms.Application.Exit();
                }

                tfs = new Tfs(new Uri(tfsUriStr));
                var picker = new IterationPicker(tfs);
                if (picker.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && picker.SelectedIterations != null)
                {
                    this.Cursor = Cursors.WaitCursor;
                    List<IterationSelection> selections = picker.SelectedIterations;

                    SetIterationsGrid(selections);

                    TableHelper.SetTable(selections, _table, tfs);
                    this.Cursor = Cursors.Default;
                }
                else
                {
                    System.Windows.Forms.Application.Exit();
                }

                picker.Dispose();

            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Could not connect : " + ex.Message);
                System.Windows.Forms.Application.Exit();
            }

        }

        private void SetIterationsGrid(List<IterationSelection> selections)
        {
            _selectedIterationsGrid.DataSource = selections;

            if (_selectedIterationsGrid.Columns.Count > 0)
            {
                _selectedIterationsGrid.Columns[0].Visible = false;
                _selectedIterationsGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        void _selectedIterationsGrid_SelectionChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            lbWorkItems.Items.Clear();
            foreach (DataGridViewRow row in _selectedIterationsGrid.SelectedRows)
            {
                foreach (var workItem in tfs.GetWorkItemsInIterationPath((string)row.Cells[1].Value))
                {
                    lbWorkItems.Items.Add(workItem);
                }
            }
            this.Cursor = Cursors.Default;
        }

        private void lbWorkItems_MouseDown(object sender, MouseEventArgs e)
        {
            if (lbWorkItems.Items.Count == 0)
                return;

            int index = lbWorkItems.IndexFromPoint(e.X, e.Y);
            string s = lbWorkItems.Items[index].ToString();
            System.Windows.Forms.DragDropEffects dde1 = DoDragDrop(s, System.Windows.Forms.DragDropEffects.All);
        }

        private void btnRefreshBoard_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var selections = _selectedIterationsGrid.DataSource as List<IterationSelection>;

            SetIterationsGrid(selections);

            TableHelper.SetTable(selections, _table, tfs);
            this.Cursor = Cursors.Default;
        }
    }
}