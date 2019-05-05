using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PrincetonPlainsboro.Models;

namespace PrincetonPlainsboro.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options) : base(options)
        {
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Diagnose> Diagnoses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Department>().ToTable("Departament");
            modelBuilder.Entity<Doctor>().ToTable("Doctor");
            modelBuilder.Entity<Case>().ToTable("Case");
            modelBuilder.Entity<Patient>().ToTable("Patient");
            modelBuilder.Entity<Diagnose>().ToTable("Diagnose");

        }
    }
    
}
