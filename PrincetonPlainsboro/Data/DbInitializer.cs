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
                new Department{Name="Oncology"},
                new Department{Name="Diagnostics"}
            };
            foreach(Department de in departments)
            {
                context.Departments.Add(de);
            }
            context.SaveChanges();

            var doctors = new Doctor[]
            {
                new Doctor{DepartmentsId=1,LastName="Wilson",FirstMidName="James",License=DateTime.Parse("2005-09-01")},
                new Doctor{DepartmentsId=2,LastName="House",FirstMidName="Gregory",License=DateTime.Parse("2005-09-01")},
                new Doctor{DepartmentsId=2,LastName="Chase",FirstMidName="Robert",License=DateTime.Parse("2009-09-01")}
            };
            foreach(Doctor doc in doctors)
            {
                context.Doctors.Add(doc);
            }
            context.SaveChanges();

            var cases = new Case[]
            {
                new Case{DoctorId=100,PatientId=305,Name="Case 12",Emergency="high key important",Complete="yes"},
                new Case{DoctorId=100,PatientId=306,Name="Case 12",Emergency="high key important",Complete="yes"},
                new Case{DoctorId=101,PatientId=308,Name="Case 13",Emergency="important",Complete="no"},
                new Case{DoctorId=102,PatientId=309,Name="Case 14",Emergency="low key important",Complete="yes"},
            };
            foreach (Case cas in cases)
            {
                context.Cases.Add(cas);
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
            var diagnoses = new Diagnose[]
            {
                new Diagnose{Name="Cancer", Description="4th stat", Treatment="chemioterapy"},

            };
            foreach (Diagnose d in diagnoses)
            {
                context.Diagnoses.Add(d);
            }
            context.SaveChanges();
        }
    }
}
