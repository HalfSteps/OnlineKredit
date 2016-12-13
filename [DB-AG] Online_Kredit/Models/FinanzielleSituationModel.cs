using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _DB_AG__Online_Kredit.Models
{
    public class FinanzielleSituationModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Leeres Feld ist nicht erlaubt")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl zwischen 430 und 999999 eingeben")]
        [Range(430, 999999, ErrorMessage = "Wert muss zwischen 430 und 999999 liegen")]
        [Display(Name = "Ihr Netto-Einkommen (14x jährlich)")]
        public double MonatsEinkommenNetto { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leeres Feld ist nicht erlaubt")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl zwischen 0 und 99999 eingeben")]
        [Range(0, 99999, ErrorMessage = "Wert muss zwischen 0 und 99999 liegen")]
        [Display(Name = "Ihre Wohnkosten (Miete inkl. Heizung, Strom, Gas etc.)")]
        public double Wohnkosten { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leeres Feld ist nicht erlaubt")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl zwischen 0 und 99999 eingeben")]
        [Range(0, 99999, ErrorMessage = "Wert muss zwischen 0 und 99999 liegen")]
        [Display(Name = "Ihre Einkünfte die durch Alimente, Unterhalt etc. bezogen werden")]
        public double EinkuenfteAlimenteUnterhalt { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leeres Feld ist nicht erlaubt")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl zwischen 0 und 9999 eingeben")]
        [Range(0, 9999, ErrorMessage = "Wert muss zwischen 0 und 9999 liegen")]
        [Display(Name = "Ihre Auszahlungen wie Alimente, Unterhalt etc.")]
        public double AusgabenAlimenteUnterhalt { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Leeres Feld ist nicht erlaubt")]
        [DataType(DataType.Currency, ErrorMessage = "Bitte eine Zahl zwischen 0 und 9999 eingeben")]
        [Range(0, 9999, ErrorMessage = "Wert muss zwischen 0 und 9999 liegen")]
        [Display(Name = "Ihre Ratenvereinbarungen (Gesamt)")]
        public double Raten { get; set; }

        [HiddenInput(DisplayValue = false)]
        [Required]
        public int ID_Kunde { get; set; }
    }
}