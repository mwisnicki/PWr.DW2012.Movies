using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Xml.XPath;
using PWr.DW2012.Movies.Model;

namespace PWr.DW2012.Movies {
    class Program {
        [STAThread]
        static void Main(string[] args) {
            Database.SetInitializer<MoviesContext>(new MoviesContext.DropCreateDatabaseAlways());
            new Program().Run();
        }

        DirectoryInfo dataDir;
        MoviesContext db;
        TextWriter log = Console.Out;

        void Run() {
            dataDir = new DirectoryInfo("../../../Data/Clean");
            Debug.Assert(dataDir.Exists);

            using (db = new MoviesContext()) {
                log.Write("Create database: ");
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Database.CreateIfNotExists();
                db.SaveChanges();
                log.WriteLine("ok");

                LoadData();

                int recordsAffected = db.SaveChanges();
                Console.WriteLine("Saved {0} entities to the database, press any key to exit.", recordsAffected);
                Console.ReadKey();
            }
        }

        void LoadData() {
            LoadAwards();
        }

        void LoadAwards() {
            var doc = LoadHtml("awtypes.htm");
            var table = doc.XPathSelectElement("//table[normalize-space(caption/text())='Award Givers']");

            log.Write("Adding awards: ");
            foreach (var row in table.XPathSelectElements(".//tr")) {
                if (IsTableHeader(row))
                    continue;
                var cells = row.XPathSelectElements(".//td").Select(td => td.Value.Trim()).ToArray();
                var r = new string[6]; // see award below
                var i = 0;

                while (i < cells.Length && i < r.Length && cells[i] != "|")
                {
                    if (i < 4 && IsDateLike(cells[i]))
                        i = 4;
                    r[i] = cells[i];
                    i++;
                }

                var award = new Award
                {
                    Id = r[0],
                    Organization = r[1],
                    Country = r[2],
                    Colloquial = r[3],
                    Year = r[4],
                    Notes = r[5]
                };

                db.Awards.Add(award);
                log.Write(".");
            }
            log.WriteLine("ok");
        }

        bool IsTableHeader(XElement row)
        {
            return row.Descendants("th").FirstOrDefault() != null;
        }

        static readonly Regex ReDateLike = new Regex(@"(1[789]|2)[0-9][0-9]");
        bool IsDateLike(string text)
        {
            return ReDateLike.IsMatch(text);
        }

        private XDocument LoadHtml(string fileName)
        {
            var path = Path.Combine(dataDir.FullName, fileName);
            var html = File.ReadAllText(path);
            var doc = XDocument.Load(path);
            return doc;
        }
    }
}
