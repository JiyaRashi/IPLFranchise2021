using IPLFranchise2021.Event;
using IPLFranchise2021.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.Logic
{
    public interface IDataReaderLogic
    {
        IList<Batsman> GetAllBatsmen();
        IList<BowlSide> GetAllBowlSide();
        IList<IPLSchedule> GetAllIPLSchedule();
        Dictionary<string,string> GetAllFPLTeam(int matchNo);
        IList<SuperStars> GetFPLTeamStars(int matchNo);
    }
    public class DataReaderLogic : IDataReaderLogic
    {
        private int _matchNo;
        public IEventAggregator _eventAggregator { get; set; }

        public DataReaderLogic(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MatchNoEvent>().Subscribe(MatchNoReceived);
        }

        private void MatchNoReceived(int obj)
        {
            _matchNo = obj;
        }

        public int MatchNo
        {
            get { return _matchNo; }
            set { _matchNo = value; }
        }
        public IList<Batsman> GetAllBatsmen()
        {

            string path = $"Data/MatchPoints/{MatchNo}/BatScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBat();

            return query.ToList();
        }
        public IList<BowlSide> GetAllBowlSide()
        {
            string path = $"Data/MatchPoints/{MatchNo}/BowlScore.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToBowl();

            return query.ToList();
        }

        public IList<IPLSchedule> GetAllIPLSchedule()
        {
            string path = "Data/IPL2021Schedule.csv";
            var query =

                File.ReadAllLines(path)
                    .Skip(1)
                    .Where(l => l.Length > 1)
                    .ToSchedule();

            return query.ToList();
        }

        public Dictionary<string, string> GetAllFPLTeam(int matchNo)
        {
            string path = (matchNo >= 29) ? $"Data/FPLTeamList2sthalft.csv" : $"Data/FPLTeamList1sthalft.csv";
            StreamReader sr = new StreamReader(path);
            Dictionary<string, string> importingData = new Dictionary<string, string>();

            var source = File.ReadAllLines(path)
                  .Skip(1)
                  .Where(l => l.Length > 1);
            foreach (var line in source)
            {

                var columns = line.Split(',');

                importingData.Add(columns[0].Trim(), columns[1].Trim());

                //importingData.Add(new FPLTeamList
                //{
                //    PlayerName = columns[0],
                //    FPLTeam = columns[1],
                //});
            }

            return importingData;
        }

        public IList<SuperStars> GetFPLTeamStars(int matchNo)
        {
            string path = $"Data/MatchPoints/{matchNo}/SuperStars.csv";
            StreamReader sr = new StreamReader(path);
            Dictionary<string, string> importingData = new Dictionary<string, string>();
            var source = File.ReadAllLines(path)
                  .Skip(1)
                  .Where(l => l.Length > 1)
                  .ToSuperStars();
            //foreach (var line in source)
            //{
            //    var columns = line.Split(',');
            //    importingData.Add(columns[0].Trim(), columns[1].Trim());
            //}
            return source.ToList();
        }
    }
}
