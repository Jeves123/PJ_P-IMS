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
    public class SchedulesController : Controller
    {
        private readonly PJInstallationDbContext _context;

        public SchedulesController(PJInstallationDbContext context)
        {
            _context = context;
        }

        // GET: Schedules
        public async Task<IActionResult> Index()
        {
            var pJInstallationDbContext = _context.Schedules.Include(s => s.Staff);
            return View(await pJInstallationDbContext.ToListAsync());
        }

        // GET: Schedules/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // GET: Schedules/Create
        public IActionResult Create()
        {
            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "StaffId");
            return View();
        }

        // POST: Schedules/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create(DateTime scheduledDate, string taskDescription, int staffId)
        {
            try
            {
                var schedule = new Schedule
                {
                    ScheduledDate = scheduledDate,
                    TaskDescription = taskDescription,
                    StaffId = staffId
                };

                _context.Schedules.Add(schedule);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return Content($"Error: {ex.Message}");
            }
        }

        // GET: Schedules/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule == null)
            {
                return NotFound();
            }
            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "StaffId", schedule.StaffId);
            return View(schedule);
        }

        // POST: Schedules/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        // Option 1: Direct SQL Approach
        [HttpPost]
        public async Task<IActionResult> Edit(int id,
            DateTime scheduledDate,
            string taskDescription,
            int staffId)
        {
            try
            {
                await _context.Database.ExecuteSqlInterpolatedAsync(
                    $@"UPDATE Schedules 
               SET ScheduledDate = {scheduledDate}, 
                   TaskDescription = {taskDescription}, 
                   StaffId = {staffId}
               WHERE ScheduleId = {id}");

                TempData["SuccessMessage"] = "Schedule updated successfully!";
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
        public async Task<IActionResult> Edit(int id, [Bind("ScheduleId,ScheduledDate,TaskDescription,StaffId")] Schedule model)
        {
            if (id != model.ScheduleId)
            {
                return NotFound();
            }

            try
            {
                if (ModelState.IsValid)
                {
                    var schedule = await _context.Schedules.FindAsync(id);
                    if (schedule == null)
                    {
                        return NotFound();
                    }

                    // Manual update of each property
                    schedule.ScheduledDate = model.ScheduledDate;
                    schedule.TaskDescription = model.TaskDescription;
                    schedule.StaffId = model.StaffId;

                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Schedule updated successfully!";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!ScheduleExists(id))
                {
                    return NotFound();
                }
                ModelState.AddModelError("", "Concurrency error. The record was modified by another user.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error updating schedule: {ex.Message}");
            }

            ViewData["StaffId"] = new SelectList(_context.Staffs, "StaffId", "Name", model.StaffId);
            return View(model);
        }


        // GET: Schedules/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schedule = await _context.Schedules
                .Include(s => s.Staff)
                .FirstOrDefaultAsync(m => m.ScheduleId == id);
            if (schedule == null)
            {
                return NotFound();
            }

            return View(schedule);
        }

        // POST: Schedules/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var schedule = await _context.Schedules.FindAsync(id);
            if (schedule != null)
            {
                _context.Schedules.Remove(schedule);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ScheduleExists(int id)
        {
            return _context.Schedules.Any(e => e.ScheduleId == id);
        }
    }
}
