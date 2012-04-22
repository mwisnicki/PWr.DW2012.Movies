using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model {

    public class Award {
        [Key]
        public string Id { get; set; }
        public string Organization { get; set; }
        public string Country { get; set; }
        public string Colloquial { get; set; }
        public string Year { get; set; }
        public string Notes { get; set; }
    }

}
