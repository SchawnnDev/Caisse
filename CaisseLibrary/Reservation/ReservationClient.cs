using System;
using System.Linq;
using CaisseReservationLibrary.Handlers;
using Microsoft.Extensions.Logging;
using Networker.Client;
using Networker.Client.Abstractions;
using Networker.Extensions.ProtobufNet;

namespace CaisseLibrary.Reservation
{
    public class ReservationClient
    {

        private IClient Client;

        public ReservationClient()
        {
            /*
            var config = new ConfigurationBuilder()
                .AddJsonFile("clientSettings.json", false, true)
                .Build(); */

          // var networkerSettings = config.GetSection("Networker");

            Client = new ClientBuilder()
                .UseIp("192.168.178.71") // 176.31.206.53
				.UseTcp(5456)
                .UseUdp(5457)

                .ConfigureLogging(loggingBuilder =>
	            {
		          //     loggingBuilder.AddConfiguration(config.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                    loggingBuilder.SetMinimumLevel(
                        LogLevel.Debug);
                })
                .RegisterPacketHandler<string, MessagePacketHandler>()
                .UseProtobufNet()
                .Build();
        }

	    public void Connect()
        {
            if (true) return;
		    var test = Client.Connect();

            Console.WriteLine($"The connection {(test.Success ? "succeed": "failed")}");

            Console.WriteLine();
            if (test.Errors != null && test.Errors.Any())
            {
                Console.WriteLine("Errors:");
                Console.WriteLine();
                foreach (var error in test.Errors)
                {
                    Console.WriteLine(error);
                }
            }
            Console.WriteLine();
           // Console.WriteLine($"Trying to connect to the server: { Client.Ping(100)}");

           Client.Send("Hello!!!");
		}

	    public void Disconnect()
	    {
			Client.Stop();
	    }

		public void SendPacket<T>(T packet)
	    {
			Client.Send(packet);
	    }

	    public long Ping() => Client.Ping();

    }
}
