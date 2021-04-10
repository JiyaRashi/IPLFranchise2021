using IPLFranchise2021.Logic;
using IPLFranchise2021.Model;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
    public class IPLScheduleViewModel :BindableBase
    {
        public DelegateCommand MatchScoreDelegateCommand { get; private set; }
        public IDataReaderLogic dataReaderLogic { get; set; }

        private IRegionManager _regionManger;
        public ObservableCollection<IPLSchedule> _iPLScheduleDetails { get; set; }
        public IPLScheduleViewModel(IRegionManager regionManger,
            IDataReaderLogic DataReaderLogic)
        {
            dataReaderLogic = DataReaderLogic;
            _iPLScheduleDetails = new ObservableCollection<IPLSchedule>((dataReaderLogic.GetAllIPLSchedule()));
            MatchScoreDelegateCommand = new DelegateCommand(Execute, CanExecute);
            _regionManger = regionManger;
        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute()
        {
            _regionManger.RequestNavigate("MainRegion", "ScoreDetailsView");
        }

        public ObservableCollection<IPLSchedule> IPLScheduleDetails
        {
            get { return _iPLScheduleDetails; }
            set { _iPLScheduleDetails = value; }
        }
    }
}
