using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;

namespace CaisseReservationLibrary.Packets
{
	[ProtoContract]
	public class ConnectPacket
	{
		[ProtoMember(1)]
		public int CheckoutId;
		[ProtoMember(2)]
		public int ComputerId;
		[ProtoMember(3)]
		public byte[] Password;

	}
}
