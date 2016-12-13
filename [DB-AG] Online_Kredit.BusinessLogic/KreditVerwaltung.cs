using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace _DB_AG__Online_Kredit.BusinessLogic
{
    public class KreditVerwaltung
    {
        /// <summary>
        /// Erzeugt einen "leeren" dummy Kunden
        /// zu dem in Folge alle Konsumkredit Daten
        /// verknüpft werden können.
        /// </summary>
        /// <returns>einen leeren Kunden wenn erfolgreich, ansonsten null</returns>
        public static Kunde ErzeugeKunde()
        {
            Debug.WriteLine("KreditVerwaltung: ErzeugeKunde");
            Debug.Indent();

            Kunde newKunde = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    newKunde = new BusinessLogic.Kunde()
                    {
                        Vorname = "anonym",
                        Nachname = "anonym",
                        Geschlecht = "w"
                    };
                    context.AlleKunden.Add(newKunde);

                    Debug.WriteLine("ErzeugeKunde: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kunden angelegt!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ErzeugeKunde");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return newKunde;
        }

        /// <summary>
        /// Speichert zu einer übergebenene ID den Wunsch Kredit und dessen Laufzeit ab
        /// </summary>
        /// <param name="kreditBetrag">die Höhe des gewünschten Kredits</param>
        /// <param name="laufzeit">die Laufzeit des gewünschten Kredits</param>
        /// <param name="idKunde">die ID des Kunden zu dem die Angaben gespeichert werden sollen</param>
        /// <returns>true wenn Eintragung gespeichert werden konnte und der Kunde existiert, ansonsten false</returns>
        public static bool KreditSpeichern(double kreditBetrag, short laufzeit, int idKunde)
        {
            Debug.WriteLine("KreditVerwaltung: KreditSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {

                        Debug.WriteLine("KreditSpeichern: Create KreditWunsch");

                        KreditWunsch newKreditWunsch = new KreditWunsch()
                        {
                            Betrag = (decimal)kreditBetrag,
                            Laufzeit = laufzeit,
                            Bewilligt = false,
                            FKKunde = aktKunde.ID
                        };

                        context.AlleKreditWünsche.Add(newKreditWunsch);
                    }

                    Debug.WriteLine("KreditSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("KreditSpeichern: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Kredit gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KreditSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }



        public static bool ArbeitgeberSpeichern(string firmenName, int idBeschäftigungsArt, int idBranche, string beschäftigtSeit, int idKunde)
        {
            Debug.WriteLine("KreditVerwaltung: ArbeitgeberSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        Arbeitgeber neuerArbeitgeber = new Arbeitgeber()
                        {
                            BeschaeftigtSeit = DateTime.Parse(beschäftigtSeit),
                            FKBranche = idBranche,
                            FKBeschaeftigungsArt = idBeschäftigungsArt,
                            Firma = firmenName
                        };
                        aktKunde.Arbeitgeber = neuerArbeitgeber;
                    }

                    Debug.WriteLine("ArbeitgeberSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("Arbeitgeber: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} ArbeitgeberDaten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in ArbeitgeberSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }

        public static List<BeschaeftigungsArt> BeschaeftigungenLaden()
        {
            Debug.WriteLine("KreditVerwaltung: BeschaeftigungenLaden");
            Debug.Indent();

            List<BeschaeftigungsArt> alleBeschaeftigungen = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleBeschaeftigungen = context.AlleBeschaeftigungsArten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in Beschaeftigungsart");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleBeschaeftigungen;
        }



        public static List<Branche> BranchenLaden()
        {
            Debug.WriteLine("KreditVerwaltung: BranchenLaden");
            Debug.Indent();

            List<Branche> alleBranchen = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleBranchen = context.AlleBranchen.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in BranchenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleBranchen;
        }




        public static bool FinanzielleSituationSpeichern(double nettoEinkommen, double ratenVerpflichtungen, double wohnkosten, double einkünfteAlimenteUnterhalt, double unterhaltsZahlungen, int idKunde)
        {
            Debug.WriteLine("KreditVerwaltung: FinanzielleSituationSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        FinanzielleSituation neueFinanzielleSituation = new FinanzielleSituation()
                        {
                            MonatsEinkommenNetto = (decimal)nettoEinkommen,
                            AusgabenAlimenteUnterhalt = (decimal)unterhaltsZahlungen,
                            EinkuenfteAlimenteUnterhalt = (decimal)einkünfteAlimenteUnterhalt,
                            Wohnkosten = (decimal)wohnkosten,
                            Raten = (decimal)ratenVerpflichtungen,
                            ID = idKunde
                        };

                        context.AlleFinanzielleSituationen.Add(neueFinanzielleSituation);
                    }

                    Debug.WriteLine("FinanzielleSituationSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("FinanzielleSituationSpeichern: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} FinanzielleSituation gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in FinanzielleSituation");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }





        /// <summary>
        /// Liefert alle Schulabschlüsse zurück
        /// </summary>
        /// <returns>alle Schulabschlüsse oder null bei einem Fehler</returns>
        public static List<Schulabschluss> BildungsangabenLaden()
        {
            Debug.WriteLine("KreditVerwaltung: BildungsangabenLaden");
            Debug.Indent();

            List<Schulabschluss> alleAbschlüsse = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleAbschlüsse = context.AlleSchulabschluesse.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in BildungsAngabenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleAbschlüsse;
        }

        /// <summary>
        /// Liefert alle FamilienStand zurück
        /// </summary>
        /// <returns>alle FamilienStand oder null bei einem Fehler</returns>
        public static List<Familienstand> FamilienstandLaden()
        {
            Debug.WriteLine("KreditVerwaltung: FamilienstandLaden");
            Debug.Indent();

            List<Familienstand> alleFamilienStandsAngaben = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleFamilienStandsAngaben = context.AlleFamilienstandAngaben.ToList();
                }
            }
            catch (Exception ex)
            {   
                Debug.WriteLine("Fehler in FamilienStandAngabenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleFamilienStandsAngaben;
        }

        /// <summary>
        /// Liefert alle Länder zurück
        /// </summary>
        /// <returns>alle Länder oder null bei einem Fehler</returns>
        public static List<Land> LaenderLaden()
        {
            Debug.WriteLine("KreditVerwaltung LaenderLaden");
            Debug.Indent();

            List<Land> alleLänder = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleLänder = context.AlleLänder.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in LaenderLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleLänder;
        }

        /// <summary>
        /// Liefert alle Wohnarten zurück
        /// </summary>
        /// <returns>alle Wohnarten oder null bei einem Fehler</returns>
        public static List<Wohnart> WohnartenLaden()
        {
            Debug.WriteLine("KreditVerwaltung: WohnartenLaden");
            Debug.Indent();

            List<Wohnart> alleWohnarten = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleWohnarten = context.AlleWohnarten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in WohnartenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleWohnarten;
        }

        /// <summary>
        /// Liefert alle IdentifikatikonsArt zurück
        /// </summary>
        /// <returns>alle IdentifikatikonsArt oder null bei einem Fehler</returns>
        public static List<IdentifikationsArt> IdentifikationsangabenLaden()
        {
            Debug.WriteLine("KreditVerwaltung: IdentifikiationsangabenLaden");
            Debug.Indent();

            List<IdentifikationsArt> alleIdentifikationsArten = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleIdentifikationsArten = context.AlleIdentifikationsArten.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in IdentifikiationsAngabenLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleIdentifikationsArten;
        }

        /// <summary>
        /// Liefert alle Titel zurück
        /// </summary>
        /// <returns>alle Titel oder null bei einem Fehler</returns>
        public static List<Titel> TitelLaden()
        {
            Debug.WriteLine("KreditVerwaltung: TitelLaden");
            Debug.Indent();

            List<Titel> alleTitel = null;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {
                    alleTitel = context.AlleTitel.ToList();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in TitelLaden");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return alleTitel;
        }



        /// <summary>
        /// Speichert die Daten für die übergebene idKunde
        /// </summary>
        /// <param name="idTitel">der Titel des Kunden</param>
        /// <param name="geschlecht">das Geschlecht des Kunden</param>
        /// <param name="geburtsDatum">das Geburtsdatum des Kunden</param>
        /// <param name="vorname">der Vorname des Kunden</param>
        /// <param name="nachname">der Nachname des Kunden</param>
        /// <param name="idBildung">die Bildung des Kunden</param>
        /// <param name="idFamilienstand">der Familienstand des Kunden</param>
        /// <param name="idIdentifikationsart">die Identifikations des Kunden</param>
        /// <param name="identifikationsNummer">der Identifikations-Nummer des Kunden</param>
        /// <param name="idStaatsbuergerschaft">die Staatsbürgerschaft des Kunden</param>
        /// <param name="idWohnart">die Wohnart des Kunden</param>
        /// <param name="idKunde">die ID des Kunden</param>
        /// <returns>true wenn das Anpassen der Werte erfolgreich war, ansonsten false</returns>
        public static bool PersönlicheDatenSpeichern(int? idTitel, string geschlecht, DateTime geburtsDatum, string vorname, string nachname, int idBildung, int idFamilienstand, int idIdentifikationsart, string identifikationsNummer, string idStaatsbuergerschaft, int idWohnart, int idKunde)
        {
            Debug.WriteLine("KreditVerwaltung: PersönlicheDatenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {

                        aktKunde.Vorname = vorname;
                        aktKunde.Nachname = nachname;
                        aktKunde.FKFamilienstand = idFamilienstand;
                        aktKunde.FKSchulabschluss = idBildung;
                        aktKunde.FKStaatsangehoerigkeit = idStaatsbuergerschaft;
                        aktKunde.FKTitel = idTitel;
                        aktKunde.FKIdentifikationsArt = idIdentifikationsart;
                        aktKunde.IdentifikationsNummer = identifikationsNummer;
                        aktKunde.Geschlecht = geschlecht;
                        aktKunde.FKWohnart = idWohnart;
                    }

                    Debug.WriteLine("PersönlicheDatenSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("PersönlicheDatenSpeichern: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} PersönlicheDaten gespeichert!");


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in PersönlicheDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }



        public static bool KontoInformationenSpeichern(string bankName, string iban, string bic, bool neuesKonto, int idKunde)
        {
            Debug.WriteLine("KreditVerwaltung: KontoInformationenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {
                        KontoDaten neueKontoDaten = new KontoDaten()
                        {
                            Bank = bankName,
                            IBAN = iban,
                            BIC = bic,
                            HatKonto = !neuesKonto,
                            ID = idKunde
                        };

                        context.AlleKontoDaten.Add(neueKontoDaten);
                    }

                    Debug.WriteLine("KontoInformationenSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("KontoInformationenSpeichern: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} Konto-Daten gespeichert!");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontoInformationenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }


        public static bool KontaktdatenSpeichern(string strasse, string hausnummer, string mail, string telefonNummer, int idKunde, string ort, string idplz, string idland)
        {
            Debug.WriteLine("KreditVerwaltung: KontaktDatenSpeichern");
            Debug.Indent();

            bool erfolgreich = false;

            try
            {
                using (var context = new dbKreditRechnerEntities())
                {

                    /// speichere zum Kunden die Angaben
                    Kunde aktKunde = context.AlleKunden.Where(x => x.ID == idKunde).FirstOrDefault();

                    if (aktKunde != null)
                    {

                        KontaktDaten newKontakt = new KontaktDaten()
                        {
                            Strasse = strasse,
                            Hausnummer = hausnummer,
                            EMail = mail,
                            Telefonnummer = telefonNummer,
                            ID = idKunde
                        };
                        context.AlleKontaktDaten.Add(newKontakt);
                        Debug.WriteLine("KontaktDatenSpeichern: NewOrtKontakt");

                        Ort newOrtKontakt = new Ort()
                        {
                            ID = idKunde,
                            PLZ = idplz,
                            Bezeichnung = ort,
                            FKLand = idland
                            
                        };
                        context.AlleOrte.Add(newOrtKontakt);
                        
                    }

                    Debug.WriteLine("KontaktDatenSpeichern: DBContextSave");
                    int anzahlZeilenBetroffen = context.SaveChanges();
                    Debug.WriteLine("KontaktDatenSpeichern: BoolchangeErfolgreich");
                    erfolgreich = anzahlZeilenBetroffen >= 1;
                    Debug.WriteLine($"{anzahlZeilenBetroffen} KontaktDaten gespeichert!");


                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Fehler in KontaktDatenSpeichern");
                Debug.Indent();
                Debug.WriteLine(ex.Message);
                Debug.Unindent();
                Debugger.Break();
            }

            Debug.Unindent();
            return erfolgreich;
        }

    }
}