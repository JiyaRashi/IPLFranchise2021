using IPLFranchise2021.Event;
using IPLFranchise2021.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IPLFranchise2021.ViewModels
{
   public class HomeViewModel :BindableBase
    {
        public DelegateCommand IPLScheduleDelegateCommand { get; private set; }

        public DelegateCommand IPLSchedule22DelegateCommand { get; private set; }

        public IEventAggregator _eventAggregator { get; set; }

        private IRegionManager _regionManger;
        public HomeViewModel(IRegionManager regionManger, IEventAggregator eventAggregator)
        {
            _regionManger = regionManger;
            _eventAggregator = eventAggregator;
            IPLScheduleDelegateCommand = new DelegateCommand(Execute, CanExecute);
            IPLSchedule22DelegateCommand = new DelegateCommand(Execute2022, CanExecute2022);

        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute()
        {

            _regionManger.RequestNavigate("MainRegion", "IPLScheduleView");
        }
        private bool CanExecute2022()
        {
            return true;
        }

        private void Execute2022()
        {
            string queryString = "2022";
            var navigationParams = new NavigationParameters(queryString);

            _regionManger.RequestNavigate("MainRegion", "IPLScheduleView", navigationParams);
        }
    }
}
