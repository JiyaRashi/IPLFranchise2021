using IPLFranchise2021.Common;
using IPLFranchise2021.Model;
using System;
using System.Collections;
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
        int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR,string FielderDetails);
        int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
            int dot, int maiden, bool hatTrick);
        int FielderNamePoints(string name);
        bool IsValueableName(string name);
        int FielderEachTotalBonousPoints(string name, int count);
        string GetName(string name, ObservableCollection<Batsman> batsmanName);
        string GetName(string name);
        string GetValName(string name);
        string GetNoDupName(string name);
        ArrayList GetRunOutpoints(string name);
    }
    public class RunsCalculatorLogic : IRunsCalculatorLogic
    {
        public int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR,string FielderDetails)
        {
            int totalScore = 0;

            int runTotalPoints = (runs >= PointsValue.BatRunMargin_30 && runs <= PointsValue.BatRunMargin_49) ? runs + PointsValue.BatRun30_Bonus_49 :
                                 (runs >= PointsValue.BatRunMargin_50 && runs <= PointsValue.BatRunMargin_69) ? runs + PointsValue.BatRun50_Bonus_69 :
                                 (runs >= PointsValue.BatRunMargin_70 && runs <= PointsValue.BatRunMargin_99) ? runs + PointsValue.BatRun70_Bonus_99 :
                                 (runs >= PointsValue.BatRunMargin_100) ? runs + PointsValue.FWCL_BatRunAbove100_Bonus :
                                 runs;

            int sixesPoints = (sixes >= 3 && sixes <= 4) ? sixes * PointsValue.SixBonus + 40 : (sixes >= 5 && sixes <= 9) ? sixes * PointsValue.SixBonus + 80 :
                              (sixes >= 10) ? sixes * PointsValue.SixBonus + 180 :
                              sixes * PointsValue.SixBonus;

            int fourPoints = (fours >= 5 && fours <= 9) ? fours * PointsValue.FourBouns + 30 : (fours >= 10 && fours <= 14) ? fours * PointsValue.FourBouns + 70 :
                              (fours >= 15) ? fours * PointsValue.FourBouns + 120 :
                              fours * PointsValue.FourBouns;

            int srPoints = Convert.ToInt32(SR);

            srPoints = (srPoints >= 250 && srPoints <= 349) ? 70 :
                             (srPoints >= 350) ? 120 :
                             (srPoints <= 50 && balls >= 5 && runs !=0) ? -10 : 0;

            int ducks = (runs == 0 && (!FielderDetails.Trim().ToUpper().Contains("NOT OUT"))) ? -20 : 
            (runs == 0 && (FielderDetails.Trim().ToUpper().Contains("NOT OUT"))&& balls >=5) ? -20 : 0;



            return totalScore + runTotalPoints + sixesPoints + fourPoints + srPoints + ducks;
        }
        public int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
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
                (Econ >= 8.00) && (Econ <= 8.99) ? 0 :
                (Econ >= 9.00) && (Econ <= 9.99) ? -10 :
                (Econ >= 10.00 && Econ <= 11.99) ? -20 :
                (Econ >= 12.00 && Econ <= 13.99) ? -30 :
                (Econ >= 14.00) ? -40 : 0;

            return _bowlTotalPoints + wicketPoints + maidenPoints + hattrickPoints + Convert.ToInt32(Econ);
        }


        public int FielderNamePoints(string name)
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

        public int FielderEachTotalBonousPoints(string name, int count)
        {
            int points = 0;
            if (name.Contains("c & b"))
            {
                points = 35 * count;
            }
            else
            {
                var result = Regex.Match(name, @"^([\w\-]+)");

                bool LBW = result.Value.Contains("lbw");
                bool catcher = result.Value.Contains("c");
                bool bowled = result.Value.Contains("b");
                bool stumbed = result.Value.Contains("st");
                bool runout = result.Value.Contains("run");

                points = LBW ? 10 :
                    bowled ? 10 :
                    catcher ? 25 :
                    stumbed ? 40 :
                    runout ? 50 : 0;

                if (catcher)
                {
                    points = (count >= 3) ? count * 25 + 80 : points * count;
                }
                else if (stumbed)
                {
                    points = (count >= 2) ? count * 25 + 70 : points * count;
                }
                else
                {
                    points = points * count;
                }
            }

            return points;
        }

        public string GetName(string name, ObservableCollection<Batsman> batsmanName)
        {
            ObservableCollection<string> CollectionName = new ObservableCollection<string>();

            string[] spaceSplit = new string[] { " " };
            string[] slashSplit = new string[] { "/" };
            List<char> charsToRemove = new List<char>() { ')' };


            foreach (var item in batsmanName)
            {
                string[] name11 = item.BatsmanName.Split(spaceSplit, StringSplitOptions.None);
                int value = name11.Length - 1;
                CollectionName.Add(name11[value].ToUpper());
            }

            string[] name1 = name.Split(spaceSplit, StringSplitOptions.None);

            if (name1[0].Equals("run"))
            {
                string[] name1222 = name.Split(slashSplit, StringSplitOptions.None);

                for (int i = 0; i < name1222.Length; i++)
                {
                    string n = name1222[i];
                    string[] name34341 = n.Split(spaceSplit, StringSplitOptions.None);
                    int value = name34341.Length - 1;
                    string str1 = Filter(name34341[value], charsToRemove);
                    CollectionName.Add(str1.ToUpper());
                }

            }

            int val = name1.Length - 1;

            string remainName = name1[val].ToUpper();
            string str = Filter(remainName, charsToRemove);
            bool r = CollectionName.Any(b => b.Contains(str));
            return name;
        }

        public string GetName(string name)
        {
            if (name.Contains("c & b"))
            {
                //string[] stringSeparators = new string[] { "c & b" };
                //string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
                return (name.Trim());
            }
            else
            {
                string[] stringSeparators = new string[] { " & ", " b ", "c & " };
                string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
                //int s = sName[0].Length;
                //int ss = sName[1].Length;
                return (sName[0].Length < 4) ? sName[1].Trim() : sName[0].Trim();
            }

            //string[] stringSeparators = new string[] { "c & b", " b ", "c & " };
            //string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            ////int s = sName[0].Length;
            ////int ss = sName[1].Length;
            //return (sName[0].Length < 4) ? sName[1].Trim() : sName[0].Trim();

        }

        public string GetValName(string name)
        {
            if (name.Contains("c & b"))
            {
                string[] stringSeparators = new string[] { "c & b" };
                string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
                return (sName[1].Trim());
            }
            else
            {
                string[] stringSeparators = new string[] { " & ", " b ", "c & " };
                string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
                //int s = sName[0].Length;
                //int ss = sName[1].Length;
                return (sName[0].Length < 4) ? sName[1].Trim() : sName[0].Trim();
            }

            //string[] stringSeparators = new string[] { "c & b", " b ", "c & " };
            //string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            ////int s = sName[0].Length;
            ////int ss = sName[1].Length;
            //return (sName[0].Length < 4) ? sName[1].Trim() : sName[0].Trim();

        }

        public string GetNoDupName(string name)
        {
            List<char> charsToRemove = new List<char>() { ')', '(' ,'[',']'};
            string[] stringSeparators = new string[] { "lbw ", "b ", "c ", "st ", "sub " };
            string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            if (sName.Length == 1)
            {
                return name;
            }
            else
            {
                string _name = name.Contains("run out") ? name : sName[1].Trim();
                string _name2 = (_name == "") ? sName[2].Trim() : _name;
                ArrayList namely = new ArrayList();
                if (_name2.Contains("("))
                {
                    return _name2 = Filter(_name2, charsToRemove);
                }

                return _name2;
            }
        }

        public ArrayList GetRunOutpoints(string name)
        {
            ArrayList namely = new ArrayList(); 
            string[] slashSplit = new string[] { "/" };
            List<char> charsToRemove = new List<char>() { ')','(','[',']' };
            string[] stringSeparators = new string[] { "run out"};
            string[] stringSeparatorsSub = new string[] { "sub " };

            string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            string[] runName = sName[1].Split(slashSplit, StringSplitOptions.None);
            for (int i = 0; i < runName.Length; i++)
            {
                string n = runName[i];
                string str1 = Filter(n, charsToRemove);
                string[] NoSubName = str1.Split(stringSeparatorsSub, StringSplitOptions.None);
                string FullName = str1.Contains("sub ") ? NoSubName[1] : NoSubName[0];
                namely.Add(FullName);

            }

            return namely;
        }

        private string Filter(string str, List<char> charsToRemove)
        {
            foreach (char c in charsToRemove)
            {
                str = str.Replace(c.ToString(), String.Empty);
            }

            return str;
        }
    }
}
