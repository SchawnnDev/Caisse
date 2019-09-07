using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CaisseDesktop.Lang;

namespace CaisseDesktop.Exceptions
{
    public class CaisseException : Exception
    {

	    public CaisseException(string message) : base(message)
	    {
		    MessageBox.Show(string.Format(French.CaisseException_ErrorOccured, message), French.CaisseException_Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
	    }

    }
}
