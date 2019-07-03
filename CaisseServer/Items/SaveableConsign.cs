using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Export;

namespace CaisseServer.Items
{
	[Table("consigns")]
	public class SaveableConsign
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		public SaveableInvoice Invoice { get; set; }

		public int Amount { get; set; }

	}
}