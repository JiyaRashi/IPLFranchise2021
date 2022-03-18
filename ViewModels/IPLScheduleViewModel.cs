using IPLFranchise2021.Event;
using IPLFranchise2021.Logic;
using IPLFranchise2021.Model;
using Prism.Commands;
using Prism.Events;
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
    public class IPLScheduleViewModel :BindableBase, INavigationAware
    {

        public void OnNavigatedFrom(NavigationContext parameters)
        {
            //int id = navigationContext.Parameters["ID"];
        }
        public IEventAggregator _eventAggregator { get; set; }

        public DelegateCommand<IPLSchedule> MatchScoreDelegateCommand { get; private set; }

        private IPLSchedule _iPLSchedule;

        int yearvalue;
        public IDataReaderLogic dataReaderLogic { get; set; }

        private IRegionManager _regionManger;
        public ObservableCollection<IPLSchedule> _iPLScheduleDetails;
        public IPLScheduleViewModel(IRegionManager regionManger, IEventAggregator eventAggregator,
            IDataReaderLogic DataReaderLogic)
        {
            dataReaderLogic = DataReaderLogic;
            _eventAggregator = eventAggregator;
            ValSubscribe();
            MatchScoreDelegateCommand = new DelegateCommand<IPLSchedule>(Execute);
            _regionManger = regionManger;
        }

        public void ValSubscribe()
        {
            _eventAggregator.GetEvent<IPLYearNoEvent>().Subscribe(YearValue,ThreadOption.UIThread);
            _iPLScheduleDetails = new ObservableCollection<IPLSchedule>((dataReaderLogic.GetAllIPLSchedule(yearvalue)));


        }

        private void YearValue(int obj)
        {
            yearvalue = obj;
        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute(IPLSchedule schedule)
        {
            _eventAggregator.GetEvent<MatchNoEvent>().Publish(schedule.MatchNo);

            var parameters = new NavigationParameters();
            parameters.Add("schedule", schedule);

            if (schedule != null)
                _regionManger.RequestNavigate("MainRegion", "ScoreDetailsView", parameters);
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {

        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public ObservableCollection<IPLSchedule> IPLScheduleDetails
        {
            get { return _iPLScheduleDetails; }
            set { SetProperty(ref _iPLScheduleDetails, value); }
        }

        public IPLSchedule IPLSchedule
        {
            get { return _iPLSchedule; }
            set { SetProperty(ref _iPLSchedule, value); }
        }
    }
}
