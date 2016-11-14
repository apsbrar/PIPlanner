using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPlanner
{
    internal class IterationSelection
    {
        public bool IsSelected { get; set; }
        public string Iteration { get; set; }

        public List<string> SubIterations { get; set; }
    }
}
