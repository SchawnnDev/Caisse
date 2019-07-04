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
	public struct CheckoutDay : IData
	{
		[ProtoMember(1)] public List<TimeSlot> TimeSlots { get; set; }

		public void From<T>(T parent, CaisseServerContext context)
		{
			var saveableDay = parent as SaveableDay;

			TimeSlots = new List<TimeSlot>();

			foreach (var timeSlot in context.TimeSlots.Where(t => t.Day.Id == saveableDay.Id).ToList())
			{
				var type = new TimeSlot();
				type.From(timeSlot, context);
				TimeSlots.Add(type);
			}
		}
	}
}
