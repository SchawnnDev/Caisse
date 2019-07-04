using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct TimeSlot : IData
	{
		[ProtoMember(1)] public SaveableTimeSlot SaveableTimeSlot;
		[ProtoMember(2)] public Cashier Cashier;
		public void From<T>(T parent, CaisseServerContext context)
		{
		}
	}
}
