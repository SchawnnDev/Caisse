using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseLibrary.Reservation;
using CaisseReservationLibrary.Packets;

namespace CaisseTest
{
	class Program
	{
		static void Main(string[] args)
		{
			var client = new ReservationClient();
			client.Connect();
//			Console.WriteLine(client.Ping());

			while (Console.ReadLine() != "exit")
			{
				client.SendPacket(new ArticleReservationPacket
				{
					ArticleId = 22,
					CheckoutId = 22,
					Number = 22
				});
			}

		}
	}
}
