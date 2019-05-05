using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PrincetonPlainsboro.Models
{
    public class Department
    {
        public int ID { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string Name { get; set; }

        public ICollection<Doctor> Doctors { get; set; }
    }
}
