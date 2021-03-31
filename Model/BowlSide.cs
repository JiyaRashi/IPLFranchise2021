using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.Model
{
    public class BowlSide
    {
        public string BowlerName { get; set; }
        public int Overs { get; set; }
        public int OverRuns { get; set; }
        public int Wickets { get; set; }
        public double Econ { get; set; }
        public int Dot { get; set; }
        public int Maiden { get; set; }
        public bool HatTrick { get; set; }

        public int BowlTotalScore { get; set; }
    }
}
