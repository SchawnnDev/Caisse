using System.Windows;

namespace CaisseDesktop
{
    /// <summary>
    ///     Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
	    private void App_OnExit(object sender, ExitEventArgs e)
	    {

		    if (CaisseLibrary.Main.TicketPrinter != null) // Release printer when closing app.
		    {
				CaisseLibrary.Main.TicketPrinter.Close();
		    }

	    }
    }
}