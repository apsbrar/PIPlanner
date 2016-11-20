using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PIPlanner
{
    public class DragDropData
    {
        public WorkItem WorkItem { get; set; }
        public ListView OriginListView { get; set; }
    }
}
