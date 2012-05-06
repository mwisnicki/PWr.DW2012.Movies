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
                log.WriteLine(db.SaveChanges());

                LoadData();

                db.SaveChanges();
                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        void LoadData() {
            LoadAwards();
            LoadActors();
            LoadMovies();
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
            log.WriteLine(db.SaveChanges());
        }

        void LoadActors() {
            var doc = LoadHtml("actors.htm");
            var table = doc; //.XPathSelectElement("//table[normalize-space(caption/text())='Award Givers']");
            var saved = 0;
            log.Write("Adding actors: ");
            var n = 0;
            foreach (var row in table.XPathSelectElements(".//tr")) {
                if (IsTableHeader(row))
                    continue;
                var cells = row.XPathSelectElements(".//td").Select(td => td.Value.Trim()).ToArray();
                var r = new string[11];
                var i = 0;
                var j = 0;

                while (i < cells.Length && j < r.Length && cells[i] != "|") {
                    if (i < 1 && IsDateLike(cells[i]))
                        j = 1;
                    r[j] = cells[i];
                    i++;
                    j++;
                }



                var actor = new Actor {
                    StageName = r[0],
                    FamilyName = r[2],
                    FirstName = r[3],
                    /*
                    WorkStart = r[1],
                    WorkEnd = r[1],
                    DateOfBirth = r[5],
                    DateOfDeath = r[6],
                     */
                    Type = r[7],
                    Origin = r[8],
                    Notes = r[9],
                    Family = r[10]
                };

                
                if (r[1] != null && r[1] != "dow") {
                    var cDow = r[1].Split('-');
                    if (cDow[0] != "")
                        actor.WorkStart = ParseYear(cDow[0]);
                    if (cDow.Length > 1 && cDow[1] != "")
                        actor.WorkEnd = ParseYear(cDow[1]);
                }
                if (r[5] != null && IsDateLike(r[5]))
                    actor.DateOfBirth = ParseYear(r[5]);
                if (r[6] != null && IsDateLike(r[6]))
                    actor.DateOfDeath = ParseYear(r[6]);

                switch (r[4]) {
                case "M":
                    actor.Gender = Gender.Male;
                    break;
                case "F":
                    actor.Gender = Gender.Female;
                    break;
                }

                db.Actors.Add(actor);
                log.Write(".");
                if (++n % 200 == 0)
                    saved += db.SaveChanges();
            }
            log.WriteLine(saved + db.SaveChanges());
        }


        void LoadMovies() {
            var s = new Status("Movies", log);

            var doc = LoadHtml("main.htm");
            var table = doc; //.XPathSelectElement("//table[normalize-space(caption/text())='Award Givers']");
            var saved = 0;
            s.BeginTable();
            var n = 0;
            foreach (var row in table.XPathSelectElements(".//tr")) {
                if (IsTableHeader(row))
                    continue;
                var cells = row.XPathSelectElements(".//td").Select(td => td.Value.Trim()).ToArray();
                var r = new string[13];
                var i = 0;
                var j = 0;

                while (i < cells.Length && j < r.Length && cells[i] != "|") {
                    if (i < 2 && IsDateLike(cells[i]))
                        j = 2;
                    r[j] = cells[i];
                    i++;
                    j++;
                }



                var movie = new Movie();
                movie.RefName = r[0];
                movie.Title = r[1];
                var year = 0;
                if (int.TryParse(r[2], out year))
                    movie.Year = year;
                //movie.Director = r[3];
                //movie.Producers.Add(r[4]);
                //movie.Studios.Add(r[5]);
                //movie.Process = r[6];
                //movie.Categories.Add(r[7]);
                //movie.Awards.Add(r[8]);
                //movie.Locations.Add(r[9]);
                movie.Notes = r[10];

                if (movie.RefName != null) {
                    var existing = db.Movies.Find(movie.RefName);
                    if (existing == null) {
                        db.Movies.Add(movie);
                        s.RowComplete(movie);
                    } else
                        // TODO usually ID is off by 1 and can be guessed by looking at neighouring rows
                        s.RowDuplicate(movie.RefName, existing.RefName);
                } else
                    s.RowSkipped(movie);
                
                if (++n % 200 == 0)
                    saved += db.SaveChanges();
            }
            s.FinishTable(saved + db.SaveChanges());
        }

        class Status {
            public string Table { get; private set; }
            private TextWriter log;
            public List<object> DuplicateKeys { get; private set; }
            public List<object> SkippedRows { get; private set; }

            public Status(string table, TextWriter log) {
                this.Table = table;
                this.log = log;
                this.DuplicateKeys = new List<object>();
                this.SkippedRows = new List<object>();
            }

            public void BeginTable() {
                log.Write("Adding {0}: ", Table);
            }

            public void RowComplete(object added) {
                log.Write(".");
            }

            public void RowDuplicate(object duplicateKey, object existingKey) {
                DuplicateKeys.Add(duplicateKey);
                log.Write("D");
            }

            public void RowSkipped(object skipped) {
                SkippedRows.Add(skipped);
                log.Write("!");
            }

            public void FinishTable(int changes) {
                log.WriteLine("{0} [{1} duplicates, {2} skipped]", changes, DuplicateKeys.Count, SkippedRows.Count);
            }
        }

        private DateTime? ParseYear(string p) {
            try {
                p = p.Replace('x', '5');
                var y = int.Parse(p);
                if (y < 200 && y > 180) { // missing last digit
                    y *= 10;
                } else if (y < 1800 && y > 1000) { // second digit is invalid
                    y = 1900 + y % 100;
                }
                return new DateTime(y, 1, 1);
            } catch (Exception e) {
                log.Write("!"); // for now swallow errors
                return null;
            }
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
