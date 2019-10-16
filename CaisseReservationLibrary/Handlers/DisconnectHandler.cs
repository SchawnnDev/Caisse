using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Events;
using CaisseReservationLibrary.Packets;
using Microsoft.Extensions.Logging;
using Networker.Common;
using Networker.Common.Abstractions;

namespace CaisseReservationLibrary.Handlers
{
	public class DisconnectHandler : PacketHandlerBase<DisconnectPacket>
	{
		private readonly ILogger<ConnectPacket> _logger;



		public DisconnectHandler(ILogger<ConnectPacket> logger)
		{
			_logger = logger;
		}

		public override async Task Process(DisconnectPacket packet, IPacketContext packetContext)
		{
		//	ServerEvents.OnDisconnect?.Invoke(packet, packetContext);
		}
	}
}