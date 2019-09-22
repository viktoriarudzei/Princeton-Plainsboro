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
    public class DoctorsController : Controller
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        // GET: Doctors
        public async Task<IActionResult> Index(string sortOrder, 
                                        string currentFilter,
                                        string searchString,
                                        int? pageNumber)
        {
            //ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            //if (searchString != null)
            //{
            //    pageNumber = 1;
            //}
            //else
            //{
            //    searchString = currentFilter;
            //}

            ViewData["CurrentFilter"] = searchString;

            var doctors = from s in _context.Doctors.Include(d => d.Department)
                          select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                doctors = doctors.Where(s => s.LastName.Contains(searchString)
                                       || s.FirstMidName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    doctors = doctors.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    doctors = doctors.OrderBy(s => s.License);
                    break;
                case "date_desc":
                    doctors = doctors.OrderByDescending(s => s.License);
                    break;
                default:
                    doctors = doctors.OrderBy(s => s.LastName);
                    break;
            }
            //int pageSize = 3;
            //return View(await PaginatedList<Doctor>.CreateAsync(doctors.AsNoTracking(), pageNumber ?? 1, pageSize));
            return View(await doctors.ToListAsync());
        }

		// GET: Doctors/Details/5
		[Route("Doctors/Details/{id}.{format}")]
		public async Task<IActionResult> DetailsJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var doctor = await _context.Doctors
				.Include(d => d.Department)
				.FirstOrDefaultAsync(m => m.DoctorID == id);
			if (doctor == null)
			{
				return NotFound();
			}

			return Ok(doctor);
		}
		public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null)
            {
                return NotFound();
            }

            return View(doctor);
        }

        // GET: Doctors/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID");
            return View();
        }

		// POST: Doctors/Create
		// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
		// more details see http://go.microsoft.com/fwlink/?LinkId=317598.

		[HttpPost]
		[Route("Doctors/Create")]
		public async Task<IActionResult> CreateJson([Bind("DoctorID,LastName,FirstMidName,License,DepartmentID")] Doctor doctor)
		{
			try
			{
				if (ModelState.IsValid)
				{
					_context.Add(doctor);
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
			ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", doctor.DepartmentID);
			return Ok("Everything looks fine");
		}
		//[HttpPost]
		//      [ValidateAntiForgeryToken]
		//      public async Task<IActionResult> Create([Bind("DoctorID,LastName,FirstMidName,License,DepartmentID")] Doctor doctor)
		//      {
		//          try
		//          {
		//              if (ModelState.IsValid)
		//              {
		//                  _context.Add(doctor);
		//                  await _context.SaveChangesAsync();
		//                  return RedirectToAction(nameof(Index));
		//              }
		//          }
		//          catch (DbUpdateException /* ex */)
		//          {
		//              //Log the error (uncomment ex variable name and write a log.
		//              ModelState.AddModelError("", "Unable to save changes. " +
		//                  "Try again, and if the problem persists " +
		//                  "see your system administrator.");
		//          }
		//          ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", doctor.DepartmentID);
		//          return View(doctor);
		//      }

		// GET: Doctors/Edit/5

		[Route("Doctors/Details/{id}.{format}")]
		public async Task<IActionResult> EditJson(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var doctor = await _context.Doctors.FindAsync(id);
			if (doctor == null)
			{
				return NotFound();
			}
			ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", doctor.DepartmentID);
			return Ok(doctor);
		}
		public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", doctor.DepartmentID);
            return View(doctor);
        }

        // POST: Doctors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DoctorID,LastName,FirstMidName,License,DepartmentID")] Doctor doctor)
        {
            if (id != doctor.DoctorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(doctor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DoctorExists(doctor.DoctorID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "DepartmentID", "DepartmentID", doctor.DepartmentID);
            return View(doctor);
        }

		[Route("Doctors/Delete/{id}.{format}")]
		public async Task<IActionResult> DeleteJson(int? id, bool? saveChangesError = false)
		{
			if (id == null)
			{
				return NotFound();
			}

			var doctor = await _context.Doctors
				.Include(d => d.Department)
				.FirstOrDefaultAsync(m => m.DoctorID == id);
			if (doctor == null)
			{
				return NotFound();
			}
			if (saveChangesError.GetValueOrDefault())
			{
				ViewData["ErrorMessage"] =
					"Delete failed. Try again, and if the problem persists " +
					"see your system administrator.";
			}

			return Ok(doctor);
		}
		// GET: Doctors/Delete/5
		public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var doctor = await _context.Doctors
                .Include(d => d.Department)
                .FirstOrDefaultAsync(m => m.DoctorID == id);
            if (doctor == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(doctor);
        }

        // POST: Doctors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Doctors.Remove(doctor);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }

        private bool DoctorExists(int id)
        {
            return _context.Doctors.Any(e => e.DoctorID == id);
        }
    }
}
