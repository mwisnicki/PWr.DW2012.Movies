using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
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

        DateTime timeStart;
        DateTime timeEnd;
        TimeSpan time;

        void Run() {
            dataDir = new DirectoryInfo("../../../Data");
            Debug.Assert(dataDir.Exists);

            timeStart = DateTime.Now;
            log.WriteLine("Started at: {0}", timeStart);

            using (db = new MoviesContext()) {
                log.Write("Create database: ");
                db.Configuration.AutoDetectChangesEnabled = false;
                db.Database.CreateIfNotExists();
                log.WriteLine(db.SaveChanges());

                LoadData();
                db.SaveChanges();

                timeEnd = DateTime.Now;
                time = timeEnd - timeStart;
                log.WriteLine("Finished at: {0}", timeEnd);
                log.WriteLine("Took: {0}", time);

                Console.WriteLine("Press any key to exit...");
                Console.ReadKey();
            }
        }

        void LoadData() {
            new AwardsLoader(this).Load();
            new MovieCategoryLoader(this).Load();
            new StudiosLoader(this).Load();
            //new ActorsLoader(this).Load();
            new MoviesLoader(this).Load();
        }

        public abstract class TableLoader<TRow, TKey>
            where TRow : class {

            public string FileName { get; protected set; }
            protected MoviesContext db { get; private set; }
            protected XDocument doc;
            protected XNode table;
            private Program program;
            private TextWriter log;

            public TableLoader(string file, Program session) {
                this.FileName = file;
                this.TableName = typeof(TRow).Name;
                this.program = session;
                this.log = session.log;
                this.db = session.db;
                this.DuplicateKeys = new List<TRow>();
                this.SkippedRows = new List<TRow>();
            }

            public string TableName { get; protected set; }

            protected abstract TKey GetKey(TRow row);

            protected virtual void TryAddRow(TRow row, DbSet<TRow> table) {
                var key = GetKey(row);
                if (key != null) {
                    var existing = table.Find(key);
                    if (existing == null) {
                        table.Add(row);
                        OnRowComplete(row);
                    } else
                        // TODO usually ID is off by 1 and can be guessed by looking at neighouring rows
                        OnRowDuplicate(row, existing);
                } else
                    OnRowSkipped(row);
            }

            protected virtual XNode GetTable(XDocument doc) {
                return doc;
            }

            protected virtual IEnumerable<XElement> GetRows(XNode table) {
                return table.XPathSelectElements(".//tr");
            }

            protected virtual string[] GetCells(XElement row) {
                return row.XPathSelectElements(".//td").Select(td => td.Value.Trim()).ToArray();
            }

            #region Processing

            public void Load() {
                LoadDocument();
                ProcessDocument();
            }

            protected void LoadDocument() {
                doc = LoadHtml(FileName);
            }

            protected void ProcessDocument() {
                table = GetTable(doc);
                ProcessTable();
            }

            protected void ProcessTable() {
                OnTableBegin();
                var saved = 0;
                var n = 0;
                var rows = GetRows(table);
                nTotalRows = rows.Count();
                foreach (var row in rows) {
                    if (IsTableHeader(row)) {
                        nTotalRows--;
                        continue;
                    }

                    var cells = GetCells(row);
                    ProcessRow(row, cells);

                    if (++n % 200 == 0)
                        saved += db.SaveChanges();
                }

                saved += db.SaveChanges();
                OnTableFinished(saved);
            }

            protected abstract void ProcessRow(XElement row, string[] cells);

            #endregion Processing

            #region Reporting

            public List<TRow> DuplicateKeys { get; private set; }
            public List<TRow> SkippedRows { get; private set; }

            const string StatusFmt = "Adding {0}: {1:P} total={2}/{3} ok={4} dup={5} skip={6} parse={7}";

            public void OnTableBegin() {
                log.Write("Adding {0}: ", TableName);
            }

            public void OnTableFinished(int saved) {
                PrintStatus();
                log.WriteLine();
            }

            int nTotalRows;
            int nDoneRows;
            int nOkRows;

            private void PrintStatus() {
                log.Write("\r" + StatusFmt, TableName, (double)nDoneRows / nTotalRows, nDoneRows, nTotalRows,
                    nOkRows, DuplicateKeys.Count, SkippedRows.Count, nParseErrors);
            }

            public void OnRowComplete(TRow added) {
                nDoneRows++;
                nOkRows++;
                PrintStatus();
            }

            public void OnRowDuplicate(TRow duplicate, TRow existing) {
                nDoneRows++;
                DuplicateKeys.Add(duplicate);
                PrintStatus();
            }

            public void OnRowSkipped(TRow skipped) {
                nDoneRows++;
                SkippedRows.Add(skipped);
                PrintStatus();
            }

            #endregion Reporting

            #region Parsing

            int nParseErrors;

            protected DateTime? ParseYear(string p) {
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
                    nParseErrors++; // for now swallow errors
                    return null;
                }
            }

            protected static bool IsTableHeader(XElement row) {
                return row.Descendants("th").FirstOrDefault() != null;
            }

            protected static readonly Regex ReDateLike = new Regex(@"(1[789]|2)[0-9][0-9]");
            protected static bool IsDateLike(string text) {
                return ReDateLike.IsMatch(text);
            }

            protected XDocument LoadHtml(string fileName) {
                var path = Path.Combine(program.dataDir.FullName, fileName);
                var html = File.ReadAllText(path);
                var doc = XDocument.Load(path);
                return doc;
            }

            #endregion Parsing
        }

    }
}
