using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CaisseIO;
using CaisseIO.Exceptions;

namespace CaisseServer.Events
{
    [Table("owners")]
    public class SaveableOwner : IImportable, IExportable
    {
        public SaveableOwner()
        {
        }

        public SaveableOwner(string login, string name, string permissions)
        {
            Login = login;
            Name = name;
            Permissions = permissions;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string Name { get; set; }

        public string Permissions { get; set; }

        public DateTime LastLogin { get; set; }

        public DateTime LastLogout { get; set; }

        public SaveableEvent Event { get; set; }

        public bool SuperAdmin { get; set; }

        public string[] GetPermissions()
        {
            return string.IsNullOrWhiteSpace(Permissions) || Permissions.Equals("*")
                ? new string[] { }
                : Permissions.Split(',');
        }

        public bool HasPermission(string permission)
        {
            return SuperAdmin || Permissions.Equals("*") || GetPermissions().Contains(permission);
        }

        public override string ToString()
        {
            return Name;
        }

        public void Import(object[] args)
        {
            if (args.Length != 9) throw new IllegalArgumentNumberException(9, "résponsable");
            if (!args[0].ToString().ToLower().Equals("owner"))
                throw new TypeNotRecognisedException("résponsable (Owner)");

            Id = args[1] as int? ?? 0;
            Login = args[2] as string;
            Name = args[3] as string;
            Permissions = args[4] as string;
            LastLogin = args[5] is DateTime time ? time : new DateTime();
            LastLogout = args[6] is DateTime dateTime ? dateTime : new DateTime();
            SuperAdmin = args[8] is bool b && b;

            if (args[7] is SaveableEvent saveableEvent)
            {
                Event = saveableEvent;
            }
            else
            {
                Event = new SaveableEvent();
                Event.Import(args[7] as object[]);
            }
        }

        public object[] Export() => new object[]
        {
            "Owner",
            Id,
            Login,
            Name,
            Permissions,
            LastLogin,
            LastLogout,
            Event.Export(),
            SuperAdmin
        };
    }
}