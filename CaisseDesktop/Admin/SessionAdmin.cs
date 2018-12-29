using System;
using System.Linq;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Admin
{
    public class SessionAdmin
    {
        public static SaveableOwner CurrentAdmin { get; set; }

        public static bool IsAuthenticated()
        {
            return CurrentAdmin != null;
        }

        public static bool HasPermission(string permission)
        {
            return true;
        }
        //IsAuthenticated() && CurrentAdmin.HasPermission(permission);

        public static void UpdateIfEdited(SaveableOwner owner)
        {
            if (CurrentAdmin == null || owner == null || CurrentAdmin.Id != owner.Id) return;

            CurrentAdmin = owner;
        }

        public static bool HasNotPermission(string permission)
        {
            //SystemSounds.Beep.Play();
            //IsAuthenticated() && CurrentAdmin.HasPermission(permission);
            return false; // for the moment
        }

        public static bool Login(string login)
        {
            using (var db = new CaisseServerContext())
            {
                var admin = db.Owners.FirstOrDefault(t => t.Login.Equals(login));

                if (admin == null) return false;

                CurrentAdmin = admin;

                admin.LastLogin = DateTime.Now;

                db.SaveChangesAsync();

                return true;
            }
        }

        public static void Logout()
        {
            using (var db = new CaisseServerContext())
            {
                CurrentAdmin.LastLogout = DateTime.Now;
                db.SaveChangesAsync();
            }

            CurrentAdmin = null;
        }
    }
}