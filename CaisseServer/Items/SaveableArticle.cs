using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Items
{
	[ProtoContract]
    [Table("articles")]
    public class SaveableArticle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }

		[ProtoMember(2)] public string ImageSrc { get; set; }

		[ProtoMember(3)] public decimal Price { get; set; }

		[ProtoMember(4)] public int Position { get; set; }

		[ProtoMember(5)] public string Color { get; set; }

		[ProtoMember(6)] public bool NeedsCup { get; set; }

		[ProtoMember(7)] public bool Active { get; set; }

		[ProtoMember(8)] public bool NumberingTracking { get; set; }

		[ProtoMember(9)] public int MaxSellNumberPerDay { get; set; }

		[ProtoMember(10)] public SaveableCheckoutType Type { get; set; }

    }
}