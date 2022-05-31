using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ReclutandoMutantes.Models
{
    public class ADN
    {
        [Required]
        public string[] dna { get; set; }
    }
}
