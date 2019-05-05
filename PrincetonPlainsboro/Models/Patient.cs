using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace PrincetonPlainsboro.Models
{
    public class Patient
    {
        public int ID { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string LastName { get; set; }

        [StringLength(50)]
        [RegularExpression(@"^[A-Z]+[a-zA-Z""'\s-]*$")]
        public string FirstMidName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime HospitalAdmissionDay { get; set; }

        public ICollection<Diagnose> Diagnoses { get; set; }
        public int CaseId { get; set; }
        public Case Case { get; set; }
    }
}
