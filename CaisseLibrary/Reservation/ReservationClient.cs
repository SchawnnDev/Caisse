using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Networker.Client;
using Networker.Client.Abstractions;

namespace CaisseLibrary.Reservation
{
    public class ReservationClient
    {

        public IClient Client;

        public ReservationClient()
        {
            /*
            var config = new ConfigurationBuilder()
                .AddJsonFile("clientSettings.json", false, true)
                .Build(); */

          // var networkerSettings = config.GetSection("Networker");

            Client = new ClientBuilder()
                .UseIp("hg.schawnndev.fr")
                .UseTcp(5456)
                .UseUdp(5457)
                .ConfigureLogging(loggingBuilder =>
                {
                  //.  loggingBuilder.AddConfiguration(config.GetSection("Logging"));
               //     loggingBuilder.AddConsole();
                })
              //  .UseProtobufNet()
                .Build();

        }


    }
}
