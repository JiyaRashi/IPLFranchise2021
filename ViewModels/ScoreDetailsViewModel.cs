using IPLFranchise2021.Logic;
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
        public IRunsCalculatorLogic runsCalculatorLogic { get; set; }

        public IDataReaderLogic dataReaderLogic { get; set; }

        public ObservableCollection<Batsman> _batsmenDetails { get; set; }
        public ObservableCollection<BowlSide> _bowlDetails { get; set; }
        public ObservableCollection<Batsman> _batsmenTotalPoints { get; set; }
        public ObservableCollection<BowlSide> _bowlingTotalPoints { get; set; }
        public ObservableCollection<OtherDetails> _batsmanOutDetails { get; set; }
        public ObservableCollection<OtherDetails> _fielderNameDupPoints { get; set; }
        public ObservableCollection<OtherDetails> _fielderPoints { get; set; }

        public string[] stringSeparators = new string[] { "c ", "b ", "run out ", "c sub ", "lbw " };
        public DelegateCommand CalculateScoreDelegateCommand { get; private set; }
        public ScoreDetailsViewModel(IRunsCalculatorLogic RunsCalculatorLogic,
            IDataReaderLogic DataReaderLogic)
        {
            runsCalculatorLogic = RunsCalculatorLogic;
            dataReaderLogic = DataReaderLogic;
            CalculateScoreDelegateCommand = new DelegateCommand(Execute, CanExecute);
            _batsmenDetails = new ObservableCollection<Batsman>((dataReaderLogic.GetAllBatsmen()));
            _bowlDetails = new ObservableCollection<BowlSide>((dataReaderLogic.GetAllBowlSide()));
            _batsmenTotalPoints = new ObservableCollection<Batsman>();
            _bowlingTotalPoints = new ObservableCollection<BowlSide>();
            _fielderNameDupPoints = new ObservableCollection<OtherDetails>();
            _batsmanOutDetails = new ObservableCollection<OtherDetails>();
            _fielderPoints = new ObservableCollection<OtherDetails>();
            

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
            FielderPoints = FielderEachTotalPoints(FielderNameDupPoints);
        }

        /// <summary>
        /// Get Batsman Details
        /// </summary>
        public ObservableCollection<Batsman> BatsmenDetails
        {
            get { return _batsmenDetails; }
            set { _batsmenDetails = value; }
        }

        /// <summary>
        /// Get Bowling Details
        /// </summary>
        public ObservableCollection<BowlSide> BowlDetails
        {
            get { return _bowlDetails; }
            set { _bowlDetails = value; }
        }
        
        /// <summary>
        /// Batsman Total Score
        /// </summary>
        public ObservableCollection<Batsman> BatsmenTotalPoints
        {
            get { return _batsmenTotalPoints; }
            set { _batsmenTotalPoints = value; }
        }

        /// <summary>
        /// Bowling Total points
        /// </summary>
        public ObservableCollection<BowlSide> BowlingTotalPoints
        {
            get { return _bowlingTotalPoints; }
            set { _bowlingTotalPoints = value; }
        }
        public ObservableCollection<OtherDetails> FielderNameDupPoints
        {
            get { return _fielderNameDupPoints; }
            set { _fielderNameDupPoints = value; }
        }
        public ObservableCollection<OtherDetails> BatsmanOutDetails
        {
            get { return _batsmanOutDetails; }
            set { _batsmanOutDetails = value; }
        }
        public ObservableCollection<OtherDetails> FielderPoints
        {
            get { return _fielderPoints; }
            set { _fielderPoints = value; }
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
                            Name = item.FielderDetails.Trim(),
                            OtherTotalScore = runsCalculatorLogic.FielderNamePoints(item.FielderDetails),
                        });
                    }

                }
            }
            return _fielderNameDupPoints;
        }

        public ObservableCollection<OtherDetails> FielderEachTotalPoints(ObservableCollection<OtherDetails> filederNmaeDupPoints)
        {
            if (_fielderPoints == null || _fielderPoints.Count == 0)
            {
                var query = from r in filederNmaeDupPoints
                            group r by r.Name into g
                            select new { Count = g.Count(), Value = g.Key };

                foreach (var item in query)
                {
                    _fielderPoints.Add(new OtherDetails()
                    {
                        Name = item.Value,
                        OtherTotalScore = runsCalculatorLogic.FielderEachTotalPoints(item.Value,item.Count)
                    });
                }

            }
            
            return _fielderPoints;
        }
        
    }
}
