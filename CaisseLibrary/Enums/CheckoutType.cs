using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.Enums
{
	public enum CheckoutType
	{
		[Description("Billets d'entrée")]
		Tickets,
		[Description("Tickets nourriture")]
		Food,
		[Description("Retour consignes")]
		Consign

	}
}
