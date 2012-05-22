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
    public class Actor {
        [Key]
        public string StageName { get; set; }
        [Column(TypeName="datetime2")]
        public DateTime? WorkStart { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? WorkEnd { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public Gender? Gender { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfDeath { get; set; }
        public string Type { get; set; } // TODO split roles
        //public string Origin { get; set; } // Country of birth ?
        public Country Origin { get; set; }
        // pict ignored
        public string Notes { get; set; }
        public string Family { get; set; }
    }

    public enum Gender {
        Unknown,
        Male,
        Female
    }


    class ActorsLoader : PWr.DW2012.Movies.Program.TableLoader<Actor, string> {

        public ActorsLoader(Program session) : base("actors63.xml", session) { }

        protected override string GetKey(Actor row) {
            return row.StageName;
        }

        protected override XNode GetTable(XDocument doc) {
            return doc.XPathSelectElement("//actors");
        }

        protected override IEnumerable<XElement> GetRows(XNode table) {
            return table.XPathSelectElements(".//actor");
        }

        protected override void ProcessRow(XElement row, string[] cells) {
            var r = new string[11];

            var actor = new Actor();
            var xe = null as XElement;

            actor.StageName = row.XPathSelectElement("stagename").Value;
            xe = row.XPathSelectElement("familyname");
            if (xe != null)
                actor.FamilyName = xe.Value;
            actor.FirstName = row.XPathSelectElement("firstname").Value;
            xe = row.XPathSelectElement("roletype");
            if (xe != null)
                actor.Type = xe.Value;
            actor.Notes = row.XPathSelectElement("notes").Value;

            actor.WorkStart = ParseYear(row.XPathSelectElement("dowstart").Value);
            xe = row.XPathSelectElement("dowend");
            if (xe != null)
                actor.WorkEnd = ParseYear(xe.Value);

            xe = row.XPathSelectElement("dob");
            if (xe != null && IsDateLike(xe.Value))
                actor.DateOfBirth = ParseYear(xe.Value);
            xe = row.XPathSelectElement("dod");
            if (xe != null && IsDateLike(xe.Value))
                actor.DateOfDeath = ParseYear(xe.Value);

            xe = row.XPathSelectElement("gender");
            if (xe != null) {
                switch (xe.Value) {
                case "M":
                    actor.Gender = Gender.Male;
                    break;
                case "F":
                    actor.Gender = Gender.Female;
                    break;
                }
            }

            TryAddRow(actor, db.Actors);
            // FIXME some actors have same names with no way to distinguish them
            // maybe both existing and duplicate rows should be dropped ?
        }
    }
}
