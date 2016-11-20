using Microsoft.TeamFoundation.WorkItemTracking.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPlanner
{
    public static class WorkItemExtensions
    {
        public static string ToLabel(this WorkItem wi)
        {
            return wi.Fields["System.Id"].Value + ":" + wi.Fields["System.Title"].Value;
        }
    }
}
