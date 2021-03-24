using IPLFranchise2021.Views;
using Prism.Mvvm;
using Prism.Regions;


namespace IPLFranchise2021.ViewModels
{
    public class ShellViewModel :BindableBase
    {
        private IRegionManager _regionManger;
        public ShellViewModel(IRegionManager regionManger)
        {
            _regionManger = regionManger;
            _regionManger.RegisterViewWithRegion("MainRegion", typeof(HomeView));
        }
    }
}
