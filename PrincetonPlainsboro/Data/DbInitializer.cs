using PrincetonPlainsboro.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PrincetonPlainsboro.Data
{
    public static class DbInitializer
    {
        public static void Initialize(HospitalContext context)
        {
            context.Database.EnsureCreated();
            //look for patients.
            if (context.Departments.Any())
            {
                return;
            }

            var departments = new Department[]
            {
                new Department{}
            };
            foreach(Department de in departments)
            {
                context.Departments.Add(de);
            }
            context.SaveChanges();

            var patients = new Patient[]
            {
                new Patient{FirstMidName="Carson",LastName="Alexander",HospitalAdmissionDay=DateTime.Parse("2005-09-01")},
                new Patient{FirstMidName="Meredith",LastName="Alonso",HospitalAdmissionDay=DateTime.Parse("2002-09-01")},
                new Patient{FirstMidName="Arturo",LastName="Anand",HospitalAdmissionDay=DateTime.Parse("2003-09-01")},
                new Patient{FirstMidName="Gytis",LastName="Barzdukas",HospitalAdmissionDay=DateTime.Parse("2002-09-01")},
                new Patient{FirstMidName="Yan",LastName="Li",HospitalAdmissionDay=DateTime.Parse("2002-09-01")},
                new Patient{FirstMidName="Peggy",LastName="Justice",HospitalAdmissionDay=DateTime.Parse("2001-09-01")},
                new Patient{FirstMidName="Laura",LastName="Norman",HospitalAdmissionDay=DateTime.Parse("2003-09-01")},
                new Patient{FirstMidName="Nino",LastName="Olivetto",HospitalAdmissionDay=DateTime.Parse("2005-09-01")}

            };
            foreach(Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();
        }
    }
}
