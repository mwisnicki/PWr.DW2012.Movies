using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using PWr.DW2012.Movies.Model;

namespace PWr.DW2012.Movies {
    class Program {
        static void Main(string[] args) {
            Database.SetInitializer<MoviesContext>(new MoviesContext.DropCreateDatabaseAlways());

            using (var db = new MoviesContext()) {
                db.Database.CreateIfNotExists();
                int recordsAffected = db.SaveChanges();
                Console.WriteLine("Saved {0} entities to the database, press any key to exit.", recordsAffected);
                Console.ReadKey();
            }
        }
    }
}
