﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Events
{
	[ProtoContract]
	[Table("events")]
	public class SaveableEvent
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }

		[ProtoMember(2)] public DateTime Start { get; set; }

		[ProtoMember(3)] public DateTime End { get; set; }

		[ProtoMember(4)] public string AddressName { get; set; }

		[ProtoMember(5)] public string Address { get; set; }
		[ProtoMember(6)] public string AddressNumber { get; set; }

		[ProtoMember(7)] public string PostalCode { get; set; }
		[ProtoMember(8)] public string City { get; set; }

		[ProtoMember(9)] public string Description { get; set; }

		[ProtoMember(10)] public string ImageSrc { get; set; }

		[ProtoMember(11)] public string Telephone { get; set; }

		[ProtoMember(12)] public string Siret { get; set; }
	}
}