using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrincetonPlainsboro.Models
{
    public class Diagnose
    {
        public int DiagnoseID { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Treatment { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
    }
}
