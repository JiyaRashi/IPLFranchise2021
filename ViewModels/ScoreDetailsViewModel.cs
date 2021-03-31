using IPLFranchise2021.Model;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
    public class ScoreDetailsViewModel : BindableBase
    {
        public ObservableCollection<Batsman> _batsmenDetails { get; set; }

        public ObservableCollection<Batsman> _allBatsmenDetails { get; set; }

        public int _totalScore;

        public DelegateCommand CalculateScoreDelegateCommand { get; private set; }


        public ScoreDetailsViewModel()
        {
            CalculateScoreDelegateCommand = new DelegateCommand(Execute, CanExecute);
            _batsmenDetails = new ObservableCollection<Batsman>((GetAllBatsmen()));
            _allBatsmenDetails = new ObservableCollection<Batsman>();
        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute()
        {
            AllBatsmenDetails = BatsmanPointsTotalScore(BatsmenDetails);
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
        public ObservableCollection<Batsman> BatsmenDetails
        {
            get { return _batsmenDetails; }
            set { _batsmenDetails = value; }
        }

        public ObservableCollection<Batsman> AllBatsmenDetails
        {
            get { return _allBatsmenDetails; }
            set { _allBatsmenDetails = value; }
        }

        public IList<Batsman> GetAllBatsmen()
        {
            string path = "Data/sampleScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBat();

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

            return totalScore + runTotalPoints + sixesPoints+ fourPoints+ srPoints+ ducks;
        }

    }
}
