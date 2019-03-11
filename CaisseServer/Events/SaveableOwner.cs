using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CaisseIO;

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

        public object[] Export()
        {
            throw new NotImplementedException();
        }

        public void Import(object[] args)
        {
            throw new NotImplementedException();
        }
    }
}