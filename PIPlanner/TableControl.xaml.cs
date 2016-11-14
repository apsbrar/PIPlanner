using Northwoods.GoXam;
using Northwoods.GoXam.Layout;
using Northwoods.GoXam.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TFSIterationPath
{
    /// <summary>
    /// Interaction logic for TableControl.xaml
    /// </summary>
    public partial class TableControl : UserControl
    {
        GraphModel<Item, String> _model = new GraphModel<Item, String>();
        List<string> _teams = new List<string>();
        List<string> _iterations = new List<string>();

        public TableControl()
        {
            InitializeComponent();

            // Create the model holding all of the regular nodes and links,
            // and also the nodes representing the Level headers and the Area headers
            _model.Modifiable = true;
            _model.NodesSource = new ObservableCollection<Item>();

            myDiagram.Model = _model;

            myDiagram.Layout = new TableLayout();
        }

        public void AddTeam(string teamName)
        {
            _teams.Add(teamName);
            _model.AddNode(new Item() { Category = "Team", Text = teamName, Team = 1, Iteration = 0 });
        }

        public void AddIteration(string iterationName)
        {
            _iterations.Add(iterationName);
            _model.AddNode(new Item() { Category = "Iteration", Text = iterationName, Team = 0, Iteration = 1 });
        }

        public void AddWorkItem(string workItem, string teamName, string iteartionName)
        {
            int teamIndex = _teams.IndexOf(teamName) + 1;
            int iterationIndex = _teams.IndexOf(teamName) + 1;

            _model.AddNode(new Item() { Key = workItem, Team = teamIndex, Iteration = iterationIndex });
        }
    }

    // Data for each node
    // Assume the headers use the Text property
    public class Item : GraphModelNodeData<String>
    {
        // these properties are assumed to be unchanging, only set at initialization,
        // so they don't need to call RaisePropertyChanged when the value changes
        public int Team { get; set; }
        public int Iteration { get; set; }

        // TODO: add your own properties, loaded from your database,
        // so that you can data-bind to them in XAML
    }


    public class TableLayout : DiagramLayout
    {
        public TableLayout()
        {
            this.ColumnSpacing = 10;
            this.RowSpacing = 10;
            this.NodeMargins = new Thickness(11);
        }

        public double ColumnSpacing { get; set; }
        public double RowSpacing { get; set; }
        public Thickness NodeMargins { get; set; }

        public override void DoLayout(IEnumerable<Node> nodes, IEnumerable<Link> links)
        {
            foreach (Node n in nodes) n.Move(new Point(0, 0), false);  // make sure each Node has a Position and a Size

            // figure out how many levels and areas there are,
            int maxLevel = 0;  // assume level zero holds Area headers
            int maxArea = 0;   // assume area zero holds Level headers
            foreach (Node n in nodes)
            {
                Item d = (Item)n.Data;
                maxLevel = Math.Max(maxLevel, d.Team);
                maxArea = Math.Max(maxArea, d.Iteration);
            }
            maxLevel++;  // zero-based arrays, assume exclusive maximum
            maxArea++;

            // then collect them in the array
            myArray = new Cell[maxArea, maxLevel];
            for (int i = 0; i < maxArea; i++)
            {
                for (int j = 0; j < maxLevel; j++)
                {
                    myArray[i, j] = new Cell();
                }
            }
            foreach (Node n in nodes)
            {
                Item d = (Item)n.Data;
                myArray[d.Iteration, d.Team].Nodes.Add(n);
            }

            // now figure out reasonable cell sizes for each cell
            for (int i = 0; i < maxArea; i++)
            {
                for (int j = 0; j < maxLevel; j++)
                {
                    Cell cell = myArray[i, j];
                    if (cell.Nodes.Count == 0) continue;
                    double nw = 0;
                    double nh = 0;
                    foreach (Node n in cell.Nodes)
                    {
                        nw = Math.Max(nw, n.Bounds.Width);
                        nh = Math.Max(nh, n.Bounds.Height);
                    }
                    nw += this.NodeMargins.Left + this.NodeMargins.Right;
                    nh += this.NodeMargins.Top + this.NodeMargins.Bottom;
                    int cols = (int)Math.Ceiling(Math.Sqrt(cell.Nodes.Count));
                    cell.Bounds = new Rect(0, 0, nw * cols, nh * Math.Ceiling(cell.Nodes.Count / (double)cols));
                    cell.MaxSize = new Size(nw, nh);
                }
            }

            // then line up all of the rows and columns
            // first normalize all column widths
            for (int i = 0; i < maxArea; i++)
            {
                double w = 0;
                for (int j = 0; j < maxLevel; j++)
                {
                    Cell cell = myArray[i, j];
                    w = Math.Max(w, cell.Bounds.Width);
                }
                for (int j = 0; j < maxLevel; j++)
                {
                    Cell cell = myArray[i, j];
                    cell.Bounds.Width = w + this.ColumnSpacing;
                }
            }
            // then normalize all row heights
            for (int j = 0; j < maxLevel; j++)
            {
                double h = 0;
                for (int i = 0; i < maxArea; i++)
                {
                    Cell cell = myArray[i, j];
                    h = Math.Max(h, cell.Bounds.Height);
                }
                for (int i = 0; i < maxArea; i++)
                {
                    Cell cell = myArray[i, j];
                    cell.Bounds.Height = h + this.RowSpacing;
                }
            }
            // then set the X,Y for each cell
            {
                double x = 0;
                double y = 0;
                for (int i = 0; i < maxArea; i++)
                {
                    for (int j = 0; j < maxLevel; j++)
                    {
                        Cell cell = myArray[i, j];
                        cell.Bounds.X = x;
                        cell.Bounds.Y = y;
                        y += cell.Bounds.Height;
                    }
                    x += myArray[i, 0].Bounds.Width;
                    y = 0;
                }
            }

            // assign positions to each node within each cell
            for (int i = 0; i < maxArea; i++)
            {
                for (int j = 0; j < maxLevel; j++)
                {
                    Cell cell = myArray[i, j];
                    if (cell.Nodes.Count == 0) continue;
                    if (cell.Nodes.Count == 1)
                    {
                        Node n = cell.Nodes[0];
                        Size nsize = new Size(n.Bounds.Width, n.Bounds.Height);
                        Rect nbounds = Spot.Center.RectForPoint(Spot.Center.PointInRect(cell.Bounds), nsize);
                        n.Move(new Point(nbounds.X, nbounds.Y), true);
                    }
                    else
                    {
                        double left = cell.Bounds.X + this.ColumnSpacing / 2 + this.NodeMargins.Left;
                        double limit = cell.Bounds.X + cell.Bounds.Width;
                        double x = left;
                        double y = cell.Bounds.Y + this.RowSpacing / 2 + this.NodeMargins.Top;
                        foreach (Node n in cell.Nodes)
                        {
                            n.Move(new Point(x, y), true);
                            if (x + n.Bounds.Width + this.NodeMargins.Left + this.NodeMargins.Right < limit)
                            {
                                x += cell.MaxSize.Width;
                            }
                            else
                            {
                                x = left;
                                y += cell.MaxSize.Height;
                            }
                        }
                    }
                }
            }

            // add separator lines between the rows and columns
            // First, create the unbound Node that holds the Canvas holding the separator Lines
            if (this.Separators == null)
            {
                Node sepnode = new Node();
                this.Separators = new Canvas();
                // this unbound Node has this Canvas as its VisualElement
                sepnode.Content = this.Separators;
                sepnode.Location = new Point(0, 0);  // it's always positioned at 0,0
                sepnode.LayerName = "Background";  // it's in the Background layer
                sepnode.LayoutId = "None";  // it's not passed to this TableLayout to be laid out
                sepnode.Avoidable = false;  // it doesn't affect the routing of AvoidsNodes links
                if (this.Diagram.PartsModel != null)
                {  // if the PartsModel exists, use it
                    this.Diagram.PartsModel.AddNode(sepnode);
                }
                else
                {  // otherwise postpone until the Diagram's Template is applied
                    this.Diagram.InitialParts.Add(sepnode);
                }
            }
            Canvas canvas = this.Separators;
            canvas.Children.Clear();  // remove any old Lines

            Cell first = myArray[0, 0];
            Cell last = myArray[maxArea - 1, maxLevel - 1];
            double totalwidth = last.Bounds.X + last.Bounds.Width - first.Bounds.X;
            double totalheight = last.Bounds.Y + last.Bounds.Height - first.Bounds.Y;
            canvas.Width = totalwidth;
            canvas.Height = totalheight;

            // vertical separator lines
            for (int i = 0; i < maxArea; i++)
            {
                Cell cell0 = myArray[i, 0];
                Line line = new Line();
                Canvas.SetLeft(line, 0);
                Canvas.SetTop(line, 0);
                line.Stroke = new SolidColorBrush(Colors.Brown);
                line.StrokeThickness = 2;
                line.StrokeDashArray = new DoubleCollection() { 2, 2 };
                line.X1 = cell0.Bounds.X + cell0.Bounds.Width;
                line.X2 = line.X1;
                line.Y1 = cell0.Bounds.Y + cell0.Bounds.Height;
                line.Y2 = cell0.Bounds.Y + totalheight;
                canvas.Children.Add(line);
            }
            // horizontal separator lines
            for (int j = 0; j < maxLevel; j++)
            {
                Cell cell0 = myArray[0, j];
                Cell cellN = myArray[maxArea - 1, j];
                Line line = new Line();
                Canvas.SetLeft(line, 0);
                Canvas.SetTop(line, 0);
                line.Stroke = new SolidColorBrush(Colors.Brown);
                line.StrokeThickness = 2;
                line.StrokeDashArray = new DoubleCollection() { 2, 2 };
                line.X1 = cell0.Bounds.X + cell0.Bounds.Width;
                line.X2 = cell0.Bounds.X + totalwidth;
                line.Y1 = cell0.Bounds.Y + cell0.Bounds.Height;
                line.Y2 = line.Y1;
                canvas.Children.Add(line);
            }
        }

        private Cell[,] myArray;

        private class Cell
        {
            public List<Node> Nodes = new List<Node>();
            public Rect Bounds = new Rect(0, 0, 0, 0);
            public Size MaxSize = new Size(0, 0);
        }

        private Canvas Separators { get; set; }
    }
}
