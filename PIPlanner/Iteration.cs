using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPlanner
{
    public class Iteration
    {
        int _id;
        string _path;

        public int Id
        {
            get { return _id; }
        }
        public string Path
        {
            get { return _path; }
        }

        public Iteration(int id, string path)
        {
            _id = id;
            _path = path;
        }
    }
}
