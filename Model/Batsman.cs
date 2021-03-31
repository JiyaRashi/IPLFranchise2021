using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.Model
{
    public class Batsman
    {
        public string BatsmanName { get; set; }
        public string Details { get; set; }
        public int Runs { get; set; }
        public int Balls { get; set; }
        public int Fours { get; set; }
        public int Sixes { get; set; }
        public double SR { get; set; }
    }
}
