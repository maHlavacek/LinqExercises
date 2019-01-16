using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Logic
{
    public class Controller
    {
        public List<Company> Companies { get; }
        public List<Cd> Cds { get; }

        /// <summary>
        /// Daten sind in XML-Dateien gespeichert und werden per Linq2XML
        /// in die beiden Collections Cds und Companies geladen.
        /// Die XML-Dateien werden automatisch beim Compilieren
        /// in das bin\debug-Verzeichnis kopiert!
        /// </summary>
        public Controller()
        {
            XElement xElement = XDocument.Load("CDs.xml").Root;
            if (xElement == null) return;
            Cds = xElement.Elements("CD").Select(x =>
                new Cd
                {
                    Title = (string) x.Element("TITLE"),
                    Artist = (string) x.Element("ARTIST"),
                    Country = (string) x.Element("COUNTRY"),
                    Year = (int) x.Element("YEAR"),
                    Price = (double) x.Element("PRICE"),
                    Company = (string) x.Element("COMPANY")
                }).ToList();

            XElement element = XDocument.Load("Companies.xml").Root;
            if (element != null)
                Companies = element.Elements("COMPANY").Select(x =>
                    new Company
                    {
                        Name = (string) x.Element("NAME"),
                        Director = (string) x.Element("DIRECTOR"),
                        Founded = (int) x.Element("FOUNDED"),
                    }).ToList();
        }

    }
}