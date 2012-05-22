using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;

namespace PWr.DW2012.Movies.Model {

    public class Award {
        [Key]
        public string Id { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string Colloquial { get; set; }
        public string Year { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public ISet<Movie> Movies { get; set; }
        public ISet<Cast> Cast { get; set; } 
    }



    class AwardsLoader : PWr.DW2012.Movies.Program.TableLoader<Award, string> {
        XmlNamespaceManager ns;

        public AwardsLoader(Program session) : base("codes09.xml", session) {
            ns = new XmlNamespaceManager(new NameTable());
            ns.AddNamespace("x", "http://www.semanticweb.org/movies/codes#");
        }

        protected override string GetKey(Award row) {
            return row.Id;
        }

        protected override XNode GetTable(XDocument doc) {
            return doc.XPathSelectElement("//x:awardtypes", ns);
        }

        protected override IEnumerable<XElement> GetRows(XNode table) {
            return table.XPathSelectElements(".//x:awcodeentry", ns);
        }

        protected override void ProcessRow(XElement row, string[] cells) {
            var r = new string[6]; // see award below

            var award = new Award();
            award.Id = row.XPathSelectElement("x:awcode", ns).Value;
            award.Colloquial = row.XPathSelectElement("x:awname", ns).Value;
            award.Organization = row.XPathSelectElement("x:aworg", ns).Value;
            award.Country = row.XPathSelectElement("x:awcountry", ns).Value;
            award.Year = row.XPathSelectElement("x:awyear", ns).Value;
            award.Description = row.XPathSelectElement("x:awexplain", ns).Value;
            award.Notes = row.XPathSelectElement("x:awnotes", ns).Value;

            TryAddRow(award, db.Awards);
        }
    }

}
