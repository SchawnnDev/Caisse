using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Threading;
using CaisseDesktop.Graphics.Utils;
using CaisseDesktop.Utils;
using CaisseServer;

namespace CaisseDesktop.Models.Windows
{
    public class StatisticsMainModel
    {

	    private ICommand _articleIncrementCommand;

	    public ICommand ArticleIncrementCommand => _articleIncrementCommand ??
	                                               (_articleIncrementCommand =
		                                               new CommandHandler(ClearInvoices, true));


	    public void ClearInvoices(object param)
	    {
		    if (!Validations.Ask("Etes-vous sûr de vouloir remettre à zero les factures ?"))
			    return;

		    Task.Run(()=>ClearInvoicesDB());


	    }

	    private void ClearInvoicesDB()
	    {

		    Dispatcher.Invoke(() =>
		    {
			    Mouse.OverrideCursor = Cursors.Wait;
			    CanLogin = false;
			    PrinterStatusLabel.Content = "Imprimante : Configuration de la connexion...";
		    });


			using (var db = new CaisseServerContext())
		    {



		    }
		}

	}
}
