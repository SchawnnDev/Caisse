using System.Collections.Generic;
using CaisseServer;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
    public struct Checkout
	{
		[ProtoMember(1)] public SaveableCheckout SaveableCheckout { get; set; }
		[ProtoMember(2)] public List<Day> Days { get; set; }

	}
}