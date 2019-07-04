using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;
using ProtoBuf;

namespace CaisseServer.Events
{
	[ProtoContract]
    [Table("days")]
    public class SaveableDay
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ProtoMember(1)] public SaveableEvent Event { get; set; }

		[ProtoMember(2)] public DateTime Start { get; set; }

		[ProtoMember(3)] public DateTime End { get; set; }

		[ProtoMember(4)] public string Color { get; set; }

    }
}