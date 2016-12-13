using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _DB_AG__Online_Kredit.Models
{
    public class KreditRechnerModel
    {

        [Required(ErrorMessage = "Ungültige Eingabe")]
        [Display(Name = "Kredit Betrag")]
        [Range(1000, 1000000, ErrorMessage = "Betrag liegt außerhalb des Bereichs von 1.000 € bis 1.000.000 €")]
        public int KreditBetrag { get; set; }

        [Required(ErrorMessage = "Ungültige Eingabe")]
        [Display(Name = "Gewünschte Laufzeit")]
        [Range(4, 90, ErrorMessage = "Laufzeit liegt außerhalb des Bereichs on 4 bis 90 Monaten")]
        public short Laufzeit { get; set; }

    }
}