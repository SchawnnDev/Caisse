using System;
using System.Linq;
using System.Media;
using CaisseServer;
using CaisseServer.Events;

namespace CaisseDesktop.Admin
{
    public class SessionAdmin
    {
        public static SaveableOwner CurrentAdmin { get; set; } = null;

        public static bool IsAuthenticated() => CurrentAdmin != null;

        public static bool HasPermission(string permission) => true; // for the moment
                                                                     //IsAuthenticated() && CurrentAdmin.HasPermission(permission);

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