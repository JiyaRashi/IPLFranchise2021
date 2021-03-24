using IPLFranchise2021.Views;
using Prism.Ioc;
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
            return Container.Resolve<ShellWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }
    }
}
