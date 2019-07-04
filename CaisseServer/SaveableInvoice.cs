using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer
{
	[ProtoContract]
	[Table("invoices")]
	public class SaveableInvoice
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public DateTime Date { get; set; }

		[ProtoMember(2)] public decimal GivenMoney { get; set; }

		[ProtoMember(3)] public SaveableCashier Cashier { get; set; }

		[ProtoMember(4)] public SaveablePaymentMethod PaymentMethod { get; set; }
	}
}