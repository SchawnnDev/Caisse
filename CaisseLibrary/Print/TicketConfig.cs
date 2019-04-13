using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CaisseLibrary.Print
{
    public struct TicketConfig
    {
        public string HostName { get; set; }
        public string Address { get; set; }
        public string PostalCodeCity { get; set; }
        public string Telephone { get; set; }
        public string Siret { get; set; }
        public bool SiretActive { get; set; }

        public TicketConfig(string hostName, string address, string postalCodeCity, string telephone, string siret)
        {
            HostName = hostName;
            Address = address;
            PostalCodeCity = postalCodeCity;
            Telephone = telephone;
            Siret = siret;
            SiretActive = true;
        }

        public string GetTelephone => string.IsNullOrWhiteSpace(Telephone) ? "N.C." : Telephone;

        public string GetSiret => string.IsNullOrWhiteSpace(Siret) ? "N.C." : Siret;
    }
}