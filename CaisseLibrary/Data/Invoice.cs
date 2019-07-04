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
	public struct Invoice
	{
		[ProtoMember(1)] public SaveableInvoice SaveableInvoice { get; set; }
		[ProtoMember(2)] public List<SaveableOperation> Operations { get; set; }
		[ProtoMember(3)] public SaveableConsign Consign { get; set; }
	}
}
