using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _DB_AG__Online_Kredit.Models
{
    public class KontaktdatenModel
    {

        public string Strasse { get; set; }
        public string Hausnummer { get; set; }
        public string ID_PLZ { get; set; }
        public string EMail { get; set; }
        public string TelefonNummer { get; set; }
        public int ID_Kunde { get; set; }
        public string Ort { get; set; }
        public string ID_Land { get; set; }

        public List<OrtschaftsModel> AlleOrte { get; set; }
        public List<StaatsbuergerschaftsModel> AlleLaender { get; set; }
    }
}