using IPLFranchise2021.Views;
using Prism.Commands;
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

        private IRegionManager _regionManger;
        public HomeViewModel(IRegionManager regionManger)
        {
            _regionManger = regionManger;
            IPLScheduleDelegateCommand = new DelegateCommand(Execute, CanExecute);
        }

        private bool CanExecute()
        {
            return true;
        }

        private void Execute()
        {
            _regionManger.RequestNavigate("MainRegion", "IPLScheduleView");
        }
    }
}
