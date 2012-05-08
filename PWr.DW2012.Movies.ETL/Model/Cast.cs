using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace PWr.DW2012.Movies.Model
{
    public class Cast
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public Actor Actor { get; set; }
        public RoleType RoleType { get; set; }
        public string Role { get; set; }
        public ISet<Award> Awards { get; set; }
        public string Notes { get; set; }
    }

    public class RoleType
    {
        [Key]
        public string Id { get; set; }
        public string Description { get; set; }
        public ISet<Cast> Casts { get; set; }
    }
}
