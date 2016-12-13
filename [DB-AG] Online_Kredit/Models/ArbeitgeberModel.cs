using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace _DB_AG__Online_Kredit.Models
{
    public class ArbeitgeberModel
    {

        public string Firma { get; set; }

        public int ID_BeschaeftigungsArt { get; set; }

        public int ID_Branche { get; set; }

        public string BeschaeftigungSeit { get; set; }

        public List<BeschaeftigungModel> AlleBeschaeftigungen { get; set; }
        public List<BrancheModel> AlleBranchen { get; set; }

        public int ID_Kunde { get; set; }
    }
}