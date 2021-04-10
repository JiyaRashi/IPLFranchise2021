using IPLFranchise2021.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPLFranchise2021.Logic
{
    public interface IRunsCalculatorLogic
    {
        int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR);
        int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
            int dot, int maiden, bool hatTrick);
        int FielderNamePoints(string name);
        bool IsValueableName(string name);
        int FielderEachTotalPoints(string name, int count);
    }
    public class RunsCalculatorLogic : IRunsCalculatorLogic
    {
        public int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR)
        {
            int totalScore = 0;

            int runTotalPoints = (runs >= 30 && runs <= 49) ? runs + 30 :
                                 (runs >= 50 && runs <= 69) ? runs + 50 :
                                 (runs >= 70 && runs <= 99) ? runs + 70 :
                                 (runs >= 100) ? runs + 200 :
                                 runs;

            int sixesPoints = (sixes >= 5 && sixes >= 9) ? sixes * 16 + 70 :
                              (sixes >= 10) ? sixes * 16 + 150 :
                              sixes * 16;

            int fourPoints = (fours >= 10 && fours >= 14) ? fours * 9 + 60 :
                              (fours >= 15) ? fours * 9 + 100 :
                              fours * 9;

            int srPoints = Convert.ToInt32(SR);

            srPoints = (srPoints >= 250 && srPoints >= 349) ? 70 :
                             (srPoints >= 350) ? 120 :
                             (srPoints <= 50 && balls >= 5) ? -20 : 0;

            int ducks = runs == 0 ? -20 : 0;


            return totalScore + runTotalPoints + sixesPoints + fourPoints + srPoints + ducks;
        }
        public int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
            int dot, int maiden, bool hatTrick)
        {
            int _bowlTotalPoints = 0;

            int wicketPoints = (wickets == 1) ? 30 :
                (wickets == 2) ? 60 :
                (wickets == 3) ? 100 :
                (wickets == 4) ? 150 :
                (wickets == 5) ? 250 : 0;
            int maidenPoints = maiden * 70;
            int hattrickPoints = hatTrick ? 200 : 0;
            int econPoints = Convert.ToInt32(Econ);
            econPoints = (econPoints <= 3) ? 150 :
                (econPoints == 4) ? 100 :
                (econPoints == 5) ? 70 :
                (econPoints == 6) ? 50 :
                (econPoints == 7) ? 40 :
                (econPoints == 8) ? -10 :
                (econPoints == 9) ? -20 :
                (econPoints == 10) ? -30 :
                (econPoints >= 11) ? -40 : 0;

            return _bowlTotalPoints + wicketPoints + maidenPoints + hattrickPoints + econPoints;
        }


        public int FielderNamePoints(string name)
        {
            string[] stringSeparators = new string[] { " " };

            string[] name1 = name.Split(stringSeparators, StringSplitOptions.None);

            bool catcher = name1[0].Contains("c");
            bool LBW = name1[0].Contains("lbw");
            bool bowled = name1[0].Contains("b");
            bool stumbed = name1[0].Contains("st");
            bool runout = name1[0].Contains("run");

            int points = LBW ? 10 :
                bowled ? 10 :
                catcher ? 25 :
                stumbed ? 30 :
                runout ? 50 : 0;
            return points;
        }
        public bool IsValueableName(string name)
        {
            string[] stringSeparators = new string[] { " & ", " b " };

            string[] otherDetails = name.Split(stringSeparators, StringSplitOptions.None);

            foreach (string author in otherDetails)
            {
                Regex r = new Regex("lbw |b |c |st| & |run out");
                bool containsAny = r.IsMatch(author);
                if ((author.ToUpper() != "\tNOT OUT\t" && containsAny))
                {

                    return true;
                }
            }

            return false; ;
        }

        public int FielderEachTotalPoints(string name, int count)
        {
            int points = 0;
            if (name.Contains("c & b"))
            {
                points = 10;
            }
            else
            {
                var result = Regex.Match(name, @"^([\w\-]+)");

                bool LBW = result.Value.Contains("lbw");
                bool bowled = result.Value.Contains("b");
                bool catcher = result.Value.Contains("c");
                bool stumbed = result.Value.Contains("st");
                bool runout = result.Value.Contains("run");

                points = LBW ? 10 :
                    bowled ? 10 :
                    catcher ? 25 :
                    stumbed ? 30 :
                    runout ? 50 : 0;

                if (catcher || stumbed)
                {
                    points = (count >= 3) ? count * 25 + 70 : points * count;
                }
                else
                {
                    points = points * count;
                }
            }

            return points;
        }
    }
}
