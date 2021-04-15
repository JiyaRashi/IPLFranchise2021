using IPLFranchise2021.Logic;
using IPLFranchise2021.Model;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Mvvm;
using IPLFranchise2021.Event;

namespace IPLFranchise2021.Repository
{
    public interface ILoadRepository
    {
        IList<Batsman> GetAllBatsmanResp();
    }
    public class LoadRepository : ILoadRepository
    {
        public IDataReaderLogic dataReaderLogic ;

        private int _matchNo;
        
        public IEventAggregator _eventAggregator { get; set; }
        public LoadRepository(IDataReaderLogic DataReaderLogic, IEventAggregator eventAggregator)
        {
            dataReaderLogic = DataReaderLogic;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MatchNoEvent>().Subscribe(MatchNoReceived);
        }

        private void MatchNoReceived(int obj)
        {
            MatchNo = obj;
        }

        public int MatchNo
        {
            get { return _matchNo; }
            set { _matchNo = value; }
        }
        public IList<Batsman> GetAllBatsmanResp()
        {
            return dataReaderLogic.GetAllBatsmen();
        }
    }
}
