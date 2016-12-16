using _DB_AG__Online_Kredit.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _DB_AG__Online_Kredit.Models
{

    public enum Geschlecht
    {
        [Display(Name = "Herr")]
        Männlich,
        [Display(Name = "Frau")]
        Weiblich
    }

    public class PersönlicheDatenModel
    {

        [EnumDataType(typeof(Geschlecht))]
        public Geschlecht Geschlecht { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "max. 50 Zeichen")]
        public string Vorname { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "max. 50 Zeichen")]
        public string Nachname { get; set; }

        [Required]
        [Display(Name = "Titel")]
        public int? ID_Titel { get; set; }

        [DataType(DataType.Date)]
        public DateTime Geburtsdatum { get; set; }

        [Required]
        [Display(Name = "Staatsbürgerschaft")]
        public string ID_Staatsbuergerschaft { get; set; }

        [Display(Name = "Anzahl unterhaltspflichtiger Kinder")]
        public int AnzahlUnterhaltspflichtigeKinder { get; set; }

        [Required]
        [Display(Name = "Aktueller Familienstand")]
        public int ID_Familienstand { get; set; }

        [Required]
        [Display(Name = "Aktuelle Wohnsituation")]
        public int ID_Wohnart { get; set; }

        [Required]
        [Display(Name = "Aktuelle Bildung")]
        public int ID_Bildung { get; set; }

        [Required]
        [Display(Name = "Identifikationstyp")]
        public int ID_Identifikationsart { get; set; }

        [StringLength(20, ErrorMessage = "max. 20 Zeichen erlaubt")]
        [Display(Name = "Identifikations-Nummer")]
        public string IdentifikationsNummer { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int ID_Kunde { get; set; }

        public List<FamilienstandModel> AlleFamilienstandangaben { get; set; }
        public List<StaatsbuergerschaftsModel> AlleStaatsbuergerschaftsangaben { get; set; }
        public List<WohnartModel> AlleWohnartangaben { get; set; }
        public List<BildungsModel> AlleBildungsangaben { get; set; }
        public List<IdentifikationsModel> AlleIdentifikationsangaben { get; set; }
        public List<TitelModel> AlleTitelangaben { get; set; }
    }
}




