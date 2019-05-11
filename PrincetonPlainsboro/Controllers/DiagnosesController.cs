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
    public class DiagnosesController : Controller
    {
        private readonly HospitalContext _context;

        public DiagnosesController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Diagnoses
        public async Task<IActionResult> Index()
        {
            var hospitalContext = _context.Diagnoses.Include(d => d.Patient);
            return View(await hospitalContext.ToListAsync());
        }

        // GET: Diagnoses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnose = await _context.Diagnoses
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DiagnoseID == id);
            if (diagnose == null)
            {
                return NotFound();
            }

            return View(diagnose);
        }

        // GET: Diagnoses/Create
        public IActionResult Create()
        {
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID");
            return View();
        }

        // POST: Diagnoses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DiagnoseID,Name,Description,Treatment,PatientId")] Diagnose diagnose)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(diagnose);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.
                ModelState.AddModelError("", "Unable to save changes. " +
                    "Try again, and if the problem persists " +
                    "see your system administrator.");
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", diagnose.PatientId);
            return View(diagnose);
        }

        // GET: Diagnoses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnose = await _context.Diagnoses.FindAsync(id);
            if (diagnose == null)
            {
                return NotFound();
            }
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", diagnose.PatientId);
            return View(diagnose);
        }

        // POST: Diagnoses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DiagnoseID,Name,Description,Treatment,PatientId")] Diagnose diagnose)
        {
            if (id != diagnose.DiagnoseID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(diagnose);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DiagnoseExists(diagnose.DiagnoseID))
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
            ViewData["PatientId"] = new SelectList(_context.Patients, "PatientID", "PatientID", diagnose.PatientId);
            return View(diagnose);
        }

        // GET: Diagnoses/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var diagnose = await _context.Diagnoses
                .Include(d => d.Patient)
                .FirstOrDefaultAsync(m => m.DiagnoseID == id);
            if (diagnose == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(diagnose);
        }

        // POST: Diagnoses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var diagnose = await _context.Diagnoses.FindAsync(id);
            if (diagnose == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Diagnoses.Remove(diagnose);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DiagnoseExists(int id)
        {
            return _context.Diagnoses.Any(e => e.DiagnoseID == id);
        }
    }
}
