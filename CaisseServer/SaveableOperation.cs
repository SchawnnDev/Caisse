using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer
{
    [Table("operations")]
    public class SaveableOperation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableItem Item { get; set; }

        public int Amount { get; set; }

        public SaveableOperation()
        {
        }
    }
}