using System.Collections.Generic;
using CaisseServer;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
    public struct Cashier
	{
		[ProtoMember(1)] public SaveableCashier SaveableCashier { get; set; }
		[ProtoMember(2)] public List<Invoice> Invoices { get; set; }
	}

}