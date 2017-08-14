using Microsoft.TeamFoundation.WorkItemTracking.WpfControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.TeamFoundation.WorkItemTracking.Client;
using Microsoft.TeamFoundation.Framework.Client;
using Microsoft.TeamFoundation.Framework.Common;
using System.Windows.Input;
using System.Windows.Controls.Primitives;

namespace PIPlanner
{
    internal static class TableHelper
    {
        public static Tfs _tfs;
        public static Grid _table;
        private static Point startPoint;
        private static List<ListViewItem> _listViewItems = null;
        private static StringBuilder _sb = null;

        internal static void SetTable(List<IterationSelection> selections, Grid table, Tfs tfs)
        {
            _listViewItems = new List<ListViewItem>();
            _sb = new StringBuilder();
            ClearTable(table);
            System.Windows.Forms.Application.DoEvents();
            AddTeams(selections, table);
            System.Windows.Forms.Application.DoEvents();

            List<IterationSelection> engSelections = selections.Where(sel => !sel.Platform).ToList();
            List<IterationSelection> pltfrmSelections = selections.Where(sel => sel.Platform).ToList();
            AddIterations(engSelections, pltfrmSelections, table, tfs);
            _listViewItems.Clear();
            _sb = null;
        }

        private static void ClearTable(Grid table)
        {
            table.RowDefinitions.Clear();
            table.ColumnDefinitions.Clear();
            table.Children.Clear();
        }

        private static void AddIterations(List<IterationSelection> selections, List<IterationSelection> pltfrmSelections, Grid table, Tfs tfs)
        {
            //Find all iterations for all the teams in selections
            var iterationsDict = new Dictionary<string, List<Iteration>>();
            foreach (var selection in selections)
            {
                if (selection.Platform) continue;
                foreach (var subIteration in selection.SubIterations)
                {
                    string subIterationText = GetIterationText(subIteration.Path);
                    if (!iterationsDict.Keys.Contains(subIterationText))
                    {
                        iterationsDict.Add(subIterationText, new List<Iteration>() { subIteration });
                    }
                    else
                    {
                        iterationsDict[subIterationText].Add(subIteration);
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
            }

            table.ColumnDefinitions.Add(new ColumnDefinition());
            int column = 1;
            foreach (var iteration in iterationsDict.Keys)
            {
                table.ColumnDefinitions.Add(new ColumnDefinition());
                var lbl = new TextBlock()
                {
                    Tag = iterationsDict[iteration],
                    Text = iteration,
                    Foreground = System.Windows.Media.Brushes.DarkSlateBlue,
                    FontWeight = FontWeights.UltraBold,
                    TextWrapping = TextWrapping.Wrap,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Padding = new Thickness(0, 5, 0, 5)
                };
                Grid.SetRow(lbl, 0);
                Grid.SetColumn(lbl, column);
                table.Children.Add(lbl);

                int row = 0;
                foreach (var teamIteration in iterationsDict[iteration])
                {
                    var sp = new StackPanel();

                    var selection = selections.FirstOrDefault(it => it.SubIterations.FirstOrDefault(sit => sit.Id == teamIteration.Id) != null);

                    for (int i = 0; i <= _table.RowDefinitions.Count; i++)
                    {
                        if (table.RowDefinitions[i].Tag == selection.Iteration)
                        {
                            row = i;
                            break;
                        }
                    }

                    var header = new TextBlock() { Tag = teamIteration, Text = teamIteration.Path, Background = Brushes.Black, Foreground = Brushes.White };
                    header.PreviewDragEnter += header_DragEnter;
                    header.PreviewDrop += header_Drop;
                    header.AllowDrop = true;
                    header.Padding = new Thickness(2);
                    var lv = new ListView();
                    lv.PreviewMouseLeftButtonDown += lv_PreviewMouseLeftButtonDown;
                    lv.PreviewMouseMove += lv_PreviewMouseMove;
                    lv.Tag = teamIteration;

                    var teamIterationWorkItems = tfs.GetWorkItemsUnderIterationPath(teamIteration.Path);
                    List<ListViewItem> wis = new List<ListViewItem>();
                    foreach (var teamIterationWorkItem in teamIterationWorkItems)
                    {
                        var lvi = GetLviForWi(teamIterationWorkItem, true);
                        wis.Add(lvi);
                    }
                    lv.ItemsSource = wis;

                    sp.Children.Add(header);
                    sp.Children.Add(lv);

                    Grid.SetRow(sp, row);
                    Grid.SetColumn(sp, column);
                    table.Children.Add(sp);

                    System.Windows.Forms.Application.DoEvents();
                }

                foreach (var pltfrmSel in pltfrmSelections)
                {
                    var sp = new StackPanel();

                    // Find the row corresponding to the current team.
                    for (int i = 0; i <= _table.RowDefinitions.Count; i++)
                    {
                        if (table.RowDefinitions[i].Tag == pltfrmSel.Iteration)
                        {
                            row = i;
                            break;
                        }
                    }

                    var teamIterationWorkItems = tfs.GetWorkItemsWithTag("ENG-" + iteration, pltfrmSel.Path);


                    var header = new TextBlock() { Tag = iteration, Text = pltfrmSel.Path, Background = Brushes.Black, Foreground = Brushes.White };
                    header.PreviewDragEnter += header_DragEnter;
                    header.PreviewDrop += header_Drop;
                    header.AllowDrop = true;
                    header.Padding = new Thickness(2);
                    var lv = new ListView();
                    lv.PreviewMouseLeftButtonDown += lv_PreviewMouseLeftButtonDown;
                    lv.PreviewMouseMove += lv_PreviewMouseMove;
                    lv.Tag = "ENG-" + iteration;

                    List<ListViewItem> wis = new List<ListViewItem>();
                    foreach (var teamIterationWorkItem in teamIterationWorkItems)
                    {
                        var lvi = GetLviForWi(teamIterationWorkItem, true);
                        wis.Add(lvi);
                    }

                    lv.ItemsSource = wis;

                    sp.Children.Add(header);
                    sp.Children.Add(lv);

                    Grid.SetRow(sp, row);
                    Grid.SetColumn(sp, column);
                    table.Children.Add(sp);

                    System.Windows.Forms.Application.DoEvents();
                }

                column++;
            }

            SetAllDependencies();
        }

        private static void SetAllDependencies()
        {
            string allIds = _sb.ToString();
            
            if (allIds.Length > 0)
            {
                allIds = allIds.Remove(allIds.Length - 1);
                var wis = _tfs.GetDependentItems(allIds);
                string ids = "";

                foreach (var lvi in _listViewItems)
                {
                    WorkItem teamIterationWorkItem = lvi.Tag as WorkItem;
                    var txtBlock = (TextBlock)lvi.Content;
                    var links = wis.Where(wi => wi.SourceId == teamIterationWorkItem.Id);

                    foreach (var link in links)
                    {
                        CreateDependencyText(txtBlock, ref ids, link, teamIterationWorkItem);
                        System.Windows.Forms.Application.DoEvents();
                    }
                }
            }
        }

        public static void lv_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // Store the mouse position
            startPoint = e.GetPosition(null);
        }

        public static void lv_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (startPoint.X == -1 && startPoint.X == -1)
                return;
            // Get the current mouse position
            Point mousePos = e.GetPosition(null);
            Vector diff = startPoint - mousePos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance ||
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                // Get the dragged ListViewItem
                ListView listView = sender as ListView;
                ListViewItem listViewItem =
                    FindAnchestor<ListViewItem>((DependencyObject)e.OriginalSource);
                if (listViewItem != null)
                {
                    WorkItem workItem = listViewItem.Tag as WorkItem;
                    if (workItem != null)
                    {
                        // Initialize the drag & drop operation
                        var data = new DragDropData() { OriginListView = listView, WorkItem = workItem };
                        DataObject dragData = new DataObject(typeof(DragDropData), data);
                        DragDrop.DoDragDrop(listViewItem, dragData, DragDropEffects.Move);
                        startPoint = new Point(-1, -1);
                    }
                }
            }
        }

        private static T FindAnchestor<T>(DependencyObject current)
    where T : DependencyObject
        {
            do
            {
                if (current is T)
                {
                    return (T)current;
                }
                current = VisualTreeHelper.GetParent(current);
            }
            while (current != null);
            return null;
        }

        public static IEnumerable<T> FindChildren<T>(DependencyObject depObj) where T : DependencyObject
        {
            if (depObj != null)
            {
                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(depObj); i++)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(depObj, i);
                    if (child != null && child is T)
                    {
                        yield return (T)child;
                    }

                    foreach (T childOfChild in FindChildren<T>(child))
                    {
                        yield return childOfChild;
                    }
                }
            }
        }

        public static void AddWorkItemToSprint(ListView lv, WorkItem teamIterationWorkItem)
        {
            var lvi = GetLviForWi(teamIterationWorkItem);
            var list = lv.ItemsSource as List<ListViewItem>;
            list.Add(lvi);
            lv.Items.Refresh();
        }

        public static ListViewItem GetLviForWi(WorkItem teamIterationWorkItem, bool dependencyCollectMode = false)
        {
            var lvi = new ListViewItem();
            lvi.Content = new TextBlock() { Text = teamIterationWorkItem.ToLabel(), HorizontalAlignment = HorizontalAlignment.Stretch, IsHitTestVisible = false };
            lvi.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            lvi.Tag = teamIterationWorkItem;

            if (!dependencyCollectMode)
                SetDependency(teamIterationWorkItem, lvi);
            else
            {
                _listViewItems.Add(lvi);
                _sb.Append(teamIterationWorkItem.Id + ",");
            }

            lvi.MouseDoubleClick += lvi_MouseDoubleClick;
            return lvi;
        }

        private static WorkItem GetWiFromBoard(int id)
        {
            var listViews = FindChildren<ListView>(_table);

            foreach (var listView in listViews)
            {
                foreach (ListViewItem lvi in listView.Items)
                {
                    var wi = lvi.Tag as WorkItem;
                    if (wi != null && wi.Id == id)
                    {
                        return wi;
                    }
                }
            }
            return null;
        }

        private static void SetDependency(WorkItem teamIterationWorkItem, ListViewItem lvi)
        {
            var txtBlock = (TextBlock)lvi.Content;
            string ids = "";

            var linkedWis = _tfs.GetDependentItems(teamIterationWorkItem.Id);

            foreach (var link in linkedWis)
            {

                CreateDependencyText(txtBlock, ref ids, link, teamIterationWorkItem);
                System.Windows.Forms.Application.DoEvents();
            }

            if (ids != "")
            {
                txtBlock.Tag = ids;
                var cmenu = new ContextMenu();
                txtBlock.ContextMenuOpening += txtBlock_ContextMenuOpening;
                txtBlock.ContextMenu = cmenu;
            }
        }

        private static void CreateDependencyText(TextBlock txtBlock, ref string ids, WorkItemLinkInfo link, WorkItem teamIterationWorkItem)
        {
            if (link.LinkTypeId == -3)
            {
                var relWi = GetWiFromBoard(link.TargetId);
                if (relWi == null)
                    relWi = _tfs.GetWorkItem(link.TargetId.ToString());
                if (teamIterationWorkItem.AreaId != relWi.AreaId) // only if dependency is from another team
                {
                    txtBlock.Inlines.Add(new System.Windows.Documents.Run
                    {
                        Background = Brushes.Blue,
                        Foreground = Brushes.White,
                        FontFamily = new FontFamily("Comic Sans"),
                        Text = System.Environment.NewLine + link.TargetId + " |" + relWi.Type.Name + "| " + relWi.IterationPath
                    });
                    txtBlock.Background = Brushes.Pink;
                    ids += link.TargetId + " ";
                }
            }
            if (link.LinkTypeId == 3 && LoadSuccesors)
            {
                var relWi = GetWiFromBoard(link.TargetId);
                if (relWi == null)
                    relWi = _tfs.GetWorkItem(link.TargetId.ToString());
                if (teamIterationWorkItem.AreaId != relWi.AreaId) // only if dependency is from another team
                {
                    txtBlock.Inlines.Add(new System.Windows.Documents.Run
                    {
                        Background = Brushes.MidnightBlue,
                        Foreground = Brushes.Pink,
                        FontFamily = new FontFamily("Comic Sans"),
                        Text = System.Environment.NewLine + link.TargetId + " |" + relWi.Type.Name + "| " + relWi.IterationPath
                    });
                    txtBlock.Background = Brushes.Pink;
                    ids += link.TargetId + " ";
                }
            }
        }

        static void txtBlock_ContextMenuOpening(object sender, ContextMenuEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine(sender.ToString());
            var txtBlock = e.Source as TextBlock;

            string ids = txtBlock.Tag as string;
            if (!string.IsNullOrWhiteSpace(ids))
            {
                var idList = ids.Split(' ');
                var newMenu = new ContextMenu();

                foreach (string id in idList)
                {
                    if (id.Trim() == "")
                        continue;

                    var mi = new MenuItem();
                    mi.Header = id;

                    var miOpen = new MenuItem();
                    miOpen.Header = "Open";
                    miOpen.Click += miOpen_Click;
                    miOpen.Tag = Convert.ToInt32(id.Trim());
                    mi.Items.Add(miOpen);


                    var miFind = new MenuItem();
                    miFind.Header = "Find";
                    miFind.Click += miFind_Click;
                    miFind.Tag = Convert.ToInt32(id.Trim());
                    mi.Items.Add(miFind);

                    newMenu.Items.Add(mi);
                }

                txtBlock.ContextMenu = newMenu;
            }
        }

        static void miFind_Click(object sender, RoutedEventArgs e)
        {
            var src = e.Source as FrameworkElement;
            if (src != null)
            {
                int id = (int)src.Tag;
                if (id > 0)
                {
                    var listViews = FindChildren<ListView>(_table);

                    foreach (var listView in listViews)
                    {
                        foreach (ListViewItem lvi in listView.Items)
                        {
                            var wi = lvi.Tag as WorkItem;
                            if (wi != null && wi.Id == id)
                            {

                                Point pointTransformToVisual = lvi.TransformToVisual(_table).Transform(new Point());
                                Rect boundsRect = VisualTreeHelper.GetDescendantBounds(lvi);
                                boundsRect.Offset(pointTransformToVisual.X, pointTransformToVisual.Y);
                                _table.BringIntoView(boundsRect);
                                listView.SelectedItem = lvi;
                                lvi.IsSelected = true;
                                lvi.Focus();
                                return;
                            }
                        }
                    }

                    MessageBox.Show(id + " Not found on the board");
                }
            }
        }

        static void miOpen_Click(object sender, RoutedEventArgs e)
        {
            var src = e.Source as FrameworkElement;
            if (src != null)
            {
                int id = (int)src.Tag;
                if (id > 0)
                {
                    var wi = _tfs.GetWorkItem(id.ToString());
                    if (wi != null)
                        ShowWorkItem(wi, id + " (Readonly)");
                }
            }
        }


        static void lvi_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                System.Windows.Forms.Application.OpenForms[0].Cursor = System.Windows.Forms.Cursors.WaitCursor;
                var lvi = sender as ListViewItem;
                if (lvi != null)
                {
                    ListView lv = FindAnchestor<ListView>(lvi);
                    var wi = lvi.Tag as WorkItem;


                    int iterationIdBefore = wi.IterationId;
                    ShowWorkItem(wi);
                    wi.Save();
                    wi.SyncToLatest();

                    if (lv.Name == "lvWorkItems") // dontmove item on save of item from Backlog List as it will crash
                        return;
                    int idAfter = wi.IterationId;
                    if (iterationIdBefore != iterationIdAfter)
                    {
                        var list = lv.ItemsSource as List<ListViewItem>;
                        list.Remove(lvi);
                        lv.Items.Refresh();
                        Grid grd = FindAnchestor<Grid>(lv);
                        var listViews = FindChildren<ListView>(grd);

                        foreach (var listView in listViews)
                        {
                            var iter = listView.Tag as Iteration;
                            if (iter != null && iter.Id == idAfter)
                            {
                                AddWorkItemToSprint(listView, wi);
                                break;
                            }
                        }
                    }
                    else
                    {
                        lvi.Content = new TextBlock() { Text = wi.ToLabel() };
                        SetDependency(wi, lvi);
                    }
                }
            }
            finally
            {
                System.Windows.Forms.Application.OpenForms[0].Cursor = System.Windows.Forms.Cursors.Default;
            }
        }

        private static void ShowWorkItem(WorkItem wi, string caption = "")
        {
            WorkItemControl witControl = new WorkItemControl();
            witControl.Item = wi;

            var container = new Window();
            if (caption == "")
                container.Title = wi.Id.ToString();
            else
                container.Title = caption;

            container.Content = witControl;
            container.ShowDialog();
        }

        static void header_DragEnter(object sender, DragEventArgs e)
        {
            if (!e.Data.GetDataPresent(typeof(string)) ||
                sender == e.Source)
            {
                e.Effects = DragDropEffects.None;
            }
        }

        static void header_Drop(object sender, DragEventArgs e)
        {
            try
            {
                if (e.Data.GetDataPresent(typeof(DragDropData)))
                {
                    TextBlock tb = sender as TextBlock;
                    if (tb != null)
                    {
                        StackPanel sp = tb.Parent as StackPanel;
                        if (sp != null)
                        {
                            var lv = sp.Children[1] as ListView;
                            if (lv != null)
                            {
                                var obj = e.Data.GetData(typeof(DragDropData));

                                DragDropData ddd = (DragDropData)obj;
                                ListView originLV = ddd.OriginListView;
                                WorkItem wi = ddd.WorkItem;

                                Iteration iter = lv.Tag as Iteration;
                                wi.PartialOpen();
                                wi.IterationId = iter.Id;
                                wi.IterationPath = iter.Path;
                                wi.Save(SaveFlags.MergeAll);
                                AddWorkItemToSprint(lv, wi);

                                foreach (object item in originLV.Items)
                                {
                                    if (item.GetType() == typeof(ListViewItem))
                                    {
                                        var tempWi = ((ListViewItem)item).Tag as WorkItem;
                                        if (tempWi != null && wi == tempWi)
                                        {
                                            var list = originLV.ItemsSource as List<ListViewItem>;
                                            list.Remove((ListViewItem)item);
                                            originLV.Items.Refresh();
                                            break;
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
        }

        private static void AddTeams(List<IterationSelection> selections, Grid table)
        {
            table.RowDefinitions.Add(new RowDefinition() { Height = GridLength.Auto });
            int row = 1;
            foreach (var selection in selections)
            {
                var rd = new RowDefinition();
                rd.Tag = selection.Iteration;
                table.RowDefinitions.Add(rd);
                string text = GetTeamName(selection.Iteration.Path);
                var lbl = new TextBlock()
                {
                    Tag = selection.Iteration,
                    Text = text,
                    Foreground = System.Windows.Media.Brushes.Green,
                    FontWeight = FontWeights.UltraBold,
                    TextWrapping = TextWrapping.Wrap,
                    Padding = new Thickness(4)
                };
                Grid.SetRow(lbl, row);
                Grid.SetColumn(lbl, 0);
                table.Children.Add(lbl);
                row++;
                System.Windows.Forms.Application.DoEvents();
            }
        }

        private static string GetTeamName(string iterationName)
        {
            string retVal = "";

            int first = iterationName.IndexOf(@"\") + 1;
            int second = iterationName.IndexOf(@"\", first + 1);

            if (second > -1)
            {
                retVal = iterationName.Substring(first, second - first);
            }
            else
            {
                retVal = iterationName.Substring(first);
            }

            return retVal;
        }

        private static string GetIterationText(string subIteration)
        {
            int lastIndexOfSlash = subIteration.LastIndexOf(@"\");
            string retVal = subIteration.Substring(lastIndexOfSlash + 1);

            // Hack: to get rid of Mack's brackets
            int lastIndexOfOpenBracket = retVal.IndexOf(@"(");
            int lastIndexOfCloseBracket = retVal.IndexOf(@")");
            if (lastIndexOfOpenBracket > -1 && lastIndexOfCloseBracket > -1)
            {
                retVal = retVal.Substring(0, lastIndexOfOpenBracket).Trim();
            }

            return retVal;
        }

        public static int iterationIdAfter { get; set; }

        public static bool LoadSuccesors { get; set; }
    }
}
