using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ProtoBuf;

namespace CaisseServer.Events
{
	[ProtoContract]
	[Table("permissions")]
	public class SaveablePermission
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[ProtoMember(1)] public string Name { get; set; }
	}
}