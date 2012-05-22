using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PWr.DW2012.Movies.Model {

    public class StudioCategory {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; } // generated
        public string Name { get; set; }
    }

    public class Studio {
        [Key]
        public string Name { get; set; }
        public string FullCompanyName { get; set; }
        public string CityLocation { get; set; }
        public string Country { get; set; }

        public string/*DateTime*/ First { get; set; }
        public string/*DateTime?*/ Last { get; set; }

        public string Founder { get; set; }
        public string/*Studio*/ Successor { get; set; }
        public string Notes { get; set; }
    }

    // Production Information table ?

    class StudiosLoader : PWr.DW2012.Movies.Program.TableLoader<Studio, string> {

        public StudiosLoader(Program session) : base("studios.htm", session) { }

        protected override string GetKey(Studio row) {
            return row.Name;
        }

        protected override XNode GetTable(XDocument doc) {
            // multiple tables so I'll just return the body and deal with it in GetRows
            return doc.XPathSelectElement("//body");
        }

        protected override IEnumerable<XElement> GetRows(XNode table) {
            return table.XPathSelectElements(".//table[@class='studio-list']//tr");
        }

        protected override void ProcessRow(XElement row, string[] cells) {
            var r = new string[9]; // see award below
            var i = 0;

            while (i < cells.Length && i < r.Length && cells[i] != "|") {
                r[i] = cells[i];
                i++;
            }

            var award = new Studio {
                Name = r[0],
                FullCompanyName = r[1],
                CityLocation = r[2],
                Country = r[3],
                First = r[4],
                Last = r[5],
                Founder = r[6],
                Successor = r[7],
                Notes = r[8],
            };

            TryAddRow(award, db.Studios);
        }
    }
}
