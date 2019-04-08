using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CaisseIO;
using CaisseIO.Exceptions;
using CaisseServer.Events;

namespace CaisseServer
{
    [Table("substitutes")]
    public class SaveableSubstitute : IImportable, IExportable

    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

        public string Name { get; set; }

	    public DateTime LastActivity { get; set; }

		public bool Active { get; set; }

		public string GetFullName()
        {
            return $"{FirstName} {Name}";
        }

        public void Import(object[] args)
        {
            if (args.Length != 5) throw new IllegalArgumentNumberException(5, "remplacant");
            if (!args[0].ToString().ToLower().Equals("remplacant"))
                throw new TypeNotRecognisedException("remplacant (Substitute)");

            Id = args[1] as int? ?? 0;
            Login = args[2] as string;
            FirstName = args[3] as string;
            Name = args[4] as string;
            LastActivity = args[5] as DateTime? ?? new DateTime();
            Active = args[6] is bool b && b;

        }

        public object[] Export() => new object[]
        {
			"Substitute",
            Id,
            Login,
            FirstName,
            Name,
            LastActivity,
            Active
        };
    }
}