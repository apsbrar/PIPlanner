using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PIPlanner
{
    internal static class TableHelper
    {
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
            var iterationsDict = new Dictionary<string, List<string>>();
            foreach (var selection in selections)
            {
                foreach (var subIteration in selection.SubIterations)
                {
                    string subIterationText = GetIterationText(subIteration);
                    if (!iterationsDict.Keys.Contains(subIterationText))
                    {
                        iterationsDict.Add(subIterationText, new List<string>() { subIteration });
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
                    
                    var header = new TextBlock() {Tag = teamIteration, Text = teamIteration, Background= Brushes.Black,  Foreground = Brushes.White};
                    header.PreviewDragEnter += header_DragEnter;
                    header.PreviewDrop += header_Drop;
                    header.AllowDrop = true;
                    header.Padding = new Thickness(2);
                    var lv = new ListView();
                    lv.Tag = teamIteration;

                    var teamIterationWorkItems = tfs.GetWorkItemsInIterationPath(teamIteration);
                    foreach (var teamIterationWorkItem in teamIterationWorkItems)
                    {
                        var lvi = new ListViewItem();
                        lvi.Content = teamIterationWorkItem;
                        lv.Items.Add(lvi);
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
                if (e.Data.GetDataPresent(typeof(string)))
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
                                string val = (string)e.Data.GetData(typeof(string));
                                foreach (object item in lv.Items)
                                {
                                    string str = "";
                                    if (item.GetType() == typeof(ListViewItem))
                                        str = ((ListViewItem)item).Content.ToString();
                                    if (item.GetType() == typeof(string))
                                        str = item.ToString();

                                    if (str == val)
                                    {
                                        return;
                                    }
                                }

                                lv.Items.Add(val);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
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
                string text = GetTeamName(selection.Iteration);
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

            retVal = iterationName.Substring(first, second - first);

            return retVal;
        }

        private static string GetIterationText(string subIteration)
        {
            int lastIndexOfSlash = subIteration.LastIndexOf(@"\");
            string retVal = subIteration.Substring(lastIndexOfSlash + 1);

            // Hack: to get rid of Mack's brackets
            int indexOfSpace = retVal.IndexOf(@" ");
            if (indexOfSpace > -1)
            {
                retVal = retVal.Substring(0, indexOfSpace).Trim();
            }

            return retVal;
        }
    }
}
