using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Networker.Common;
using Networker.Common.Abstractions;
using CaisseReservationLibrary.Packets;

namespace CaisseReservationLibrary.Handlers
{
	public class ArticleReservationPacketHandler : PacketHandlerBase<ArticleReservationPacket>
	{
		private readonly ILogger<ArticleReservationPacket> _logger;

		public ArticleReservationPacketHandler(ILogger<ArticleReservationPacket> logger)
		{
			_logger = logger;
		}

		public override async Task Process(ArticleReservationPacket packet, IPacketContext packetContext)
		{
			_logger.LogDebug($"Checkout id={packet.CheckoutId} reserved {packet.Number} of article={packet.ArticleId}");
			/*
			packetContext.Sender.Send(new ArticleReservationPacket
			{
				Message = "Hey, I got your message!"
			}); */
		}
	}
}