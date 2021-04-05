using IPLFranchise2021.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
    public class ScoreDetailsViewModel : BindableBase
    {
        public ObservableCollection<BowlSide> _bowlDetails { get; set; }
        public ObservableCollection<BowlSide> _allBowlDetails { get; set; }
        public ObservableCollection<Batsman> _allBatsmenDetails { get; set; }
        public ObservableCollection<Batsman> _batsmenDetails { get; set; }
        public ObservableCollection<OtherDetails> _otherPointsDetails { get; set; }
        public ObservableCollection<OtherDetails> _allotherPointsDetails { get; set; }
        public ObservableCollection<OtherDetails> noDuplicate { get; set; }

        public string[] stringSeparators = new string[] { "c ", "b ", "run out ", "c sub", "lbw " };
        public Dictionary<string, int> totalDuplicate { get; set; }

        public int _totalScore;

        public int _bowlTotalScore;
        public DelegateCommand CalculateScoreDelegateCommand { get; private set; }
        public ScoreDetailsViewModel()
        {
            CalculateScoreDelegateCommand = new DelegateCommand(Execute, CanExecute);
            _batsmenDetails = new ObservableCollection<Batsman>((GetAllBatsmen()));
            _bowlDetails = new ObservableCollection<BowlSide>((GetAllBowlSide()));
            _allBatsmenDetails = new ObservableCollection<Batsman>();
            _allBowlDetails = new ObservableCollection<BowlSide>();
            _otherPointsDetails = new ObservableCollection<OtherDetails>();
            _allotherPointsDetails = new ObservableCollection<OtherDetails>();
            noDuplicate = new ObservableCollection<OtherDetails>();
            totalDuplicate = new Dictionary<string, int>();

        }
        private bool CanExecute()
        {
            return true;
        }
        private void Execute()
        {
            AllBatsmenDetails = BatsmanPointsTotalScore(BatsmenDetails);
            AllBowlDetails = BowlPointsTotalScore(BowlDetails);
            AllotherPointsDetails = OtherPoints(OtherPointsDetails);
            NoDuplicate = NoDuplicateName(OtherPointsDetails);
            TotalDuplicate = TotalDuplicateName(OtherPointsDetails);
        }



        public int TotalScore
        {
            get { return _totalScore; }
            set
            {
                _totalScore = value;
                RaisePropertyChanged();

            }
        }
        public int BowlTotalScore
        {
            get { return _bowlTotalScore; }
            set
            {
                _bowlTotalScore = value;
                RaisePropertyChanged();

            }
        }
        public ObservableCollection<Batsman> BatsmenDetails
        {
            get { return _batsmenDetails; }
            set { _batsmenDetails = value; }
        }

        public ObservableCollection<BowlSide> BowlDetails
        {
            get { return _bowlDetails; }
            set { _bowlDetails = value; }
        }

        public ObservableCollection<Batsman> AllBatsmenDetails
        {
            get { return _allBatsmenDetails; }
            set { _allBatsmenDetails = value; }
        }

        public ObservableCollection<BowlSide> AllBowlDetails
        {
            get { return _allBowlDetails; }
            set { _allBowlDetails = value; }
        }

        public ObservableCollection<OtherDetails> OtherPointsDetails
        {
            get { return _otherPointsDetails; }
            set { _otherPointsDetails = value; }
        }
        public ObservableCollection<OtherDetails> AllotherPointsDetails
        {
            get { return _allotherPointsDetails; }
            set { _allotherPointsDetails = value; }
        }

        public ObservableCollection<OtherDetails> NoDuplicate
        {
            get { return noDuplicate; }
            set { noDuplicate = value; }
        }

        public Dictionary<string, int> TotalDuplicate
        {
            get { return totalDuplicate; }
            set { totalDuplicate = value; }
        }
        public IList<Batsman> GetAllBatsmen()
        {
            string path = "Data/sampleBatScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBat();

            return query.ToList();
        }
        public IList<BowlSide> GetAllBowlSide()
        {
            string path = "Data/sampleBowlScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBowl();

            return query.ToList();
        }
        public ObservableCollection<Batsman> BatsmanPointsTotalScore(ObservableCollection<Batsman> _batsmenDetail)
        {
            if (_allBatsmenDetails == null || _allBatsmenDetails.Count == 0)
            {
                foreach (var item in _batsmenDetail)
                {

                    _allBatsmenDetails.Add(

                        new Batsman
                        {
                            TotalScore = BatsmanScoreCalulator(item.Runs, item.Balls, item.Fours, item.Sixes, item.SR, item.Details)
                        });
                }
            }
            return _allBatsmenDetails;
        }
        public int BatsmanScoreCalulator(int runs, int balls, int fours, int sixes, double SR, string details)
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
            string[] stringSeparators = new string[] { " & ", " b " };
            string[] otherDetails = details.Split(stringSeparators, StringSplitOptions.None);
            foreach (string author in otherDetails)
            {
                Regex r = new Regex("lbw |b |c |st| & |run out");
                bool containsAny = r.IsMatch(author);
                if ((author.ToUpper() != "\tNOT OUT\t" && containsAny))
                {

                    OtherPointsDetails.Add(new OtherDetails() { Name = author.Trim() });
                }
            }
            return totalScore + runTotalPoints + sixesPoints + fourPoints + srPoints + ducks;
        }
        public ObservableCollection<BowlSide> BowlPointsTotalScore(ObservableCollection<BowlSide> _bowlDetails)
        {
            if (_allBowlDetails == null || _allBowlDetails.Count == 0)
            {
                foreach (var item in _bowlDetails)
                {

                    _allBowlDetails.Add(

                        new BowlSide
                        {
                            BowlTotalScore = BowlScoreCalulator(item.OverRuns, item.Overs, item.Wickets, item.Econ,
                            item.Dot, item.Maiden, item.HatTrick)
                        });
                }
            }
            return _allBowlDetails;
        }
        public int BowlScoreCalulator(int overRuns, double overs, int wickets, double Econ,
            int dot, int maiden, bool hatTrick)
        {
            int _bowlTotalPoints = 0;

            int wicketPoints = (wickets == 1) ? 30 :
                (wickets > 1 && wickets <= 2) ? 60 :
                (wickets > 2 && wickets <= 3) ? 100 :
                (wickets > 3 && wickets <= 4) ? 150 :
                (wickets > 4 && wickets <= 5) ? 250 : 0;
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
        public ObservableCollection<OtherDetails> OtherPoints(ObservableCollection<OtherDetails> otherDetails)
        {
            if (_allotherPointsDetails == null || _allotherPointsDetails.Count == 0)
            {
                string[] stringSeparators = new string[] { " "};

                foreach (var item in otherDetails)
                {
                    string[] name1 = item.Name.Split(stringSeparators, StringSplitOptions.None);
                    //Regex r = new Regex("c sub|c");

                    bool catcher = name1[0].Contains("c");
                    bool LBW = name1[0].Contains("lbw");
                    bool bowled = name1[0].Contains("b");
                    bool stumbed = name1[0].Contains("st");
                    bool runout = name1[0].Contains("run");

                    int points = LBW ? 10 :
                        bowled ? 10 :
                        catcher ? 25 :
                        stumbed ? 30 :
                        runout ? 50 :0;

                    _allotherPointsDetails.Add(
                    new OtherDetails()
                    {
                        Name = item.Name,
                        OtherTotalScore = points
                    });

                }

               
                

            }
            return _allotherPointsDetails;
        }

        public ObservableCollection<OtherDetails> NoDuplicateName(ObservableCollection<OtherDetails> otherPointsDetails)
        {
            if (noDuplicate == null || noDuplicate.Count == 0)
            {
                var query = from r in otherPointsDetails
                            group r by r.Name into g
                            select new { Count = g.Count(), Value = g.Key };


                foreach (var item in query)
                {
                    bool catcher = item.Value.Contains("c ");
                    bool catchersub = item.Value.Contains("c sub (");
                    bool LBW = item.Value.Contains("lbw ");
                    bool bowled = item.Value.Contains("b ");
                    bool stumbed = item.Value.Contains("st ");
                    bool runout = item.Value.Contains("run out ");
                    bool runoutsub = item.Value.Contains("run out (sub ");

                int points = LBW ? 10 :
                        bowled ? 10 :
                        catcher ? 25 :
                        catchersub ? 25 :
                        stumbed ? 30 :
                        runout ? 50 :
                        runoutsub?50: 0;

                    if (catcher || catchersub || stumbed|| runoutsub)
                    {
                        points = (item.Count >= 3) ? item.Count * 25 + 70 : points * item.Count;
                    }
                    else
                    {
                        points = points * item.Count;
                    }


                    noDuplicate.Add(new OtherDetails()
                    {
                        Name = item.Value,
                        OtherTotalScore = points
                    });
                }

           }

            //foreach (var v in query)
            //{
            //    noDuplicate.Add(new OtherDetails()
            //    {
            //        Name = v.Value,
            //        OtherTotalScore = v.Count
            //    });
            //}
            return noDuplicate;
        }

        public Dictionary<string, int> TotalDuplicateName(ObservableCollection<OtherDetails> otherPointsDetails)
        {
            //if (totalDuplicate == null || totalDuplicate.Count == 0)
            //{
            //    foreach (var item in otherPointsDetails)
            //    {
            //        string[] name2 = item.Name.Split(stringSeparators, StringSplitOptions.None);
            //        string rawName = name2[1];

            //        if (totalDuplicate.Count>0 && totalDuplicate.ContainsKey(rawName))
            //        {
            //            int isDuplicate = totalDuplicate[name2[1]];
            //            totalDuplicate[name2[1]]=isDuplicate * 2;

            //        }
            //        //string totalDuplicatekey = totalDuplicate;
            //        if (!totalDuplicate.ContainsKey(rawName))
            //        {
            //            totalDuplicate.Add
            //                (
            //                  name2[1], item.OtherTotalScore
            //                );
            //        }
            //    }

            //}

            if (totalDuplicate == null || totalDuplicate.Count == 0)
            {
                foreach (var item in otherPointsDetails)
                {
                }
            }


            return totalDuplicate;
        }

            bool duplicateFinder(ObservableCollection<OtherDetails> duplicate, string name)
        {
           // string[] stringSeparators = new string[] { "c ", "b ", "run out ", "c sub", "lbw " };


            foreach (var item in duplicate)
            {
                string[] name1 = name.Split(stringSeparators, StringSplitOptions.None);
                string[] name2 = item.Name.Split(stringSeparators, StringSplitOptions.None);
                if (name1[1].ToString().Equals(name2[1]) && duplicate.Count > 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
