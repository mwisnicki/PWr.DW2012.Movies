using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model {

    public class MoviesContext {
        public ISet<MovieMajorSection> MovieMajorSections { get; set; }
        public ISet<Movie> Movies { get; set; }
        public ISet<Person> People { get; set; }

        public void Initialize() {
            // Insert following
            Insert(MovieCategory.Values);
            Insert(ProcessCode.Values);
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
        public string Id { get; set; }
        public string AlternativeId { get; set; } // Person.Country
        public string Name { get; set; }
    }
}
