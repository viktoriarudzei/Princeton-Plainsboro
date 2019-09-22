using System;
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
    public class CasesController : Controller
    {
        private readonly HospitalContext _context;

        public CasesController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Cases
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var hospitalContext = from s in _context.Cases.Include(i => i.Doctor).Include(p => p.Patient)
                                  select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                hospitalContext = hospitalContext.Where(s => s.Name.Contains(searchString));
            }
            return View(await hospitalContext.ToListAsync());
        }


		[Route("Cases/Details/{id}.{format}")]

		public async Task<IActionResult> DetailsJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @case = await _context.Cases
				.Include(i => i.Doctor)
				.Include(i => i.Patient)
				.FirstOrDefaultAsync(m => m.CaseID == id);
			if (@case == null)
			{
				return NotFound();
			}

			return Ok(@case);
		}
		// GET: Cases/Details/5
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases
                .Include(i => i.Doctor)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }

            return View(@case);
        }

        // GET: Cases/Create
        public IActionResult Create()
        {
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID");
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID");
            return View();
        }

        // POST: Cases/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJson([Bind("CaseID,Name,Emergency,Complete,DoctorId,PatientId")] Case @case)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(@case);
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", @case.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", @case.PatientId);
			return Ok("Everything looks fine");
		}

		//[HttpPost]
		//[Route("Cases/Create")]

		//public async Task<IActionResult> Create([Bind("CaseID,Name,Emergency,Complete,DoctorId,PatientId")] Case @case)
		//{
		//	try
		//	{
		//		if (ModelState.IsValid)
		//		{
		//			_context.Add(@case);
		//			await _context.SaveChangesAsync();
		//			return RedirectToAction(nameof(Index));
		//		}
		//	}
		//	catch (DbUpdateException /* ex */)
		//	{
		//		//Log the error (uncomment ex variable name and write a log.
		//		ModelState.AddModelError("", "Unable to save changes. " +
		//			"Try again, and if the problem persists " +
		//			"see your system administrator.");
		//	}
		//	ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", @case.DoctorId);
		//	ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", @case.PatientId);
		//	return View(@case);
		//}

		[Route("Cases/Edit/{id}.{format}")]
		public async Task<IActionResult> EditJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @case = await _context.Cases.FindAsync(id);
			if (@case == null)
			{
				return NotFound();
			}
			ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", @case.DoctorId);
			ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", @case.PatientId);
			return Ok(@case);
		}
		// GET: Cases/Edit/5
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return NotFound();
            }
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", @case.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", @case.PatientId);
            return View(@case);
        }

        // POST: Cases/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CaseID,Name,Emergency,Complete,DoctorId,PatientId")] Case @case)
        {
            if (id != @case.CaseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@case);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CaseExists(@case.CaseID))
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
            ViewData["DoctorId"] = new SelectList(_context.Doctors, "DoctorID", "DoctorID", @case.DoctorId);
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", @case.PatientId);
            return View(@case);
        }


		[Route("Cases/Delete/{id}.{format}")]
		public async Task<IActionResult> DeleteJson(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}

			var @case = await _context.Cases
				.Include(i => i.Doctor)
				.Include(i => i.Patient)
				.FirstOrDefaultAsync(m => m.CaseID == id);
			if (@case == null)
			{
				return NotFound();
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return Ok(@case);
		}
		// GET: Cases/Delete/5
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @case = await _context.Cases
                .Include(i => i.Doctor)
                .Include(i => i.Patient)
                .FirstOrDefaultAsync(m => m.CaseID == id);
            if (@case == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(@case);
        }

        // POST: Cases/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var @case = await _context.Cases.FindAsync(id);
            if (@case == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Cases.Remove(@case);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool CaseExists(int id)
        {
            return _context.Cases.Any(e => e.CaseID == id);
        }
    }
}
