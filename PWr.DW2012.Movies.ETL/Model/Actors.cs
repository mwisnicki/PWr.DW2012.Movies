﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace PWr.DW2012.Movies.Model {
    public class Actor {
        [Key]
        public string StageName { get; set; }
        [Column(TypeName="datetime2")]
        public DateTime? WorkStart { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? WorkEnd { get; set; }
        public string FamilyName { get; set; }
        public string FirstName { get; set; }
        public Gender? Gender { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfBirth { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime? DateOfDeath { get; set; }
        public string Type { get; set; } // TODO split roles
        //public string Origin { get; set; } // Country of birth ?
        public Country Origin { get; set; }
        // pict ignored
        public string Notes { get; set; }
        public string Family { get; set; }
    }

    public enum Gender {
        Unknown,
        Male,
        Female
    }
}
