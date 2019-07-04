using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CaisseServer.Events;
using ProtoBuf;

namespace CaisseLibrary.Data
{
	[ProtoContract]
	public struct Day
	{
		[ProtoMember(1)] public SaveableDay SaveableDay { get; set; }
		[ProtoMember(2)] public List<TimeSlot> TimeSlots { get; set; }
	}
}
