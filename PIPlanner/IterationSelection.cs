using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPlanner
{
    [Serializable]
    public class IterationSelection
    {
        public bool IsSelected { get; set; }
        public bool Platform { get; set; }
        public Iteration Iteration { get; set; }

        public string Path { get { return Iteration.Path; } }

        public List<Iteration> SubIterations { get; set; }
    }
}
