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
                new Doctor{DepartmentID=departments.Single(i=>i.Name=="Oncology").DepartmentID,LastName="Wilson",FirstMidName="James",License=DateTime.Parse("2005-09-01")},
                new Doctor{DepartmentID=departments.Single(i=>i.Name=="Diagnostics").DepartmentID,LastName="House",FirstMidName="Gregory",License=DateTime.Parse("2005-09-01")},
                new Doctor{DepartmentID=departments.Single(i=>i.Name=="Diagnostics").DepartmentID,LastName="Chase",FirstMidName="Robert",License=DateTime.Parse("2009-09-01")}
            };
            foreach(Doctor doc in doctors)
            {
                context.Doctors.Add(doc);
            }
            context.SaveChanges();

            var patients = new Patient[]
            {
                new Patient { FirstMidName = "Carson",   LastName = "Alexander",
                    HospitalAdmissionDay = DateTime.Parse("2010-09-01") },
                new Patient { FirstMidName = "Meredith", LastName = "Alonso",
                    HospitalAdmissionDay = DateTime.Parse("2012-09-01") },
                new Patient { FirstMidName = "Arturo",   LastName = "Anand",
                    HospitalAdmissionDay = DateTime.Parse("2013-09-01") },
                new Patient { FirstMidName = "Gytis",    LastName = "Barzdukas",
                    HospitalAdmissionDay = DateTime.Parse("2012-09-01") }
            };
            foreach (Patient p in patients)
            {
                context.Patients.Add(p);
            }
            context.SaveChanges();

            var cases = new Case[]
            {
                new Case{DoctorId=doctors.Single(i=>i.LastName=="Wilson").DoctorID,PatientId=patients.Single(i=>i.LastName=="Alexander").PatientID,Name="Case 12",Emergency="high key important",Complete="yes"},
                new Case{DoctorId=doctors.Single(i=>i.LastName=="House").DoctorID,PatientId=patients.Single(i=>i.LastName=="Alonso").PatientID,Name="Case 15",Emergency="high key important",Complete="yes"},
                new Case{DoctorId=doctors.Single(i=>i.LastName=="House").DoctorID,PatientId=patients.Single(i=>i.LastName=="Anand").PatientID,Name="Case 13",Emergency="important",Complete="no"},
                new Case{DoctorId=doctors.Single(i=>i.LastName=="Chase").DoctorID,PatientId=patients.Single(i=>i.LastName=="Barzdukas").PatientID,Name="Case 14",Emergency="low key important",Complete="yes"},
            };
            foreach (Case cas in cases)
            {
                context.Cases.Add(cas);
            }
            context.SaveChanges();


            var diagnoses = new Diagnose[]
            {
                new Diagnose{PatientId=patients.Single(i=>i.LastName=="Alexander").PatientID,Name="Cancer", Description="4th stat", Treatment="chemioterapy"},
                new Diagnose{PatientId=patients.Single(i=>i.LastName=="Alonso").PatientID,Name="Volchanka", Description="bad", Treatment="steroids"},
                new Diagnose{PatientId=patients.Single(i=>i.LastName=="Anand").PatientID,Name="Cancer", Description="3th stat", Treatment="chemioterapy"},
                new Diagnose{PatientId=patients.Single(i=>i.LastName=="Barzdukas").PatientID,Name="Gentington", Description="bad", Treatment="no treatment"}

            };
            foreach (Diagnose d in diagnoses)
            {
                context.Diagnoses.Add(d);
            }
            context.SaveChanges();
        }
    }
}
