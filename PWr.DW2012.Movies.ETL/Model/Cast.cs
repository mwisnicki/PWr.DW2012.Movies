using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using System.Xml;
using System.Data.Entity.Infrastructure;
using System.Data;

namespace PWr.DW2012.Movies.Model
{
    public class Cast
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Actor Actor { get; set; }
        public RoleType RoleType { get; set; }
        public string Role { get; set; }
        public ISet<Award> Awards { get; set; }
        public string Notes { get; set; }

        public Cast() {
            Awards = new HashSet<Award>();
        }
    }

    public class RoleType
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public ISet<Cast> Casts { get; set; }
    }


    class CastLoader : PWr.DW2012.Movies.Program.TableLoader<Cast, int> {

        public CastLoader(Program session) : base("casts124.xml", session) { }

        protected override int GetKey(Cast row) {
            return row.Id;
        }

        protected override XNode GetTable(XDocument doc) {
            return doc.XPathSelectElement("//casts");
        }

        protected override IEnumerable<XElement> GetRows(XNode table) {
            return table.XPathSelectElements(".//filmc/m");
        }

        protected override void ProcessRow(XElement row, string[] cells) {
            var cast = new Cast();
            var xe = null as XElement;

            var movie = row.XPathSelectElement("f").Value;
            var oMovie = db.Movies.Find(movie);

            var actor = row.XPathSelectElement("a").Value;
            cast.Actor = db.Actors.Find(actor);

            var awards = row.XPathSelectElements("awards/award");
            if (awards != null) {
                foreach (var award in awards) {
                    var oAward = db.Awards.Find(award.Value);
                    cast.Awards.Add(oAward);
                }
            }

            if (oMovie != null) {
                oMovie.Cast.Add(cast);
                db.Cast.Add(cast);
                Context.ObjectStateManager.ChangeRelationshipState(oMovie, cast, m => m.Cast, EntityState.Added);
                
                //TryAddRow(cast, db.Cast);
                OnRowComplete(cast);
            } else {
                OnRowSkipped(cast);
            }

            // FIXME some actors have same names with no way to distinguish them
            // maybe both existing and duplicate rows should be dropped ?
        }
    }
}
