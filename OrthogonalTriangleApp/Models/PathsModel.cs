using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrthogonalTriangleApp.Models
{
    internal class PathsModel
    {
        public List<LineModel> Path {  get; set; }
        public short sum { get; set; }
        public bool stop { get; set; }
    }
}
