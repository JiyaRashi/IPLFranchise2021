using IPLFranchise2021.Event;
using IPLFranchise2021.Logic;
using IPLFranchise2021.Model;
using IPLFranchise2021.Repository;
using Prism.Commands;
using Prism.Events;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
    public class ScoreDetailsViewModel : BindableBase, INavigationAware
    {
        IRegionNavigationJournal _journal;
        public IEventAggregator _eventAggregator { get; set; }
        public IRunsCalculatorLogic runsCalculatorLogic { get; set; }
        public ILoadRepository loadRepository { get; set; }
        public IDataReaderLogic dataReaderLogic { get; set; }
        public ObservableCollection<Batsman> _batsmenDetails;
        public ObservableCollection<BowlSide> _bowlDetails;
        public ObservableCollection<Batsman> _batsmenTotalPoints = new ObservableCollection<Batsman>();
        public ObservableCollection<BowlSide> _bowlingTotalPoints = new ObservableCollection<BowlSide>();
        public ObservableCollection<OtherDetails> _fielderNameDupPoints = new ObservableCollection<OtherDetails>();
        public ObservableCollection<OtherDetails> _fielderBonousPoints = new ObservableCollection<OtherDetails>();
        public ObservableCollection<OtherDetails> _splittedName = new ObservableCollection<OtherDetails>();
        public ObservableCollection<OtherDetails> _fielderTotalPoints = new ObservableCollection<OtherDetails>();
        public ObservableCollection<OtherDetails> _nameCollectionTotalPoints = new ObservableCollection<OtherDetails>();

        public ObservableCollection<FPLTotalPoints> _fPLTotalPoints = new ObservableCollection<FPLTotalPoints>();
        public ObservableCollection<FPLTeamTotalPoints> _fPLteamTotalPoints = new ObservableCollection<FPLTeamTotalPoints>();

        public Dictionary<string,string> _fPLTeamLists = new Dictionary<string, string>();

        public DelegateCommand CalculateScoreDelegateCommand { get; private set; }

        public DelegateCommand FPLTeamDelegateCommand { get; private set; }

        
        public DelegateCommand GoBackDelegateCommand { get; set; }

        private int _matchNo;
        private IPLSchedule _selectedMatch;

        public ScoreDetailsViewModel(IRunsCalculatorLogic RunsCalculatorLogic, ILoadRepository LoadRepository, IEventAggregator eventAggregator,
            IDataReaderLogic DataReaderLogic)
        {
            dataReaderLogic = DataReaderLogic;
            runsCalculatorLogic = RunsCalculatorLogic;
            loadRepository = LoadRepository;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MatchNoEvent>().Subscribe(MatchNoReceived);
            CalculateScoreDelegateCommand = new DelegateCommand(Execute, CanExecute);
            GoBackDelegateCommand = new DelegateCommand(GoBack);
            FPLTeamDelegateCommand = new DelegateCommand(GetFPLTeamPoints);
            FPLTeamLists = dataReaderLogic.GetAllFPLTeam();
        }

        public int MatchNo
        {
            get { return _matchNo; }
            set { _matchNo = value; }
        }
        private void MatchNoReceived(int obj)
        {
            _matchNo = obj;
        }
        private bool CanExecute()
        {
            return true;
        }
        private void Execute()
        {
            BatsmenTotalPoints = BatsmanPointsTotalScore(BatsmenDetails);
            BowlingTotalPoints = BowlPointsTotalScore(BowlDetails);
            FielderNameDupPoints = FielderEachPoints(BatsmenDetails);
            FielderBonousPoints = FielderBonousTotalPoints(FielderNameDupPoints);
            FielderTotalPoints = GetFielderTotalPoints(FielderBonousPoints);
            NameCollectionTotalPoints = AddNameScore(BatsmenDetails, BatsmenTotalPoints, BowlDetails, BowlingTotalPoints, FielderTotalPoints);
            FPLTotal_Points = AddTeamNametoList(NameCollectionTotalPoints,FPLTeamLists);
        }

        /// <summary>
        /// Get Batsman Details
        /// </summary>
        public ObservableCollection<OtherDetails> NameCollectionTotalPoints
        {
            get { return _nameCollectionTotalPoints; }
            set
            {
                SetProperty(ref _nameCollectionTotalPoints, value);
                RaisePropertyChanged("NameCollectionTotalPoints");

            }
        }

        public ObservableCollection<FPLTotalPoints> FPLTotal_Points
        {
            get { return _fPLTotalPoints; }
            set
            {
                SetProperty(ref _fPLTotalPoints, value);
                RaisePropertyChanged("FPLTotal_Points");

            }
        }

        public ObservableCollection<FPLTeamTotalPoints> FPLteamTotalPoints
        {
            get { return _fPLteamTotalPoints; }
            set
            {
                SetProperty(ref _fPLteamTotalPoints, value);
                RaisePropertyChanged("FPLteamTotalPoints");

            }
        }

        public Dictionary<string, string> FPLTeamLists
        {
            get { return _fPLTeamLists; }
            set
            {
                SetProperty(ref _fPLTeamLists, value);

            }
        }

        public ObservableCollection<Batsman> BatsmenDetails
        {
            get { return _batsmenDetails; }
            set
            {
                SetProperty(ref _batsmenDetails, value);
                RaisePropertyChanged("BatsmenDetails");

            }
        }

        /// <summary>
        /// Get Bowling Details
        /// </summary>
        public ObservableCollection<BowlSide> BowlDetails
        {
            get { return _bowlDetails; }
            set
            {
                SetProperty(ref _bowlDetails, value);
                RaisePropertyChanged("BowlDetails");
            }
        }

        /// <summary>
        /// Batsman Total Score
        /// </summary>
        public ObservableCollection<Batsman> BatsmenTotalPoints
        {
            get { return _batsmenTotalPoints; }
            set
            {
                SetProperty(ref _batsmenTotalPoints, value);
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Bowling Total points
        /// </summary>
        public ObservableCollection<BowlSide> BowlingTotalPoints
        {
            get { return _bowlingTotalPoints; }
            set
            {
                SetProperty(ref _bowlingTotalPoints, value);
                RaisePropertyChanged();
            }
        }
        public ObservableCollection<OtherDetails> FielderNameDupPoints
        {
            get { return _fielderNameDupPoints; }
            set
            {
                SetProperty(ref _fielderNameDupPoints, value);
                RaisePropertyChanged("FielderNameDupPoints");
            }
        }
        public ObservableCollection<OtherDetails> SplittedName
        {
            get { return _splittedName; }
            set
            {
                SetProperty(ref _splittedName, value);
                RaisePropertyChanged("SplittedName");
            }
        }
        public ObservableCollection<OtherDetails> FielderBonousPoints
        {
            get { return _fielderBonousPoints; }
            set
            {
                SetProperty(ref _fielderBonousPoints, value);
                RaisePropertyChanged("FielderBonousPoints");

            }
        }

        public IPLSchedule SelectedMatch
        {
            get { return _selectedMatch; }
            set
            {
                SetProperty(ref _selectedMatch, value);
                RaisePropertyChanged("SelectedMatch");
            }
        }

        public ObservableCollection<OtherDetails> FielderTotalPoints
        {
            get { return _fielderTotalPoints; }
            set
            {
                SetProperty(ref _fielderTotalPoints, value);
                RaisePropertyChanged("FielderTotalPoints");
            }
        }

        /// <summary>
        /// Batsman Total Score
        /// </summary>
        /// <param name="_batsmenDetail"></param>
        /// <returns></returns>
        public ObservableCollection<Batsman> BatsmanPointsTotalScore(ObservableCollection<Batsman> _batsmenDetail)
        {
            if (_batsmenTotalPoints == null || _batsmenTotalPoints.Count == 0)
            {
                foreach (var item in _batsmenDetail)
                {

                    _batsmenTotalPoints.Add(

                        new Batsman
                        {
                            TotalScore = runsCalculatorLogic.BatsmanScoreCalulator
                            (item.Runs, item.Balls, item.Fours, item.Sixes, item.SR)
                        }); ;
                }

            }
            return _batsmenTotalPoints;
        }

        /// <summary>
        /// Bowling Total Points
        /// </summary>
        /// <param name="_bowlDetails"></param>
        /// <returns></returns>
        public ObservableCollection<BowlSide> BowlPointsTotalScore(ObservableCollection<BowlSide> _bowlDetails)
        {
            if (_bowlingTotalPoints == null || _bowlingTotalPoints.Count == 0)
            {
                foreach (var item in _bowlDetails)
                {

                    _bowlingTotalPoints.Add(

                        new BowlSide
                        {
                            BowlTotalScore = runsCalculatorLogic.BowlScoreCalulator(item.OverRuns, item.Overs, item.Wickets, item.Econ,
                            item.Dot, item.Maiden, item.HatTrick)
                        });
                }
            }
            return _bowlingTotalPoints;
        }

        public ObservableCollection<OtherDetails> FielderEachPoints(ObservableCollection<Batsman> batsmanName)
        {
            if (_fielderNameDupPoints == null || _fielderNameDupPoints.Count == 0)
            {
                foreach (var item in batsmanName)
                {
                    if (runsCalculatorLogic.IsValueableName(item.FielderDetails))
                    {
                        _fielderNameDupPoints.Add(
                        new OtherDetails()
                        {
                            Name = runsCalculatorLogic.GetName(item.FielderDetails),
                            OtherTotalScore = runsCalculatorLogic.FielderNamePoints(item.FielderDetails),
                        });
                    }

                }
            }
            return _fielderNameDupPoints;
        }

        public ObservableCollection<OtherDetails> FielderBonousTotalPoints(ObservableCollection<OtherDetails> filederNameDupPoints)
        {
            if (_fielderBonousPoints == null || _fielderBonousPoints.Count == 0)
            {
                var query = from r in filederNameDupPoints
                            group r by r.Name into g
                            select new { Count = g.Count(), Value = g.Key };

                foreach (var item in query)
                {
                    _fielderBonousPoints.Add(new OtherDetails()
                    {
                        Name = runsCalculatorLogic.GetName(item.Value),
                        OtherTotalScore = runsCalculatorLogic.FielderEachTotalBonousPoints(item.Value, item.Count)
                    });
                }

            }
            return _fielderBonousPoints;
        }

        public ObservableCollection<OtherDetails> GetFielderTotalPoints(ObservableCollection<OtherDetails> filederbonusPoints)
        {
            if (_fielderTotalPoints == null || _fielderTotalPoints.Count == 0)
            {
                foreach (var item in filederbonusPoints)
                {
                    if (item.Name.Contains("run out"))
                    {
                        ArrayList runoutName = runsCalculatorLogic.GetRunOutpoints(item.Name);

                        if (runoutName.Count == 1)
                        {
                            string n = (string)runoutName[0];
                            _splittedName.Add(new OtherDetails()
                            {
                                Name = n.Trim(),
                                OtherTotalScore = 50
                            });
                        }
                        else
                        {
                            for (int i = 0; i < runoutName.Count; i++)
                            {
                                string n = (string)runoutName[i];
                                _splittedName.Add(new OtherDetails()
                                {
                                    Name = runsCalculatorLogic.GetName(n),
                                    OtherTotalScore = (i == 0) ? 30 : (i == 1) ? 20 : 0
                                });
                            }
                        }

                    }
                    else
                    {
                        _splittedName.Add(new OtherDetails()
                        {
                            Name = runsCalculatorLogic.GetNoDupName(item.Name),
                            OtherTotalScore = item.OtherTotalScore
                        });
                    }
                }

                var result = from r in _splittedName
                             group r by r.Name into g
                             select new { Count = g.Sum(x => x.OtherTotalScore), Value = g.Key };

                foreach (var item in result)
                {
                    _fielderTotalPoints.Add(new OtherDetails()
                    {
                        Name = item.Value,
                        OtherTotalScore = item.Count
                    });
                }
            }
            return _fielderTotalPoints;
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            _batsmenTotalPoints = new ObservableCollection<Batsman>();
            _batsmenTotalPoints = new ObservableCollection<Batsman>();
            _bowlingTotalPoints = new ObservableCollection<BowlSide>();
            _fielderBonousPoints = new ObservableCollection<OtherDetails>();
            _fielderTotalPoints = new ObservableCollection<OtherDetails>();
            _splittedName = new ObservableCollection<OtherDetails>();
            _fielderNameDupPoints = new ObservableCollection<OtherDetails>();
            _bowlDetails = new ObservableCollection<BowlSide>();
            _batsmenDetails = new ObservableCollection<Batsman>();

            _nameCollectionTotalPoints = new ObservableCollection<OtherDetails>();
            _fPLTotalPoints = new ObservableCollection<FPLTotalPoints>();
            _fPLteamTotalPoints = new ObservableCollection<FPLTeamTotalPoints>();
            _journal = navigationContext.NavigationService.Journal;

            var _selectedMatch = navigationContext.Parameters["schedule"] as IPLSchedule;
            if (_selectedMatch != null)
                SelectedMatch = _selectedMatch;
            BatsmenDetails = GetAllBatsmen();
            BowlDetails = GetAllBowlSide();
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {

        }

        private void GoBack()
        {
            BatsmenTotalPoints = null;
            BowlingTotalPoints = null;
            FielderNameDupPoints = null;
            FielderBonousPoints = null;
            FielderTotalPoints = null;
            NameCollectionTotalPoints = null;
            FPLteamTotalPoints = null;
            FPLTotal_Points = null;
            // _journal.Clear();
            _journal.GoBack();
        }

        public ObservableCollection<OtherDetails> AddNameScore(ObservableCollection<Batsman> Batname, ObservableCollection<Batsman> BatTotalpoints,
            ObservableCollection<BowlSide> Bowlname, ObservableCollection<BowlSide> BowlTotalpoints, ObservableCollection<OtherDetails> FilederPoints)
        {
            ObservableCollection<OtherDetails> _nameTotalPoints = new ObservableCollection<OtherDetails>();
            if (_nameTotalPoints == null || _nameTotalPoints.Count == 0)
            {
                string[] slashSplit = new string[] { "\t" };
                for (int i = 0; i <= Batname.Count - 1; i++)
                {
                    string[] runName = Batname[i].BatsmanName.Split(slashSplit, StringSplitOptions.None);
                    _nameTotalPoints.Add(new OtherDetails() { Name = runName[0], OtherTotalScore = BatTotalpoints[i].TotalScore });

                }

                for (int i = 0; i <= Bowlname.Count - 1; i++)
                {
                    string[] runName = Bowlname[i].BowlerName.Split(slashSplit, StringSplitOptions.None);
                    _nameTotalPoints.Add(new OtherDetails() { Name = runName[0], OtherTotalScore = BowlTotalpoints[i].BowlTotalScore });

                }

                for (int i = 0; i <= FilederPoints.Count - 1; i++)
                {
                    string[] runName = FilederPoints[i].Name.Split(slashSplit, StringSplitOptions.None);
                    _nameTotalPoints.Add(new OtherDetails() { Name = runName[0], OtherTotalScore = FilederPoints[i].OtherTotalScore });

                }
            }
            var result = from r in _nameTotalPoints
                         group r by r.Name into g
                         select new { Count = g.Sum(x => x.OtherTotalScore), Value = g.Key };
            //string[] slashSplit = new string[] { "\t" };
            //for (int i = 0; i <= Batname.Count- 1; i++)
            //{
            //    string[] runName = Batname[i].BatsmanName.Split(slashSplit, StringSplitOptions.None);
            //    _nameScore.Add(runName[0], BatTotalpoints[i].TotalScore);
            //}

            //for (int i = 0; i <= Bowlname.Count - 1; i++)
            //{
            //    string[] runName = Bowlname[i].BowlerName.Split(slashSplit, StringSplitOptions.None);

            //    if (_nameScore.ContainsKey(runName[0]))
            //    {
            //        //int value
            //        int batvalue = _nameScore[runName[0]];
            //        int bowValue = BowlTotalpoints[i].BowlTotalScore;
            //        _nameScore[runName[0]] = batvalue + bowValue;
            //    }
            //    else
            //    {
            //        _nameScore.Add(runName[0], BowlTotalpoints[i].BowlTotalScore);
            //    }
            //}

            //for (int i = 0; i <= FilederPoints.Count - 1; i++)
            //{
            //    string vvv = FilederPoints[i].Name;

            //    if (_nameScore.ContainsKey(vvv))
            //    {
            //        int exitsVal = _nameScore[FilederPoints[i].Name];
            //        int currentValue = FilederPoints[i].OtherTotalScore;
            //        _nameScore[FilederPoints[i].Name] = exitsVal + currentValue;

            //    }
            //    else
            //    {
            //        _nameScore.Add(FilederPoints[i].Name, FilederPoints[i].OtherTotalScore);
            //    }
            //}

            foreach (var item in result)
            {
                _nameCollectionTotalPoints.Add(new OtherDetails() { Name = item.Value, OtherTotalScore = item.Count });
            }

            return _nameCollectionTotalPoints;
        }


        public ObservableCollection<FPLTotalPoints> AddTeamNametoList(ObservableCollection<OtherDetails> NameTotalPoints, Dictionary<string, string> FPLteamList)
        {
            string[] slashSplit = new string[] { "\t" };
            if (_fPLTotalPoints == null || _fPLTotalPoints.Count == 0)
            {
                for (int i = 0; i <= NameTotalPoints.Count - 1; i++)
                {
                    string name1 = NameTotalPoints[i].Name;
                    if (FPLteamList.ContainsKey(name1))
                    {
                        string fplTeam = FPLteamList[name1];
                        _fPLTotalPoints.Add(new FPLTotalPoints()
                        {
                            PlayerName = NameTotalPoints[i].Name,
                            Points = NameTotalPoints[i].OtherTotalScore,
                            FPLTeam = fplTeam
                        });

                    }
                }

            }
            return _fPLTotalPoints;

        }

        public void GetFPLTeamPoints()
        {

            FPLteamTotalPoints = GetTeamTotalPoints(FPLTotal_Points);
        }

        public ObservableCollection<FPLTeamTotalPoints> GetTeamTotalPoints(ObservableCollection<FPLTotalPoints> NameTotalPoints)
        {
            if (_fPLteamTotalPoints == null || _fPLteamTotalPoints.Count == 0)
            {
                ObservableCollection<FPLTotalPoints> _checkStatrMOM = new ObservableCollection<FPLTotalPoints>();
                foreach (var item in NameTotalPoints)
                {
                    int _pointsStar = (item.IsStar) ? item.Points * 2 : 0;
                    int _pointsMom = (item.IsMoM) ? item.Points + 100 : 0;

                    _checkStatrMOM.Add(new FPLTotalPoints()
                    {
                        PlayerName = item.PlayerName,
                        FPLTeam = item.FPLTeam,
                        IsStar = item.IsStar,
                        IsMoM = item.IsMoM,
                        Points = 
                        (item.IsMoM && item.IsStar) ? _pointsStar + 200 : 
                        (!item.IsMoM && !item.IsStar) ? item.Points :
                        (item.IsMoM) ? _pointsMom :
                        (item.IsStar) ? _pointsStar : item.Points                       
                    }); 
                }
                var result = from r in _checkStatrMOM
                             group r by r.FPLTeam into g
                             select new { Count = g.Sum(x => x.Points), Value = g.Key };

                foreach (var item in result)
                {
                    _fPLteamTotalPoints.Add( new FPLTeamTotalPoints() 
                    { 
                        FPLTeam = item.Value,
                        Points = item.Count 
                    });
                }
            }

            return _fPLteamTotalPoints;
        }

        public ObservableCollection<Batsman> GetAllBatsmen()
        {

            string path = $"Data/MatchPoints/{SelectedMatch.MatchNo}/BatScore.csv";
            StreamReader sr = new StreamReader(path);
            ObservableCollection<Batsman> importingData = new ObservableCollection<Batsman>();

            var source = File.ReadAllLines(path)
                  .Skip(1)
                  .Where(l => l.Length > 1);

            foreach (var line in source)
            {
                var columns = line.Split(',');
                importingData.Add(new Batsman
                {
                    BatsmanName = columns[0],
                    FielderDetails = columns[1].Trim(),
                    Runs = int.Parse(columns[2]),
                    Balls = int.Parse(columns[3]),
                    SR = double.Parse(columns[4]),
                    Fours = int.Parse(columns[5]),
                    Sixes = int.Parse(columns[6]),
                });
            }

            return importingData;
        }

        public ObservableCollection<BowlSide> GetAllBowlSide()
        {
            string path = $"Data/MatchPoints/{SelectedMatch.MatchNo}/BowlScore.csv";
            StreamReader sr = new StreamReader(path);
            ObservableCollection<BowlSide> importingData = new ObservableCollection<BowlSide>();

            var source = File.ReadAllLines(path)
                  .Skip(1)
                  .Where(l => l.Length > 1);
            foreach (var line in source)
            {

                var columns = line.Split(',');

                importingData.Add(new BowlSide
                {
                    BowlerName = columns[0],
                    Overs = double.Parse(columns[1]),
                    OverRuns = int.Parse(columns[2]),
                    Wickets = int.Parse(columns[3]),
                    Econ = double.Parse(columns[4]),
                    Dot = int.Parse(columns[5]),
                    Maiden = int.Parse(columns[6]),
                    HatTrick = Convert.ToBoolean(columns[7])
                });
            }

            return importingData;
        }

        
    }
}
