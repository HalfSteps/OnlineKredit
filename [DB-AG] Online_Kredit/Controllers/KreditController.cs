﻿using _DB_AG__Online_Kredit.BusinessLogic;
using _DB_AG__Online_Kredit.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace _DB_AG__Online_Kredit.Controllers
{
    public class KreditController : Controller
    {
        [HttpGet]
        // GET: Kredit
        public ActionResult KreditRechner()
        {
            Debug.WriteLine("HttpGet: Kredit/KreditRechner");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KreditRechner(KreditRechnerModel model)
        {
            Debug.WriteLine("HttpPost: Kredit/KreditRechner");

            if (ModelState.IsValid)
            {
                Kunde newKunde = KreditVerwaltung.ErzeugeKunde();

                if (newKunde != null && KreditVerwaltung.KreditSpeichern(model.KreditBetrag, model.Laufzeit, newKunde.ID))
                {
                    Response.Cookies.Add(new HttpCookie("id", newKunde.ID.ToString()));
                    return RedirectToAction("FinanzielleSituation");
                }
            }

            return View(model);

        }



        [HttpGet]
        public ActionResult FinanzielleSituation()
        {
            Debug.WriteLine("HttpGet: Kredit/FinanzielleSituation");

            FinanzielleSituationModel model = new FinanzielleSituationModel()
            {
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {

            Debug.WriteLine("HttpPost: Kredit/FinanzielleSituation");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KreditVerwaltung.FinanzielleSituationSpeichern(
                                                model.MonatsEinkommenNetto,
                                                model.Raten,
                                                model.Wohnkosten,
                                                model.EinkuenfteAlimenteUnterhalt,
                                                model.AusgabenAlimenteUnterhalt,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("Arbeitgeber");
                }
            }

            return View(model);
        }



        [HttpGet]
        public ActionResult Arbeitgeber()
        {
            Debug.WriteLine("GET - Kredit - Arbeitgeber");

            List<BeschaeftigungModel> alleBeschaeftigungen = new List<BeschaeftigungModel>();
            List<BrancheModel> alleBranchen = new List<BrancheModel>();

            foreach (var branche in KreditVerwaltung.BranchenLaden())
            {
                alleBranchen.Add(new BrancheModel()
                {
                    ID = branche.ID.ToString(),
                    Bezeichnung = branche.Bezeichnung
                });
            }

            foreach (var beschaeftigung in KreditVerwaltung.BeschaeftigungenLaden())
            {
                alleBeschaeftigungen.Add(new BeschaeftigungModel()
                {
                    ID = beschaeftigung.ID.ToString(),
                    Bezeichnung = beschaeftigung.Bezeichnung
                });
            }

            ArbeitgeberModel model = new ArbeitgeberModel()
            {
                AlleBeschaeftigungen = alleBeschaeftigungen,
                AlleBranchen = alleBranchen,
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };

            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.WriteLine("HttpPost: Kredit - Arbeitgeber");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KreditVerwaltung.ArbeitgeberSpeichern(
                                                model.Firma,
                                                model.ID_BeschaeftigungsArt,
                                                model.ID_Branche,
                                                model.BeschaeftigungSeit,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("PersönlicheDaten");
                }
            }
            return View();
        }





        [HttpGet]
        public ActionResult PersönlicheDaten()
        {
            Debug.WriteLine("HttpGet: Kredit - PersönlicheDaten");

            List<BildungsModel> alleBildungsAngaben = new List<BildungsModel>();
            List<FamilienstandModel> alleFamilienStandAngaben = new List<FamilienstandModel>();
            List<IdentifikationsModel> alleIdentifikationsangaben = new List<IdentifikationsModel>();
            List<StaatsbuergerschaftsModel> alleStaatsbuergerschaftsAngaben = new List<StaatsbuergerschaftsModel>();
            List<TitelModel> alleTitelAngaben = new List<TitelModel>();
            List<WohnartModel> alleWohnartAngaben = new List<WohnartModel>();

            /// Lade Daten aus Logic
            foreach (var bildungsAngabe in KreditVerwaltung.BildungsangabenLaden())
            {
                alleBildungsAngaben.Add(new BildungsModel()
                {
                    ID = bildungsAngabe.ID.ToString(),
                    Bezeichnung = bildungsAngabe.Bezeichnung
                });
            }

            foreach (var familienStand in KreditVerwaltung.FamilienstandLaden())
            {
                alleFamilienStandAngaben.Add(new FamilienstandModel()
                {
                    ID = familienStand.ID.ToString(),
                    Bezeichnung = familienStand.Bezeichnung
                });
            }
            foreach (var Identifikationsangabe in KreditVerwaltung.IdentifikationsangabenLaden())
            {
                alleIdentifikationsangaben.Add(new IdentifikationsModel()
                {
                    ID = Identifikationsangabe.ID.ToString(),
                    Bezeichnung = Identifikationsangabe.Bezeichnung
                });
            }
            foreach (var land in KreditVerwaltung.LaenderLaden())
            {
                alleStaatsbuergerschaftsAngaben.Add(new StaatsbuergerschaftsModel()
                {
                    ID = land.ID,
                    Bezeichnung = land.Bezeichnung
                });
            }
            foreach (var titel in KreditVerwaltung.TitelLaden())
            {
                alleTitelAngaben.Add(new TitelModel()
                {
                    ID = titel.ID.ToString(),
                    Bezeichnung = titel.Bezeichnung
                });
            }
            foreach (var wohnart in KreditVerwaltung.WohnartenLaden())
            {
                alleWohnartAngaben.Add(new WohnartModel()
                {
                    ID = wohnart.ID.ToString(),
                    Bezeichnung = wohnart.Bezeichnung
                });
            }


            PersönlicheDatenModel model = new PersönlicheDatenModel()
            {
                AlleBildungsangaben = alleBildungsAngaben,
                AlleFamilienstandangaben = alleFamilienStandAngaben,
                AlleIdentifikationsangaben = alleIdentifikationsangaben,
                AlleStaatsbuergerschaftsangaben = alleStaatsbuergerschaftsAngaben,
                AlleTitelangaben = alleTitelAngaben,
                AlleWohnartangaben = alleWohnartAngaben,
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersönlicheDaten(PersönlicheDatenModel model)
        {
            Debug.WriteLine("HttpPost: Kredit - PersönlicheDaten");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KreditVerwaltung.PersönlicheDatenSpeichern(
                                                model.ID_Titel,
                                                model.Geschlecht == Geschlecht.Männlich ? "m" : "w",
                                                model.Geburtsdatum,
                                                model.Vorname,
                                                model.Nachname,
                                                model.ID_Bildung,
                                                model.ID_Familienstand,
                                                model.ID_Identifikationsart,
                                                model.IdentifikationsNummer,
                                                model.ID_Staatsbuergerschaft,
                                                model.ID_Wohnart,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("KontoInformationen");
                }
            }
            return View();
        }



        [HttpGet]
        public ActionResult KontoInformationen()
        {
            Debug.WriteLine("HttpGet: KonsumKredit - KontoInformationen");

            KontoInformationsModel model = new KontoInformationsModel()
            {
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontoInformationen(KontoInformationsModel model)
        {
            Debug.WriteLine("HttpPost: KonsumKredit - KontoInformationen");

            if (ModelState.IsValid)
            {
                /// speichere Daten über BusinessLogic
                if (KreditVerwaltung.KontoInformationenSpeichern(
                                                model.BankName,
                                                model.IBAN,
                                                model.BIC,
                                                model.NeuesKonto,
                                                model.ID_Kunde))
                {
                    return RedirectToAction("KontaktDaten");
                }
            }

            return View();
        }



        [HttpGet]
        public ActionResult KontaktDaten()
        {
            Debug.WriteLine("HttpGet: Kredit - KontaktDaten");


            List<StaatsbuergerschaftsModel> alleLaender = new List<StaatsbuergerschaftsModel>();

            foreach (var landauswahl in KreditVerwaltung.LaenderLaden())
            {
                alleLaender.Add(new StaatsbuergerschaftsModel()
                {
                    ID = landauswahl.ID,
                    Bezeichnung = landauswahl.Bezeichnung
                });
            }


            KontaktdatenModel model = new KontaktdatenModel()
            {
                AlleLaender = alleLaender,
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };


                return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktdatenModel model)
        {
            Debug.WriteLine("HttpPost: Kredit - KontaktDaten");

            if (ModelState.IsValid)
            {
                if (KreditVerwaltung.KontaktdatenSpeichern(
                    model.Straße, 
                    model.Hausnummer,  
                    model.EMail,
                    model.TelefonNummer, 
                    model.ID_Kunde,
                    model.Ort,
                    model.ID_PLZ,
                    model.ID_Land
                    
                    ))
                {
                    return RedirectToAction("Zusammenfassung");
                }
            }

            return View(model);
        }


        [HttpGet]
        public ActionResult Zusammenfassung()
        {
            Debug.WriteLine("GET - KonsumKredit - Zusammenfassung");

            /// ermittle für diese Kunden_ID
            /// alle gespeicherten Daten (ACHTUNG! das sind viele ....)
            /// gib Sie alle in das ZusammenfassungsModel (bzw. die UNTER-Modelle) 
            /// hinein.
            ZusammenfassungsModel model = new ZusammenfassungsModel();
            model.ID_Kunde = int.Parse(Request.Cookies["idKunde"].Value);

            /// lädt ALLE Daten zu diesem Kunden (also auch die angehängten/referenzierten
            /// Entities) aus der DB
            Kunde aktKunde = KreditVerwaltung.KundeLaden(model.ID_Kunde);

            model.GewünschterBetrag = (int)aktKunde.KreditWunsch.Betrag.Value;
            model.Laufzeit = aktKunde.KreditWunsch.Laufzeit.Value;

            model.NettoEinkommen = (double)aktKunde.FinanzielleSituation.MonatsEinkommen.Value;
            model.Wohnkosten = (double)aktKunde.FinanzielleSituation.Wohnkosten.Value;
            model.EinkünfteAlimenteUnterhalt = (double)aktKunde.FinanzielleSituation.EinkuenfteAlimenteUnterhalt.Value;
            model.UnterhaltsZahlungen = (double)aktKunde.FinanzielleSituation.AusgabenALIUNT.Value;
            model.RatenVerpflichtungen = (double)aktKunde.FinanzielleSituation.RatenZahlungen.Value;

            model.Geschlecht = aktKunde.Gechlecht == "m" ? "Herr" : "Frau";
            model.Vorname = aktKunde.Vorname;
            model.Nachname = aktKunde.Nachname;
            model.Titel = aktKunde.Titel?.Bezeichnung;
            model.TitelNachstehend = aktKunde.TitelNachstehend?.Bezeichnung;
            model.GeburtsDatum = DateTime.Now;
            model.Staatsbuergerschaft = aktKunde.Staatsangehoerigkeit?.Bezeichnung;
            model.AnzahlUnterhaltspflichtigeKinder = -1;
            model.Familienstand = aktKunde.Familienstand?.Bezeichnung;
            model.Wohnart = aktKunde.Wohnart?.Bezeichnung;
            model.Bildung = aktKunde.Schulabschluss?.Bezeichnung;
            model.Identifikationsart = aktKunde.IdentifikationsArt?.Bezeichnung;
            model.IdentifikationsNummer = aktKunde.IdentifikationsNummer;

            model.FirmenName = aktKunde.Arbeitgeber?.Firma;
            model.BeschäftigungsArt = aktKunde.Arbeitgeber?.BeschaeftigungsArt?.Bezeichnung;
            model.Branche = aktKunde.Arbeitgeber?.Branche?.Bezeichnung;
            model.BeschäftigtSeit = aktKunde.Arbeitgeber?.BeschaeftigtSeit.Value.ToShortDateString();

            model.Strasse = aktKunde.KontaktDaten?.Strasse;
            model.Hausnummer = aktKunde.KontaktDaten?.Hausnummer;
            model.Ort = aktKunde.KontaktDaten?.Ort.PLZ;
            model.Mail = aktKunde.KontaktDaten?.EMail;
            model.TelefonNummer = aktKunde.KontaktDaten?.Telefonnummer;

            model.NeuesKonto = (bool)aktKunde.KontoDaten?.IstDB_Kunde.Value;
            model.BankName = aktKunde.KontoDaten?.BankName;
            model.IBAN = aktKunde.KontoDaten?.IBAN;
            model.BIC = aktKunde.KontoDaten?.BIC;

            /// gib model an die View
            return View(model);
        }


        //public ActionResult Zusammenfassung()
        //{
        //    return View();
        //}

        //public ActionResult Zusammenfassung(ZusammenfassungModel model)
        //{
        //    return View();
        //}

        //}
    }
}