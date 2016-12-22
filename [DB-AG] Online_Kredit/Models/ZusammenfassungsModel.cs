using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _DB_AG__Online_Kredit.Models
{
    public class ZusammenfassungsModel
    {

        public int ID_Kunde { get; set; }


        public int Betrag { get; set; }
        public int Laufzeit { get; set; }

        public double MonatsNettoEinkommen { get; set; }
        public double Wohnkosten { get; set; }
        public double EinkuenfteAlimenteUnterhalt { get; set; }
        public double AusgabenAlimenteUnterhalt { get; set; }
        public double Raten { get; set; }


        public string Geschlecht { get; set; }
        public string Vorname { get; set; }
        public string Nachname { get; set; }
        public string Titel { get; set; }
        public DateTime Geburtsdatum { get; set; }
        public string Staatsangehoerigkeit { get; set; }
        public string Familienstand { get; set; }
        public string Wohnart { get; set; }
        public string Schulabschluss { get; set; }
        public string IdentifikationsArt { get; set; }
        public string IdentifikationsNummer { get; set; }


        public string FirmaName { get; set; }
        public string BeschaeftigungsArt { get; set; }
        public string Branche { get; set; }
        public string BeschaeftigtSeit { get; set; }


        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string Land { get; set; }
        public string Ort { get; set; }
        public string PLZ { get; set; }
        public string EMail { get; set; }
        public string Telefonnummer { get; set; }


        public bool NeuesKonto { get; set; }
        public string Bank { get; set; }
        public string IBAN { get; set; }
        public string BIC { get; set; }

    }
}