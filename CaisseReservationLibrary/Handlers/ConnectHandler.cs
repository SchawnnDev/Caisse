using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseReservationLibrary.Packets;
using Microsoft.Extensions.Logging;
using Networker.Common;
using Networker.Common.Abstractions;

namespace CaisseReservationLibrary.Handlers
{
	public class ConnectHandler : PacketHandlerBase<ConnectPacket>
	{
		private readonly ILogger<ConnectPacket> _logger;

		public delegate void ConnectEvent(ConnectPacket packet, IPacketContext context);
		public event ConnectEvent OnConnect; 
		public ConnectHandler(ILogger<ConnectPacket> logger)
		{
			_logger = logger;
		}

		public override async Task Process(ConnectPacket packet, IPacketContext packetContext)
		{
			OnConnect?.Invoke(packet, packetContext);
		}
	}
}