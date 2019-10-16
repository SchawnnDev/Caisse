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
    public class MessagePacketHandler : PacketHandlerBase<string>
    {
        private readonly ILogger<ArticleReservationPacket> _logger;
		
        public MessagePacketHandler(ILogger<ArticleReservationPacket> logger)
        {
            _logger = logger;
        }

        public override async Task Process(string packet, IPacketContext packetContext)
        {
            _logger.LogDebug(packet);
            //packetContext.Sender.Send();
            /*
			packetContext.Sender.Send(new ArticleReservationPacket
			{
				Message = "Hey, I got your message!"
			}); */
        }
    }
}