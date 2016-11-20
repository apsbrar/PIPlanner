using Microsoft.TeamFoundation.WorkItemTracking.Client;
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
        System.Windows.Controls.ListView lvWorkItems = new System.Windows.Controls.ListView();

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

            elementHost2.Child = lvWorkItems;
            lvWorkItems.Name = "lvWorkItems";
            lvWorkItems.PreviewMouseLeftButtonDown += TableHelper.lv_PreviewMouseLeftButtonDown;
            lvWorkItems.PreviewMouseMove += TableHelper.lv_PreviewMouseMove;
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
                System.Windows.Forms.MessageBox.Show("Could not connect : " + ex.Message + System.Environment.NewLine + ex.StackTrace);
                System.Windows.Forms.Application.Exit();
            }

        }

        private void SetIterationsGrid(List<IterationSelection> selections)
        {
            _selectedIterationsGrid.DataSource = selections;

            if (_selectedIterationsGrid.Columns.Count > 0)
            {
                _selectedIterationsGrid.Columns["IsSelected"].Visible = false;
                _selectedIterationsGrid.Columns["Iteration"].Visible = false;
                _selectedIterationsGrid.Columns["Path"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        void _selectedIterationsGrid_SelectionChanged(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            lvWorkItems.Items.Clear();

            foreach (DataGridViewRow row in _selectedIterationsGrid.SelectedRows)
            {
                var val = row.Cells["Path"].Value;
                foreach (var workItem in tfs.GetWorkItemsInIterationPath((string)val))
                {
                    TableHelper.AddWorkItemToSprint(lvWorkItems, workItem);
                }

            }
            this.Cursor = Cursors.Default;
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            var selections = _selectedIterationsGrid.DataSource as List<IterationSelection>;

            SetIterationsGrid(selections);

            TableHelper.SetTable(selections, _table, tfs);

            _selectedIterationsGrid_SelectionChanged(null, null);
            this.Cursor = Cursors.Default;
        }
    }
}