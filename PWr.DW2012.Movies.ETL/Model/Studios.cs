using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

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

        public DateTime First { get; set; }
        public DateTime? Last { get; set; }

        public string Founder { get; set; }
        public Studio Successor { get; set; }
        public string Notes { get; set; }
    }

    // Production Information table ?

}
