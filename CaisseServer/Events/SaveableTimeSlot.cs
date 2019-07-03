﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Events
{
    [Table("time_slots")]
    public class SaveableTimeSlot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public SaveableDay Day { get; set; }

        public SaveableCheckout Checkout { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public SaveableCashier Cashier { get; set; }

        public SaveableCashier Substitute { get; set; }

        public bool SubstituteActive { get; set; }

        public bool Pause { get; set; }

        [NotMapped] public bool Blank { get; set; }

    }
}