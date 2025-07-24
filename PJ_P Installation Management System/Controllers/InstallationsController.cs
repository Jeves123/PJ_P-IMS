using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PJ_P_Installation_Management_System.Data;
using PJ_P_Installation_Management_System.Models;

namespace PJ_P_Installation_Management_System.Controllers
{
    public class InstallationsController : Controller
    {
        private readonly PJInstallationDbContext _context;

        public InstallationsController(PJInstallationDbContext context)
        {
            _context = context;
        }

        // GET: Installations
        public async Task<IActionResult> Index()
        {
            var pJInstallationDbContext = _context.Installations.Include(i => i.Staff);
            return View(await pJInstallationDbContext.ToListAsync());
        }

        // GET: Installations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var installation = await _context.Installations
                .Include(i => i.Staff)
                .FirstOrDefaultAsync(m => m.InstallationId == id);
            if (installation == null)
            {
                return NotFound();
            }

            return View(installation);
        }

        // GET: Installations/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "StaffId");
            return View();
        }

        // POST: Installations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(string projectName, string location, DateTime startDate, string status, int staffId)
        {
            try
            {
                var installation = new Installation
                {
                    ProjectName = projectName,
                    Location = location,
                    StartDate = startDate,
                    Status = status,
                    StaffId = staffId
                };

                _context.Installations.Add(installation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // GET: Installations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var installation = await _context.Installations.FindAsync(id);
            if (installation == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "StaffId", installation.StaffId);
            return View(installation);
        }

        // POST: Installations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Option 1: Direct SQL Approach (like your working Purchase version)
        [HttpPost]
        public async Task<IActionResult> Edit(int id,
            string projectName,
            string location,
            DateTime startDate,
            string status,
            int staffId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE Installations 
               SET ProjectName = {projectName}, 
                   Location = {location}, 
                   StartDate = {startDate}, 
                   Status = {status},
                   StaffId = {staffId}
               WHERE InstallationId = {id}");

                TempData["SuccessMessage"] = "Installation updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // Option 2: Improved EF Core Approach
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("InstallationId,ProjectName,Location,StartDate,Status,StaffId")] Installation model)
        {
            if (id != model.InstallationId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var installation = await _context.Installations.FindAsync(id);
                    if (installation == null)
                    {
                        return NotFound();
                    }

                    // Manual update of each property
                    installation.ProjectName = model.ProjectName;
                    installation.Location = model.Location;
                    installation.StartDate = model.StartDate;
                    installation.Status = model.Status;
                    installation.StaffId = model.StaffId;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Installation updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!InstallationExists(id))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "Concurrency error. The record was modified by another user.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating installation: {ex.Message}");
            }

            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "Name", model.StaffId);
            return View(model);
        }

        // GET: Installations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var installation = await _context.Installations
                .Include(i => i.Staff)
                .FirstOrDefaultAsync(m => m.InstallationId == id);
            if (installation == null)
            {
                return NotFound();
            }

            return View(installation);
        }

        // POST: Installations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var installation = await _context.Installations.FindAsync(id);
            if (installation != null)
            {
                _context.Installations.Remove(installation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InstallationExists(int id)
        {
            return _context.Installations.Any(e => e.InstallationId == id);
        }
    }
}
