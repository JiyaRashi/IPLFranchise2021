using IPLFranchise2021.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.Logic
{
    public class PointsCalculator_FWCL
    {
        public int FWCL_BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR, string FielderDetails)
        {
            int totalScore = 0;

            int runTotalPoints = (runs >= PointsValue.BatRunMargin_30 && runs <= PointsValue.BatRunMargin_49) ? runs + PointsValue.BatRun30_Bonus_49 :
                                 (runs >= PointsValue.BatRunMargin_50 && runs <= PointsValue.BatRunMargin_69) ? runs + PointsValue.BatRun50_Bonus_69 :
                                 (runs >= PointsValue.BatRunMargin_70 && runs <= PointsValue.BatRunMargin_99) ? runs + PointsValue.BatRun70_Bonus_99 :
                                 (runs >= PointsValue.BatRunMargin_100) ? runs + PointsValue.FWCL_BatRunAbove100_Bonus :
                                 runs;

            int sixesPoints = (sixes >= 5 && sixes <= 9) ? sixes * PointsValue.SixBonus + PointsValue.FWCL_SixBonusGreater5 :
                              (sixes >= 10) ? sixes * PointsValue.SixBonus + PointsValue.FWCL_SixBonusGreater10 :
                              sixes * PointsValue.SixBonus;

            int fourPoints = (fours >= 10 && fours <= 14) ? fours * PointsValue.FourBouns + PointsValue.FWCL_FourBonusGreater10 :
                              (fours >= 15) ? fours * PointsValue.FourBouns + PointsValue.FWCL_FourBonusGreater15 :
                              fours * PointsValue.FourBouns;

            int srPoints = Convert.ToInt32(SR);

            srPoints = (srPoints >= 250 && srPoints <= 349) ? PointsValue.FWCL_SRgreater250 :
                             (srPoints >= 350) ? PointsValue.FWCL_SRgreater350 :
                             (srPoints <= 50 && balls >= 5 && runs != 0) ? -10 : 0;

            int ducks = (runs == 0 && (!FielderDetails.Trim().ToUpper().Contains("NOT OUT"))) ? -20 :
            (runs == 0 && (FielderDetails.Trim().ToUpper().Contains("NOT OUT")) && balls >= 5) ? -20 : 0;



            return totalScore + runTotalPoints + sixesPoints + fourPoints + srPoints + ducks;
        }

        public int FWCL_BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
           int dot, int maiden, bool hatTrick)
        {
            int _bowlTotalPoints = 0;

            int wicketPoints = (wickets == 1) ? 30 :
                (wickets == 2) ? 60 :
                (wickets == 3) ? 100 :
                (wickets == 4) ? 170 :
                (wickets == 5) ? 300 : 0;
            int maidenPoints = maiden * 80;
            int hattrickPoints = hatTrick ? 300 : 0;
            //int econPoints = Convert.ToInt32(Econ);
            Econ = (Econ <= 2.00) ? 250 :
                (Econ >= 2.01) && (Econ <= 3.00) ? 160 :
                (Econ >= 3.01) && (Econ <= 4.00) ? 100 :
                (Econ >= 4.01) && (Econ <= 5.00) ? 70 :
                (Econ >= 5.01) && (Econ <= 6.00) ? 50 :
                (Econ >= 6.01) && (Econ <= 7.00) ? 40 :
                (Econ >= 7.01) && (Econ <= 7.99) ? 0 :
                (Econ >= 8.00) && (Econ <= 8.99) ? -10 :
                (Econ >= 9.00) && (Econ <= 9.99) ? -20 :
                (Econ >= 10.00 && Econ <= 11.99) ? -30 :
                (Econ >= 12.00) ? -40 : 0;

            return _bowlTotalPoints + wicketPoints + maidenPoints + hattrickPoints + Convert.ToInt32(Econ);
        }

        public int FWCL_FielderNamePoints(string name)
        {
            if (name.Contains("c & b"))
            {
                return 35;
            }
            else
            {
                string[] stringSeparators = new string[] { " & ", " b " };

                string[] name1 = name.Split(stringSeparators, StringSplitOptions.None);
                int j = (name1[0].Length < 4) ? 1 : 0;

                bool catcher = name1[j].Contains("c ");
                bool LBW = name1[j].Contains("lbw ");
                bool bowled = name1[j].Contains("b ");
                bool stumbed = name1[j].Contains("st ");
                bool runout = name1[j].Contains("run ");

                int points = LBW ? 10 :
                    bowled ? 10 :
                    catcher ? 25 :
                    stumbed ? 40 :
                    runout ? 50 : 0;
                return points;
            }
        }
    }
}
