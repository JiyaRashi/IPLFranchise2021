using IPLFranchise2021.Logic;
using IPLFranchise2021.Repository;
using IPLFranchise2021.ViewModels;
using IPLFranchise2021.Views;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Unity;
using System.Windows;

namespace IPLFranchise2021
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<ShellView>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
           
            containerRegistry.RegisterForNavigation<HomeView, HomeViewModel>();
            containerRegistry.RegisterForNavigation<ScoreDetailsView, ScoreDetailsViewModel>();
            containerRegistry.RegisterForNavigation<IPLScheduleView, IPLScheduleViewModel>();
            containerRegistry.Register<IRunsCalculatorLogic, RunsCalculatorLogic>();
            containerRegistry.Register<IDataReaderLogic, DataReaderLogic>();
            containerRegistry.Register<ILoadRepository, LoadRepository>();

        }

        protected override void ConfigureViewModelLocator()
        {
            base.ConfigureViewModelLocator();
            // type / type
            //ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), typeof(CustomViewModel));

            // type / factory
            //ViewModelLocationProvider.Register(typeof(MainWindow).ToString(), () => Container.Resolve<CustomViewModel>());

            // generic factory
            //ViewModelLocationProvider.Register<MainWindow>(() => Container.Resolve<CustomViewModel>());

            // generic type
            ViewModelLocationProvider.Register<ShellView, ShellViewModel>();
        }

    }
}
