using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseServer
{
    [Table("payment_methods")]
    public class SaveablePaymentMethod
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public double MinFee { get; set; }

        public string AcceptedDetails { get; set; }

        public SaveablePaymentMethod()
        {

        }

    }
}
