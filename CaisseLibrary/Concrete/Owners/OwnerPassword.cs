using System;
using System.Collections;
using System.Text;
using CaisseLibrary.Interfaces;

namespace CaisseLibrary.Concrete.Owners
{
    public class OwnerPassword : IPassword
    {
        public string Generate(int length)
        {
            const string valid = "0123456789";
            var res = new StringBuilder();
            var rnd = new Random();

            while (0 < length--)
                res.Append(valid[rnd.Next(valid.Length)]);
            return res.ToString();
        }

        public string GenerateNoDuplicate(int length, IList existing)
        {
            string login;

            do
            {
                login = Generate(length);
            } while (existing.Contains(login));

            return login;
        }
    }
}