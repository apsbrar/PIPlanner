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

namespace PIPlanner
{
    internal static class TableHelper
    {
        public static Tfs _tfs;
        private static Point startPoint;
        internal static void SetTable(List<IterationSelection> selections, Grid table, Tfs tfs)
        {
            ClearTable(table);

            AddTeams(selections, table);

            AddIterations(selections, table, tfs);
        }

        private static void ClearTable(Grid table)
        {
            table.RowDefinitions.Clear();
            table.ColumnDefinitions.Clear();
            table.Children.Clear();
        }

        private static void AddIterations(List<IterationSelection> selections, Grid table, Tfs tfs)
        {
            //Find all iterations for all the teams in selections
            var iterationsDict = new Dictionary<string, List<Iteration>>();
            foreach (var selection in selections)
            {
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

                int row = 1;
                foreach (var teamIteration in iterationsDict[iteration])
                {
                    var sp = new StackPanel();

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
                    foreach (var teamIterationWorkItem in teamIterationWorkItems)
                    {
                        AddWorkItemToSprint(lv, teamIterationWorkItem);
                    }

                    sp.Children.Add(header);
                    sp.Children.Add(lv);

                    Grid.SetRow(sp, row);
                    Grid.SetColumn(sp, column);
                    table.Children.Add(sp);

                    row++;
                }

                column++;
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
            var lvi = new ListViewItem();
            lvi.Content = new TextBlock() { Text = teamIterationWorkItem.ToLabel(), HorizontalAlignment= HorizontalAlignment.Stretch };
            lvi.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            lvi.Tag = teamIterationWorkItem;

            //var dependencies = _tfs.GetDependentItemIds(teamIterationWorkItem.Id);
            //foreach (var dependency in dependencies)
            //{
            //    if (dependency.SourceId == teamIterationWorkItem.Id)
            //    {
            //        lvi.Content = lvi.Content.ToString() + System.Environment.NewLine + dependency.TargetId;
            //        lvi.Background = Brushes.Pink;
            //    }
            //}

            SetDependency(teamIterationWorkItem, lvi);

            lvi.MouseDoubleClick += lvi_MouseDoubleClick;
            lv.Items.Add(lvi);
        }

        private static void SetDependency(WorkItem teamIterationWorkItem, ListViewItem lvi)
        {
            var txtBlock = (TextBlock)lvi.Content;
            foreach (Link link in teamIterationWorkItem.Links)
            {
                if (link.GetType() == typeof(RelatedLink))
                {
                    var rel = link as RelatedLink;
                    if (rel.LinkTypeEnd.Name == "Predecessor")
                    {
                        txtBlock.Text = txtBlock.Text.ToString() + System.Environment.NewLine + rel.RelatedWorkItemId;
                        txtBlock.Background = Brushes.Pink;
                    }
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


                    var witControl = new WorkItemControl();
                    witControl.Item = lvi.Tag as WorkItem;

                    var container = new Window();
                    container.Content = witControl;
                    int idBefore = witControl.Item.IterationId;
                    container.ShowDialog();
                    witControl.Item.Save();
                    witControl.Item.SyncToLatest();
                    if (lv.Name == "lvWorkItems") // dontmove item on save of item from Backlog List as it will crash
                        return;
                    int idAfter = witControl.Item.IterationId;
                    if (idBefore != idAfter)
                    {
                        lv.Items.Remove(lvi);
                        Grid grd = FindAnchestor<Grid>(lv);
                        var listViews = FindChildren<ListView>(grd);

                        foreach (var listView in listViews)
                        {
                            var iter = listView.Tag as Iteration;
                            if (iter != null && iter.Id == idAfter)
                            {
                                AddWorkItemToSprint(listView, witControl.Item);
                                break;
                            }
                        }
                    }
                    else
                    {
                        lvi.Content = new TextBlock() { Text = witControl.Item.ToLabel() };
                        SetDependency(witControl.Item, lvi);
                    }
                }
            }
            finally
            {
                System.Windows.Forms.Application.OpenForms[0].Cursor = System.Windows.Forms.Cursors.Default;
            }
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
                                            originLV.Items.Remove(item);
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
                table.RowDefinitions.Add(new RowDefinition());
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
    }
}
