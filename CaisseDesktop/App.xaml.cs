using System.Windows;
using CaisseDesktop.Frontend.Views;
using Splat;

namespace CaisseDesktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
	    protected override void OnStartup(StartupEventArgs e)
	    {
		    base.OnStartup(e);
		    
		    Locator.Current.GetService<StartupView>().ShowDialog();
		    Locator.Current.GetService<SelectionView>().Show();
	    }

	    public App()
	    {
			Bootstrapper = new AppBootstrapper();
	    }

	    public AppBootstrapper Bootstrapper { get; }
    }
}