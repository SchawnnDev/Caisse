using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Events
{
	[ProtoContract]
	[Table("owners")]
	public class SaveableOwner
	{
		public SaveableOwner()
		{
		}

		public SaveableOwner(string login, string firstName, string name)
		{
			Login = login;
			FirstName = firstName;
			Name = name;
		}

		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public string Login { get; set; }

		[ProtoMember(2)] public string FirstName { get; set; }

		[ProtoMember(3)] public string Name { get; set; }

		[ProtoMember(4)] public DateTime LastLogin { get; set; }

		[ProtoMember(5)] public DateTime LastLogout { get; set; }

		[ProtoMember(6)] public SaveableEvent Event { get; set; }

		[ProtoMember(7)] public bool SuperAdmin { get; set; }

		public override string ToString() => Name;

        public override bool Equals(object obj) => obj is SaveableOwner && ((SaveableOwner)obj).Id == Id;
    }
}