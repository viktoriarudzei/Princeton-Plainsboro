﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PrincetonPlainsboro.Data;
using PrincetonPlainsboro.Models;

namespace PrincetonPlainsboro.Controllers
{
    public class PatientsController : Controller
    {
        private readonly HospitalContext _context;

        public PatientsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Patients
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";
            var patients = from s in _context.Patients
                           select s;
            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    patients = patients.OrderBy(s => s.HospitalAdmissionDay);
                    break;
                case "date_desc":
                    patients = patients.OrderByDescending(s => s.HospitalAdmissionDay);
                    break;
                default:
                    patients = patients.OrderBy(s => s.LastName);
                    break;
            }
            return View(await patients.AsNoTracking().ToListAsync());
        }


		[Route("Patients/Details/{id}.{format}")]
		public async Task<IActionResult> DetailsJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patient = await _context.Patients
				.FirstOrDefaultAsync(m => m.PatientID == id);
			if (patient == null)
			{
				return NotFound();
			}

			return Ok(patient);
		}
		// GET: Patients/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        // GET: Patients/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Patients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(
        //    [Bind("PatientID,LastName,FirstMidName,HospitalAdmissionDay")] Patient patient)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(patient);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //    }
        //    catch (DbUpdateException /* ex */)
        //    {
        //        //Log the error (uncomment ex variable name and write a log.
        //        ModelState.AddModelError("", "Unable to save changes. " +
        //            "Try again, and if the problem persists " +
        //            "see your system administrator.");
        //    }
        //    return View(patient);
        //}

		[HttpPost]
		[Route("Doctors/Create")]
		public async Task<IActionResult> CreateJson(
			[Bind("PatientID,LastName,FirstMidName,HospitalAdmissionDay")] Patient patient)
		{
			try
			{
				if (ModelState.IsValid)
				{
					_context.Add(patient);
					await _context.SaveChangesAsync();
					return Ok("Everything looks fine");
				}
			}
			catch (DbUpdateException /* ex */)
			{
				//Log the error (uncomment ex variable name and write a log.
				ModelState.AddModelError("", "Unable to save changes. " +
					"Try again, and if the problem persists " +
					"see your system administrator.");
			}
			return Ok("Everything looks fine");
		}


		[Route("Patients/Edit/{id}.{format}")]
		public async Task<IActionResult> EditJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patient = await _context.Patients.FindAsync(id);
			if (patient == null)
			{
				return NotFound();
			}
			return Ok(patient);
		}
		// GET: Patients/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }
            return View(patient);
        }

        // POST: Patients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PatientID,LastName,FirstMidName,HospitalAdmissionDay")] Patient patient)
        {
            if (id != patient.PatientID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(patient);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PatientExists(patient.PatientID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(patient);
        }


		[Route("Patients/Delete/{id}.{format}")]
		public async Task<IActionResult> DeleteJson(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}

			var patient = await _context.Patients
				.FirstOrDefaultAsync(m => m.PatientID == id);
			if (patient == null)
			{
				return NotFound();
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return Ok(patient);
		}
		// GET: Patients/Delete/5
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patient = await _context.Patients
                .FirstOrDefaultAsync(m => m.PatientID == id);
            if (patient == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(patient);
        }

        // POST: Patients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return RedirectToAction(nameof(Index));
            }
            try
            {
                _context.Patients.Remove(patient);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool PatientExists(int id)
        {
            return _context.Patients.Any(e => e.PatientID == id);
        }
    }
}
