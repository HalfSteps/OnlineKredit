using _DB_AG__Online_Kredit.BusinessLogic;
using _DB_AG__Online_Kredit.Models;
using KreditFreigabe;
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
        public ActionResult KreditRechner()
        {
            Debug.WriteLine("HttpGet: Kredit/KreditRechner");

            KreditRechnerModel model = new KreditRechnerModel()
            {
                KreditBetrag = 25000,  
                Laufzeit = 12  
            };
            int k_id = -1;
            if (Request.Cookies["id"] != null && int.TryParse(Request.Cookies["id"].Value, out k_id))
            {
                KreditWunsch wunsch = KreditVerwaltung.KreditLaden(k_id);
                model.KreditBetrag = (int)wunsch.Betrag;
                model.Laufzeit = wunsch.Laufzeit;
            }


            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KreditRechner(KreditRechnerModel model)
        {
            Debug.WriteLine("HttpPost: Kredit/KreditRechner");

            if (ModelState.IsValid)
            {

                if (Request.Cookies["id"] == null)
                {

                    Kunde newKunde = KreditVerwaltung.ErzeugeKunde();

                    if (newKunde != null && KreditVerwaltung.KreditSpeichern(model.KreditBetrag, model.Laufzeit, newKunde.ID))
                    {
                        Response.Cookies.Add(new HttpCookie("id", newKunde.ID.ToString()));
                        return RedirectToAction("FinanzielleSituation");
                    }
                }
                else
                {
                    int idKunde = int.Parse(Request.Cookies["id"].Value);

                    if (KreditVerwaltung.KreditSpeichern(model.KreditBetrag, model.Laufzeit, idKunde))
                    {
                        return RedirectToAction("FinanzielleSituation");
                    }
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


            FinanzielleSituation situation = KreditVerwaltung.FinanzielleSituationLaden(model.ID_Kunde);
            if (situation != null)
            {
                model.EinkuenfteAlimenteUnterhalt = (double)situation.EinkuenfteAlimenteUnterhalt.Value;
                model.MonatsEinkommenNetto = (double)situation.MonatsEinkommenNetto.Value;
                model.Raten = (double)situation.Raten.Value;
                model.AusgabenAlimenteUnterhalt = (double)situation.AusgabenAlimenteUnterhalt.Value;
                model.Wohnkosten = (double)situation.Wohnkosten.Value;
            }


            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult FinanzielleSituation(FinanzielleSituationModel model)
        {

            Debug.WriteLine("HttpPost: Kredit/FinanzielleSituation");

            if (ModelState.IsValid)
            {
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
            Debug.WriteLine("HttpGet: Kredit/Arbeitgeber");

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

            Arbeitgeber arbeitgeberDaten = KreditVerwaltung.ArbeitgeberLaden(model.ID_Kunde);
            if (arbeitgeberDaten != null)
            {
                model.BeschaeftigungSeit = arbeitgeberDaten.BeschaeftigtSeit.Value.ToString("MM.yyyy");
                model.Firma = arbeitgeberDaten.Firma;
                model.ID_BeschaeftigungsArt = arbeitgeberDaten.FKBeschaeftigungsArt; ;
                model.ID_Branche = arbeitgeberDaten.FKBranche.Value;
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Arbeitgeber(ArbeitgeberModel model)
        {
            Debug.WriteLine("HttpPost: Kredit/Arbeitgeber");

            if (ModelState.IsValid)
            {
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
            Debug.WriteLine("HttpGet: Kredit/PersönlicheDaten");

            List<BildungsModel> alleBildungsAngaben = new List<BildungsModel>();
            List<FamilienstandModel> alleFamilienStandAngaben = new List<FamilienstandModel>();
            List<IdentifikationsModel> alleIdentifikationsangaben = new List<IdentifikationsModel>();
            List<StaatsbuergerschaftsModel> alleStaatsbuergerschaftsAngaben = new List<StaatsbuergerschaftsModel>();
            List<TitelModel> alleTitelAngaben = new List<TitelModel>();
            List<WohnartModel> alleWohnartAngaben = new List<WohnartModel>();

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


            Kunde kunde = KreditVerwaltung.PersönlicheDatenLaden(model.ID_Kunde);
            if (kunde != null)
            {
                model.Geschlecht = !string.IsNullOrEmpty(kunde.Geschlecht) && kunde.Geschlecht == "m" ? Geschlecht.Männlich : Geschlecht.Weiblich;
                model.Vorname = kunde.Vorname;
                model.Nachname = kunde.Nachname;
                model.ID_Titel = kunde.FKTitel.HasValue ? kunde.FKTitel.Value : 0;
                //model.GeburtsDatum = DateTime.Now;
                model.ID_Staatsbuergerschaft = kunde.FKStaatsangehoerigkeit;
                model.ID_Familienstand = kunde.FKFamilienstand.HasValue ? kunde.FKFamilienstand.Value : 0;
                model.ID_Wohnart = kunde.FKWohnart.HasValue ? kunde.FKWohnart.Value : 0;
                model.ID_Bildung = kunde.FKSchulabschluss.HasValue ? kunde.FKSchulabschluss.Value : 0;
                model.ID_Identifikationsart = kunde.FKIdentifikationsArt.HasValue ? kunde.FKIdentifikationsArt.Value : 0;
                model.IdentifikationsNummer = kunde.IdentifikationsNummer;
            }


            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PersönlicheDaten(PersönlicheDatenModel model)
        {
            Debug.WriteLine("HttpPost: Kredit/PersönlicheDaten");

            if (ModelState.IsValid)
            {
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
            Debug.WriteLine("HttpGet: KreditVerwaltung/KontoInformationen");

            KontoInformationsModel model = new KontoInformationsModel()
            {
                ID_Kunde = int.Parse(Request.Cookies["id"].Value)
            };

            KontoDaten daten = KreditVerwaltung.KontoInformationenLaden(model.ID_Kunde);
            if (daten != null)
            {
                model.BankName = daten.Bank;
                model.BIC = daten.BIC;
                model.IBAN = daten.IBAN;
                model.NeuesKonto = !daten.HatKonto;
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontoInformationen(KontoInformationsModel model)
        {
            Debug.WriteLine("HttpPost: KreditVerwaltung/KontoInformationen");

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
            Debug.WriteLine("HttpGet: Kredit/KontaktDaten");


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


            KontaktDaten daten = KreditVerwaltung.KontaktDatenLaden(model.ID_Kunde);
            if (daten != null)
            {
                model.Strasse = daten.Strasse;
                model.Hausnummer = daten.Hausnummer;
                model.EMail = daten.EMail;
                model.TelefonNummer = daten.Telefonnummer;
                model.Ort = daten.Ort.Bezeichnung;
            }


            return View(model);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult KontaktDaten(KontaktdatenModel model)
        {
            Debug.WriteLine("HttpPost: Kredit/KontaktDaten");

            if (ModelState.IsValid)
            {
                if (KreditVerwaltung.KontaktdatenSpeichern(
                    model.Strasse, 
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
            Debug.WriteLine("HttpGet: KreditVerwaltung/Zusammenfassung");

            ZusammenfassungsModel model = new ZusammenfassungsModel();

            model.ID_Kunde = int.Parse(Request.Cookies["id"].Value);

            Kunde aktKunde = KreditVerwaltung.KundeLaden(model.ID_Kunde);

            model.Betrag = (int)aktKunde.KreditWunsch.Betrag;
            model.Laufzeit = aktKunde.KreditWunsch.Laufzeit;

            model.MonatsNettoEinkommen = (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto.Value;
            model.Wohnkosten = (double)aktKunde.FinanzielleSituation.Wohnkosten.Value;
            model.EinkuenfteAlimenteUnterhalt = (double)aktKunde.FinanzielleSituation.EinkuenfteAlimenteUnterhalt.Value;
            model.EinkuenfteAlimenteUnterhalt = (double)aktKunde.FinanzielleSituation.AusgabenAlimenteUnterhalt.Value;
            model.Raten = (double)aktKunde.FinanzielleSituation.Raten.Value;

            model.Geschlecht = aktKunde.Geschlecht == "m" ? "Herr" : "Frau";
            model.Vorname = aktKunde.Vorname;
            model.Nachname = aktKunde.Nachname;
            model.Titel = aktKunde.Titel?.Bezeichnung;
            model.Geburtsdatum = DateTime.Now;
            model.Staatsangehoerigkeit = aktKunde.Staatsangehoerigkeit?.Bezeichnung;
            model.Familienstand = aktKunde.Familienstand?.Bezeichnung;
            model.Wohnart = aktKunde.Wohnart?.Bezeichnung;
            model.Schulabschluss = aktKunde.Schulabschluss?.Bezeichnung;
            model.IdentifikationsArt = aktKunde.IdentifikationsArt?.Bezeichnung;
            model.IdentifikationsNummer = aktKunde.IdentifikationsNummer;

            model.FirmaName = aktKunde.Arbeitgeber?.Firma;
            model.Branche = aktKunde.Arbeitgeber?.Branche?.Bezeichnung;
            model.BeschaeftigungsArt = aktKunde.Arbeitgeber?.BeschaeftigungsArt?.Bezeichnung;
            model.BeschaeftigtSeit = aktKunde.Arbeitgeber?.BeschaeftigtSeit.Value.ToShortDateString();

            model.Strasse = aktKunde.KontaktDaten?.Strasse;
            model.Hausnummer = aktKunde.KontaktDaten?.Hausnummer;
            model.Land = aktKunde.KontaktDaten?.Ort?.Land?.Bezeichnung;
            model.PLZ = aktKunde.KontaktDaten?.Ort?.PLZ;
            model.EMail = aktKunde.KontaktDaten?.EMail;
            model.Telefonnummer = aktKunde.KontaktDaten?.Telefonnummer;

            model.NeuesKonto = (bool)aktKunde.KontoDaten?.HatKonto;
            model.Bank = aktKunde.KontoDaten?.Bank;
            model.IBAN = aktKunde.KontoDaten?.IBAN;
            model.BIC = aktKunde.KontoDaten?.BIC;



            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Bestätigung(int id, bool? bestätigt)
        {
            if (bestätigt.HasValue && bestätigt.Value)
            {
                Debug.WriteLine("HttpPost: Kredit/Bestätigung");
                Debug.Indent();

                Kunde aktKunde = KreditVerwaltung.KundeLaden(id);
                Response.Cookies.Remove("id");

                bool istFreigegeben = FreigabeErteilt.Freigabe(
                                                          aktKunde.Geschlecht,
                                                            aktKunde.Vorname,
                                                            aktKunde.Nachname,
                                                            aktKunde.Familienstand.Bezeichnung,
                                                            (double)aktKunde.FinanzielleSituation.MonatsEinkommenNetto,
                                                            (double)aktKunde.FinanzielleSituation.Wohnkosten,
                                                            (double)aktKunde.FinanzielleSituation.EinkuenfteAlimenteUnterhalt,
                                                            (double)aktKunde.FinanzielleSituation.AusgabenAlimenteUnterhalt,
                                                            (double)aktKunde.FinanzielleSituation.Raten);

                /// Rüfe Service/DLL auf und prüfe auf Kreditfreigabe
                Debug.WriteLine($"Kreditfreigabe {(istFreigegeben ? "" : "nicht")}erteilt!");

                Debug.Unindent();
                return RedirectToAction("Index", "Freigabe", new { erfolgreich = istFreigegeben });

            }
            else
            {
                return RedirectToAction("Zusammenfassung");
            }
        }

    }
}