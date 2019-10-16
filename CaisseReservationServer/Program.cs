using System;
using System.Threading;
using CaisseReservationLibrary;
using CaisseReservationLibrary.Handlers;
using CaisseReservationLibrary.Packets;
using CaisseReservationServer.Commands;
using CaisseReservationServer.Handlers;
using Microsoft.Extensions.Logging;
using Networker.Extensions.ProtobufNet;
using Networker.Server;
using Networker.Server.Abstractions;

namespace CaisseReservationServer
{
    public class Program
    {

		private static ServerHandler ServerHandler;
        private static CommandHandler CommandHandler;

        static void Main(string[] args)
        {

            AppDomain.CurrentDomain.ProcessExit += CurrentDomain_ProcessExit;

            Console.WriteLine("Starting Caisse Reservation Server V1.0...");

            Thread.Sleep(1000);

			ServerHandler.Start();

            CommandHandler = new CommandHandler();

            CommandHandler.Register(new ClearCommand());
            CommandHandler.Register(new StatusCommand());
            CommandHandler.Register(new ExitCommand());

            new Thread(() =>
            {

                Thread.Sleep(2000);

                Start();

            }).Start();



        }

        private static void Start()
        {

            CommandHandler.Init();

            while (true)
            {
                //if (!Server.Information.IsRunning) break;

                CommandHandler.Handle(Console.ReadLine());

            }
        }

        static void CurrentDomain_ProcessExit(object sender, EventArgs e)
        {
            ServerHandler.Stop();
        }

    }
}