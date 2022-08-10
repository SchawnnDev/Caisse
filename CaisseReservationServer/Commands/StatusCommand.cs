using System;
using System.Collections.Generic;
using System.Text;
using CaisseReservationLibrary.Interfaces;
using Console = System.Console;

namespace CaisseReservationServer.Commands
{
    public class StatusCommand : ICommand
    {
        public string Name() => "status";

        public void Process(string[] args)
        {
            /*
            var info = Program.Server.Information;
            Console.WriteLine($"> Server is actually: {(info.IsRunning ? "" : "not ")}running");
            Console.WriteLine($"> Invalid TCP Packets: {info.InvalidTcpPackets}");
            Console.WriteLine($"> Processed TCP Packets: {info.ProcessedTcpPackets}");
            Console.WriteLine($"> Invalid UDP Packets: {info.InvalidUdpPackets}");
            Console.WriteLine($"> Processed UDP Packets: {info.ProcessedUdpPackets}");*/
        }

        public void PrintHelp()
        {
            Console.WriteLine("Get status of the server");
        }
    }
}
