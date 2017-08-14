using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIPlanner
{
    public partial class IterationPicker : Form
    {
        private Tfs _tfs;
        private DataTable _dataTable = null;
        private DataSet _dataSet = null;
        int _edsIndex = 0;

        public bool LoadSuccesors
        {
            get
            {
                return chkLoadSuccesors.Checked;
            }
        }

        public IterationPicker(Tfs tfs)
        {
            InitializeComponent();

            //initialize dataset
            _dataSet = new DataSet();
            _dataTable = _dataSet.Tables.Add("IterationSelection");
            _dataTable.Columns.Add(new DataColumn("IsSelected", typeof(bool)));
            _dataTable.Columns.Add(new DataColumn("Platform", typeof(bool)));
            _dataTable.Columns.Add(new DataColumn("Iteration", typeof(string)));
            _dataTable.Columns.Add(new DataColumn("Id", typeof(int)));
            _dataTable.Columns[0].DefaultValue = false;

            _tfs = tfs;

            Projects.Items.Clear();
            Projects.Items.Add("-- Select --");
            Projects.SelectedIndex = 0;


            string selectedProject = "";
            if (ConfigurationManager.AppSettings["DefaultProject"] != null)
            {
                selectedProject = ConfigurationManager.AppSettings["DefaultProject"].ToString();
            }

            int i = 0;
            foreach (string project in _tfs.Projects)
            {
                Projects.Items.Add(project);

                if (project == selectedProject)
                {
                    _edsIndex = i + 1;
                }
                i++;
            }
        }

        private void IterationPicker_Load(object sender, EventArgs e)
        {
            if (_edsIndex > 0)
            {
                Projects.SelectedIndex = _edsIndex;
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastProject) &&
                       !string.IsNullOrWhiteSpace(Properties.Settings.Default.LastFilter))
            {
                btnLoadPreviousFilter.Enabled = true;
            }
            else
            {
                btnLoadPreviousFilter.Enabled = false;
            }

            if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.LastProject) &&
                !string.IsNullOrWhiteSpace(Properties.Settings.Default.LastSelection))
            {
                btnLoadSelection.Enabled = true;
            }
            else
            {
                btnLoadSelection.Enabled = false;
            }
        }

        private void Projects_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Projects.SelectedIndex == 0)
            {
                return;
            }

            var item = (string)Projects.SelectedItem;
            _dataTable.Rows.Clear();
            foreach (var iteration in _tfs.GetIterationPaths(item))
            {
                _dataTable.Rows.Add(false, false, iteration.Path, iteration.Id);
            }

            bindingSource_main.DataSource = _dataSet;
            bindingSource_main.DataMember = _dataTable.TableName;
            _grid.DataSource = bindingSource_main;
            _grid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            _grid.Columns[2].ReadOnly = true;
            _grid.Columns[3].Visible = false;

            btnSelectAll.Visible = _dataTable.Rows.Count > 0;
        }


        internal List<IterationSelection> SelectedIterations
        {
            get
            {
                if (_dataTable != null && _dataTable.Rows.Count > 0)
                {
                    var rows = _dataTable.AsEnumerable();
                    var parentIterations = rows
                                            .Where(r => r.Field<bool>("IsSelected"))
                                            .Select(r => new IterationSelection()
                                            {
                                                IsSelected = r.Field<bool>("IsSelected"),
                                                Platform = r.Field<bool>("Platform"),
                                                Iteration = new Iteration(r.Field<int>("Id"), r.Field<string>("Iteration"))

                                            }).ToList();

                    foreach (var parentIteration in parentIterations)
                    {
                        var childIterations = rows
                            .Where(r => r.Field<string>("Iteration").IndexOf(parentIteration.Iteration.Path + @"\") > -1)
                            .Select(r => new Iteration(r.Field<int>("Id"), r.Field<string>("Iteration"))).ToList();
                        parentIteration.SubIterations = childIterations;
                    }

                    return parentIterations;

                }
                return null;
            }

        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in _grid.Rows)
            {
                row.Cells[0].Value = true;
            }

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.LastProject = Projects.SelectedItem.ToString();
            Properties.Settings.Default.LastFilter = _grid.FilterString;
            string selection = "";
            foreach (var iteration in SelectedIterations)
            {
                selection = selection + iteration.Path + "," + iteration.Platform + ";";
            }
            Properties.Settings.Default.LastSelection = selection;
            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _grid_FilterStringChanged(object sender, EventArgs e)
        {
            bindingSource_main.Filter = _grid.FilterString;
        }

        private void btnLoadPreviousFilter_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Projects.FindStringExact(Properties.Settings.Default.LastProject);
                Projects.SelectedIndex = index;
                _grid.LoadFilterAndSort(Properties.Settings.Default.LastFilter, "");
                _grid.EnableFilterAndSort(_grid.Columns[0]);
                _grid.EnableFilterAndSort(_grid.Columns[1]);
                btnSelectAll_Click(null, null);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnLoadSelection_Click(object sender, EventArgs e)
        {
            try
            {
                int index = Projects.FindStringExact(Properties.Settings.Default.LastProject);
                Projects.SelectedIndex = index;

                string[] selection = Properties.Settings.Default.LastSelection.Split(';');

                for (int i = 0; i < selection.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(selection[i])) continue;
                    string[] val = selection[i].Split(',');
                    var rows = _dataTable.AsEnumerable();
                    var incRow = rows.Where(r => r.Field<string>("Iteration") == val[0]);
                    var row = incRow.FirstOrDefault();
                    if (row != null)
                    {
                        row.SetField(0, true);
                        row.SetField(1, val[1].ToLower() == "true");
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}
