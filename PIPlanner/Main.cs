using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;

namespace PIPlanner
{
    public partial class Main : Form
    {
        private Tfs tfs;
        ScaleTransform _st = new ScaleTransform(1, 1);
        Grid _table = new Grid();
        System.Windows.Controls.ListView lvWorkItems = new System.Windows.Controls.ListView();

        public Main()
        {
            InitializeComponent();
            var dockpanel = new DockPanel() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, LastChildFill = true};
            var scroll = new ScrollViewer() {HorizontalScrollBarVisibility = ScrollBarVisibility.Auto, VerticalScrollBarVisibility = ScrollBarVisibility.Auto};
            var dockpanel1 = new DockPanel() { HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch, LastChildFill = true};
            var slider = CreateZoomSlider();
            var tb = new TextBlock();

            tb.Margin = new Thickness(8, 2, 12, 2);
            tb.Inlines.Add(new System.Windows.Documents.Run
            {
                Background = System.Windows.Media.Brushes.Blue,
                Foreground = System.Windows.Media.Brushes.White,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans"),
                Text = "Predecessor" + System.Environment.NewLine
            });
            tb.Inlines.Add(new System.Windows.Documents.Run
            {
                Background = System.Windows.Media.Brushes.MidnightBlue,
                Foreground = System.Windows.Media.Brushes.Pink,
                FontFamily = new System.Windows.Media.FontFamily("Comic Sans"),
                Text = "Successor"
            });

            dockpanel1.Children.Add(tb);
            dockpanel1.Children.Add(slider);
  
            dockpanel.Children.Add(dockpanel1);
            dockpanel.Children.Add(scroll);
            DockPanel.SetDock(scroll, System.Windows.Controls.Dock.Top);
            DockPanel.SetDock(dockpanel1, System.Windows.Controls.Dock.Bottom);

            scroll.Content = _table;
            elementHost1.Child = dockpanel;

            _table.LayoutTransform = _st;
            _table.Background = System.Windows.Media.Brushes.White;
            _table.ShowGridLines = true;

            _selectedIterationsGrid.SelectionChanged += _selectedIterationsGrid_SelectionChanged;

            elementHost2.Child = lvWorkItems;
            lvWorkItems.Name = "lvWorkItems";
            lvWorkItems.PreviewMouseLeftButtonDown += TableHelper.lv_PreviewMouseLeftButtonDown;
            lvWorkItems.PreviewMouseMove += TableHelper.lv_PreviewMouseMove;
        }

        private Slider CreateZoomSlider()
        {
            var slider = new Slider()
            {
                Minimum = 1,
                Maximum = 4,
                HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                IsMoveToPointEnabled = true,
                AutoToolTipPrecision = 2,
                AutoToolTipPlacement = AutoToolTipPlacement.BottomRight,
                TickPlacement = TickPlacement.BottomRight,
                IsSnapToTickEnabled = true,
                TickFrequency = .2
            };

            // Manually add ticks to the slider.
            DoubleCollection tickMarks = new DoubleCollection();
            tickMarks.Add(1.1);
            tickMarks.Add(1.2);
            tickMarks.Add(1.3);
            tickMarks.Add(1.4);
            tickMarks.Add(1.5);
            tickMarks.Add(1.6);
            tickMarks.Add(1.7);
            tickMarks.Add(1.8);
            tickMarks.Add(1.9);
            tickMarks.Add(2);
            tickMarks.Add(2.1);
            tickMarks.Add(2.2);
            tickMarks.Add(2.3);
            tickMarks.Add(2.4);
            tickMarks.Add(2.5);
            tickMarks.Add(2.6);
            tickMarks.Add(2.7);
            tickMarks.Add(2.8);
            tickMarks.Add(2.9);
            tickMarks.Add(3);
            tickMarks.Add(4);
            slider.Ticks = tickMarks;

            slider.ValueChanged += slider_ValueChanged;
            return slider;
        }

        void slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            _st.ScaleX = e.NewValue;
            _st.ScaleY = e.NewValue;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            var progress = new Progress();
            progress.FormClosed += progress_FormClosed;

            try
            {
                if( ConfigurationManager.AppSettings["TFSUri"] == null)
                {
                    System.Windows.Forms.MessageBox.Show("TFS Uri not set in config file");
                    System.Windows.Forms.Application.Exit();
                }

                int autoRefreshInterval = 300000;
                if (ConfigurationManager.AppSettings["AutoRefreshInterval"] != null)
                {
                    int.TryParse(ConfigurationManager.AppSettings["AutoRefreshInterval"], out autoRefreshInterval);
                }

                timer1.Interval = autoRefreshInterval;

                string tfsUriStr = ConfigurationManager.AppSettings["TFSUri"].ToString();
                if (!Uri.IsWellFormedUriString(tfsUriStr, UriKind.Absolute))
                {
                    System.Windows.Forms.MessageBox.Show("TFS Uri in config file is not valid");
                    System.Windows.Forms.Application.Exit();
                }

                tfs = new Tfs(new Uri(tfsUriStr));
                TableHelper._tfs = tfs;
                TableHelper._table = _table;
                var picker = new IterationPicker(tfs);
                if (picker.ShowDialog(this) == System.Windows.Forms.DialogResult.OK && picker.SelectedIterations != null)
                {
                    progress.Show(this);

                    System.Windows.Forms.Application.DoEvents();

                    System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
                    sw.Start();
                    this.Cursor = Cursors.WaitCursor;
                    List<IterationSelection> selections = picker.SelectedIterations;
                    TableHelper.LoadSuccesors = picker.LoadSuccesors;

                    SetIterationsGrid(selections);

                    TableHelper.SetTable(selections, _table, tfs);
                    this.Cursor = Cursors.Default;
                    sw.Stop();
                    var secs = sw.Elapsed.TotalSeconds;
                    System.Diagnostics.Debug.WriteLine(secs);
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
            finally
            {
                progress.FormClosed -= progress_FormClosed;
                progress.Close();
            }

        }

        void progress_FormClosed(object sender, FormClosedEventArgs e)
        {
            System.Windows.Forms.Application.Exit();
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
            //lvWorkItems.Items.Clear();

            foreach (DataGridViewRow row in _selectedIterationsGrid.SelectedRows)
            {
                var val = row.Cells["Path"].Value;
                List<System.Windows.Controls.ListViewItem> wis = new List<System.Windows.Controls.ListViewItem>();

                foreach (var workItem in tfs.GetWorkItemsInIterationPath((string)val))
                {
                    var lvi = TableHelper.GetLviForWi(workItem);
                    wis.Add(lvi);
                }
                lvWorkItems.ItemsSource = wis;

            }
            this.Cursor = Cursors.Default;
        }


        private void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                splitContainer1.Enabled = false;
                var selections = _selectedIterationsGrid.DataSource as List<IterationSelection>;

                SetIterationsGrid(selections);

                TableHelper.SetTable(selections, _table, tfs);

                _selectedIterationsGrid_SelectionChanged(null, null);
                
            }
            finally
            {
                splitContainer1.Enabled = true;
                this.Cursor = Cursors.Default;
            }
        }

        private void chkAutoRefresh_CheckedChanged(object sender, EventArgs e)
        {
            timer1.Enabled = chkAutoRefresh.Checked;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            btnRefresh_Click(null, null);
        }


    }
}