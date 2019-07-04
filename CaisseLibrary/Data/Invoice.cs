using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Items;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Invoice : IData
	{
		[ProtoMember(1)] public SaveableInvoice SaveableInvoice { get; set; }
		[ProtoMember(2)] public List<SaveableOperation> Operations { get; set; }
		[ProtoMember(3)] public SaveableConsign Consign { get; set; }
		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableInvoice = parent as SaveableInvoice;

			SaveableInvoice = saveableInvoice;
			Operations = context.Operations.Where(t => t.Invoice.Id == saveableInvoice.Id).ToList();
			Consign = context.Consigns.SingleOrDefault(t=>t.Invoice.Id == saveableInvoice.Id);
		}
	}
}
