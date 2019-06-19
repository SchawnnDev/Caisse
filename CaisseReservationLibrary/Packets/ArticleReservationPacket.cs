using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CaisseReservationLibrary.Packets
{
	[ProtoContract] // Packet must be little as possible.
	public class ArticleReservationPacket
	{

		[ProtoMember(1)] public int ArticleId;
		[ProtoMember(2)] public int CheckoutId;
		[ProtoMember(3)] public int Number;

	}
}
