using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CaisseServer.Export;
using CaisseServer.Export.Exceptions;

namespace CaisseServer.Events
{
    [Table("owners")]
    public class SaveableOwner
    {
        public SaveableOwner()
        {
        }

        public SaveableOwner(string login, string firstName, string name, string permissions)
        {
            Login = login;
            FirstName = firstName;
            Name = name;
            Permissions = permissions;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Login { get; set; }

        public string FirstName { get; set; }

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

        public bool HasPermission(string permission) => SuperAdmin || Permissions.Equals("*") || GetPermissions().Contains(permission);

        public override string ToString() => Name;

    }
}