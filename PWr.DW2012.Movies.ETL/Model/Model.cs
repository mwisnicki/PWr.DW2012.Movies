using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model {

    public class MoviesContext : DbContext {
        public DbSet<MovieMajorSection> MovieMajorSections { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Person> People { get; set; }

        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<ProcessCode> ProcessCodes { get; set; }

        public void InitializeDb() {
            MovieCategory.Values.ForEach(c => MovieCategories.Add(c));
            ProcessCode.Values.ForEach(c => ProcessCodes.Add(c));
        }

        private void Insert<T>(IEnumerable<T> list) {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// All notes seem to have shared format
    /// </summary>
    /// TODO move from Movie.Notes
    public class Notes {
        public string Note { get; set; }
    }

    public class Country {
        [Key]
        public string Id { get; set; }
        public string AlternativeId { get; set; } // Person.Country
        public string Name { get; set; }
    }
}
