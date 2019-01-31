﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Events;

namespace CaisseServer.Items
{
    [Table("item_max_sell_numbers")]
    public class SaveableItemMaxSellNumber
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public SaveableItem Item { get; set; }

        public int Amount { get; set; }

    }

}