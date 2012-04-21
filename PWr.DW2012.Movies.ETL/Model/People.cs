﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model {

    public class Person {
        public string Id { get; set; } // Ref_name

        public bool IsDirector { get; set; }
        public bool IsProducer { get; set; }
        public bool IsWriter { get; set; }
        public bool IsActor { get; set; }
        public bool IsCinematographer { get; set; }
        public bool IsArtDirector { get; set; }
        public bool IsComposer { get; set; }
        public bool IsChoreographer { get; set; }
        public bool IsBookAuthor { get; set; }

        public string Code { get; set; } // also an Id

        // Years are converted as YYYY-01-01 00:00

        public DateTime YearStart { get; set; }
        public DateTime YearEnd { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfDeath { get; set; }

        public string Country { get; set; } // using non-standard codes (bkgd=background?)
        public string Notes { get; set; } // for now unparsed, format seems to be shared with Movie.Notes

        /// <summary>
        /// Set when generated by PZ reference in Movie.Producer
        /// </summary>
        public bool IsApproximate { get; set; }
        /// <summary>
        /// Set when generated by PN or PZ reference in Movie.Producer
        /// </summary>
        public bool IsGenerated { get; set; }
    }

}