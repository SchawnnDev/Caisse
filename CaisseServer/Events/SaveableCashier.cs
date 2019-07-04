using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
	[Table("cashiers")]
	public class SaveableCashier

	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public string Login { get; set; }

		[ProtoMember(2)] public string FirstName { get; set; }

		[ProtoMember(3)] public string Name { get; set; }

		[ProtoMember(4)] public bool WasHere { get; set; }

		[ProtoMember(5)] public bool Substitute { get; set; }

		[ProtoMember(6)] public SaveableCheckout Checkout { get; set; }

		[ProtoMember(7)] public DateTime LastActivity { get; set; }

		public string GetFullName()
		{
			return $"{FirstName} {Name}";
		}
	}
}