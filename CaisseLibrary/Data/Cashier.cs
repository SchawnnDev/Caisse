using System.Collections.Generic;
using System.Linq;
using CaisseServer;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Cashier : IData
	{
		[ProtoMember(1)] public SaveableCashier SaveableCashier { get; set; }
		[ProtoMember(2)] public List<Invoice> Invoices { get; set; }
		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableCashier = parent as SaveableCashier;

			SaveableCashier = saveableCashier;
			Invoices = new List<Invoice>();

			foreach (var saveableInvoice in context.Invoices.Where(t => t.Cashier.Id == saveableCashier.Id).ToList())
			{
				var invoice = new Invoice();
				invoice.From(saveableInvoice, context);
				Invoices.Add(invoice);
			}
		}
	}

}