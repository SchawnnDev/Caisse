using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Events
{
	[ProtoContract]
	[Table("time_slots")]
	public class SaveableTimeSlot
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public SaveableDay Day { get; set; }

		[ProtoMember(2)] public SaveableCheckout Checkout { get; set; }

		[ProtoMember(3)] public DateTime Start { get; set; }

		[ProtoMember(4)] public DateTime End { get; set; }

		[ProtoMember(5)] public SaveableCashier Cashier { get; set; }

		[ProtoMember(6)] public SaveableCashier Substitute { get; set; }

		[ProtoMember(7)] public bool SubstituteActive { get; set; }

		[ProtoMember(8)] public bool Pause { get; set; }

		[NotMapped] public bool Blank { get; set; }
	}
}