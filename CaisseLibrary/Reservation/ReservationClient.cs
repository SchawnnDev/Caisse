using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
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
                .UseIp("localhost") // 176.31.206.53
				.UseTcp(5456)
                .UseUdp(5457)
                .ConfigureLogging(loggingBuilder =>
	            {
		          //     loggingBuilder.AddConfiguration(config.GetSection("Logging"));
                    loggingBuilder.AddConsole();
                })
                .UseProtobufNet()
                .Build();

        }

	    public void Connect()
	    {
		    Client.Connect();
		}

	    public void Disconnect()
	    {
			Client.Stop();
	    }

		public void SendPacket<T>(T packet)
	    {
			Client.SendUdp(packet);
	    }

	    public long Ping() => Client.Ping();

    }
}
