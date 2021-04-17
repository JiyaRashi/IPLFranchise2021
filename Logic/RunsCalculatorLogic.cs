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
        int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR);
        int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
            int dot, int maiden, bool hatTrick);
        int FielderNamePoints(string name);
        bool IsValueableName(string name);
        int FielderEachTotalBonousPoints(string name, int count);
        string GetName(string name, ObservableCollection<Batsman> batsmanName);
        string GetName(string name);
        string GetNoDupName(string name);
        ArrayList GetRunOutpoints(string name);
    }
    public class RunsCalculatorLogic : IRunsCalculatorLogic
    {
        public int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR)
        {
            int totalScore = 0;
            int FourBouns = 5;
            int SixBonus = 10;

            int runTotalPoints = (runs >= 30 && runs <= 49) ? runs + 30 :
                                 (runs >= 50 && runs <= 69) ? runs + 50 :
                                 (runs >= 70 && runs <= 99) ? runs + 70 :
                                 (runs >= 100) ? runs + 200 :
                                 runs;

            int sixesPoints = (sixes >= 5 && sixes <= 9) ? sixes * SixBonus + 70 :
                              (sixes >= 10) ? sixes * SixBonus + 150 :
                              sixes * SixBonus;

            int fourPoints = (fours >= 10 && fours <= 14) ? fours * FourBouns + 60 :
                              (fours >= 15) ? fours * FourBouns + 100 :
                              fours * FourBouns;

            int srPoints = Convert.ToInt32(SR);

            srPoints = (srPoints >= 250 && srPoints <= 349) ? 70 :
                             (srPoints >= 350) ? 120 :
                             (srPoints <= 50 && balls >= 5 && runs !=0) ? -20 : 0;

            int ducks = (runs == 0 && balls > 0) ? -20 : 0;


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
                (econPoints == 10 || econPoints == 11) ? -30 :
                (econPoints >= 12) ? -40 : 0;

            return _bowlTotalPoints + wicketPoints + maidenPoints + hattrickPoints + econPoints;
        }


        public int FielderNamePoints(string name)
        {
            string[] stringSeparators = new string[] {" & ", " b " };

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

        public int FielderEachTotalBonousPoints(string name, int count)
        {
            int points = 0;
            if (name.Contains("c & b"))
            {
                points = 10 * count;
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
            string[] stringSeparators = new string[] { " & ", " b ","c & "};
            string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            //int s = sName[0].Length;
            //int ss = sName[1].Length;
            return (sName[0].Length <4)? sName[1].Trim(): sName[0].Trim();

        }

        public string GetNoDupName(string name)
        {

            string[] stringSeparators = new string[] { "lbw ", "b ", "c ", "st " };
            string[] sName = name.Split(stringSeparators, StringSplitOptions.None);

            return name.Contains("run out") ? name : sName[1].Trim();

        }

        public ArrayList GetRunOutpoints(string name)
        {
            ArrayList namely = new ArrayList(); 
            string[] slashSplit = new string[] { "/" };
            List<char> charsToRemove = new List<char>() { ')','(' };
            string[] stringSeparators = new string[] { "run out"};
            string[] sName = name.Split(stringSeparators, StringSplitOptions.None);
            string[] runName = sName[1].Split(slashSplit, StringSplitOptions.None);
            for (int i = 0; i < runName.Length; i++)
            {
                string n = runName[i];
                string str1 = Filter(n, charsToRemove);
                namely.Add(str1);

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
