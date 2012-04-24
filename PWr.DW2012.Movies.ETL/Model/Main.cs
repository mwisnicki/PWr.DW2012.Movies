using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model {

    public class MovieMajorSection {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public ISet<MovieMinorSection> MinorSections { get; set; }
    }

    public class MovieMinorSection {
        [Key]
        public string Name { get; set; }
        public string Description { get; set; }
        public ISet<Movie> Movies { get; set; }
    }

    public class Movie {
#if true // temp hack for duped keys
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
#else
        [Key]
#endif
        public string RefName { get; set; }
        public string Title { get; set; }
        public int? Year { get; set; }
#if false
        public Person Director { get; set; }
        /// <summary>
        /// <remarks>
        /// P => Existing Person
        /// PN => Generated Person with Id="PN:name"
        /// PU => null
        /// PZ => Generated Person with Id="PZ:name" and IsApproximate=true
        /// </remarks>
        /// </summary>
        public ISet<Person> Producers { get; set; }
        /// <summary>
        /// <remarks>
        /// S => Existing Studio
        /// St => Generated Studio
        /// SU => null
        /// </remarks>
        /// </summary>
        public ISet<Studio> Studios { get; set; }
        /// <summary>
        /// <remarks>
        /// SD => Existing or generated Studio
        /// </remarks>
        /// </summary>
        public ISet<Studio> Distributors { get; set; }
        /// <summary>
        /// When Studio is SL
        /// XXX is it a production location or a national studio (eg. commie country)
        /// </summary>
        public ISet<Country> Countries { get; set; }
        /// <summary>
        /// <remarks>
        /// XXX Couple of movies have multiple processes, but for now they are ignored
        /// </remarks>
        /// </summary>
        public ProcessCode Process { get; set; }
        public ISet<MovieCategory> Categories { get; set; }
        public ISet<Award> Awards { get; set; }
        public ISet<MovieLocation> Locations { get; set; }
        public DateTime? /*MovieTime*/ LocationTime { get; set; }
        /// <summary>
        /// Anything that's left of last column after parsing
        /// </summary>
        public string NotesRemaining { get; set; }
#endif
        // XXX this really ought to be in separate tables
        #region Notes
#if fale
        public ISet<AlternateTitle> AlternateTitles { get; set; }
        // XXX can be multiple, includes titles
        public string OriginalAuthor { get; set; }
        public Person CoDirector { get; set; }
        public Person Screenwriter { get; set; }
        public string Cinematographer { get; set; }
        public string Choreographer { get; set; }
        public string Music { get; set; }
        public string Editor { get; set; }
        public string ArtsDirector { get; set; }
        public string FilmedAt { get; set; }
        public string Symbology { get; set; }
        public string NotesLang { get; set; }
        public string SerialName { get; set; }
        public string LengthString { get; set; }
        public int? LengthMinutes { get; set; } // computed from above
        public string Rating { get; set; }
        public string IncomeString { get; set; }
        public int? IncomeDollars { get; set; } // computed
#endif
        public string Notes { get; set; } // Nt()
        // ignore: Seen, VT
        #endregion
    }

    public class AlternateTitle {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Reason { get; set; }
    }

    public class MovieLocation {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        /// <summary>
        /// A hierachy like: somewhere, London, England
        /// <remarks>
        /// Could be anything, but usually City, Country
        /// </remarks>
        /// </summary>
        public string Place { get; set; }
    }

    public class MovieTime {
        /// <summary>
        /// Optional time.
        /// <remarks>
        /// Usually a year but sometimes a range, century or epoch or a particular day of year.
        /// </remarks>
        /// </summary>
        public string Time { get; set; }
    }

    public class ProcessCode {
        [Key]
        public string Id { get; set; }

        public static readonly ProcessCode BWS = new ProcessCode { Id = "bws" };
        public static readonly ProcessCode BNW = new ProcessCode { Id = "bnw" };
        public static readonly ProcessCode COL = new ProcessCode { Id = "col" };
        public static readonly ProcessCode COLPROCESS = new ProcessCode { Id = @"\colprocess" };
        public static readonly ProcessCode CLD = new ProcessCode { Id = "cld" };

        public static readonly List<ProcessCode> Values = new List<ProcessCode> {
            BWS, BNW, COL, COLPROCESS, CLD
        };
    }

    public class MovieCategory {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public ISet<Movie> Movies { get; set; }

        public static readonly MovieCategory BioP = new MovieCategory { Id = "BioP", Name = "Biography?" };
        public static readonly MovieCategory Disa = new MovieCategory { Id = "Disa", Name = "Disaster" };
        public static readonly MovieCategory Dram = new MovieCategory { Id = "Dram", Name = "Drama" };
        public static readonly MovieCategory CnR = new MovieCategory { Id = "CnR", Name = "?" };
        public static readonly MovieCategory Comd = new MovieCategory { Id = "Comd", Name = "Comedy" };
        public static readonly MovieCategory Faml = new MovieCategory { Id = "Faml", Name = "Family" };
        public static readonly MovieCategory Hist = new MovieCategory { Id = "Hist", Name = "Historical" };
        public static readonly MovieCategory Horr = new MovieCategory { Id = "Horr", Name = "Horror" };
        public static readonly MovieCategory Musc = new MovieCategory { Id = "Musc", Name = "Music (Musical?)" };
        public static readonly MovieCategory Noir = new MovieCategory { Id = "Noir", Name = "Film Noir" };
        public static readonly MovieCategory Romt = new MovieCategory { Id = "Romt", Name = "Romantic" };
        public static readonly MovieCategory ScFi = new MovieCategory { Id = "ScFi", Name = "Science Fiction" };
        public static readonly MovieCategory Susp = new MovieCategory { Id = "Susp", Name = "Suspense?" };

        public static readonly List<MovieCategory> Values = new List<MovieCategory> {
            BioP, Disa, Dram, CnR, Comd, Faml, Hist, Horr, Musc, Noir, Romt, ScFi, Susp
        };
    }

}
