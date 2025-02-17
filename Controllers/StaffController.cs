using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using eTutoring.Data;
using eTutoring.Models;
using eTutoring.Repositories;

namespace eTutoring.Controllers
{
    public class StaffController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public StaffController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Staff
        public async Task<IActionResult> Index()
        {
            var staffMembers = await _unitOfWork.Staff.GetAllAsync();
            return View(staffMembers);
        }

        // GET: Staff/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _unitOfWork.Staff.GetStaffWithDetailsAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // GET: Staff/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Staff/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Position")] Staff staff)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Staff.AddAsync(staff);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(staff);
        }

        // GET: Staff/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _unitOfWork.Staff.GetByIdAsync(id);
            if (staff == null)
            {
                return NotFound();
            }
            return View(staff);
        }

        // POST: Staff/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,Name,Position")] Staff staff)
        {
            if (id != staff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Staff.UpdateAsync(staff);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await StaffExists(staff.Id))
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
            return View(staff);
        }

        // GET: Staff/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var staff = await _unitOfWork.Staff.GetStaffWithDetailsAsync(id);
            if (staff == null)
            {
                return NotFound();
            }

            return View(staff);
        }

        // POST: Staff/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var staff = await _unitOfWork.Staff.GetByIdAsync(id);
            if (staff != null)
            {
                await _unitOfWork.Staff.DeleteAsync(staff);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Staff/ByPosition
        public async Task<IActionResult> ByPosition(string position)
        {
            var staffMembers = await _unitOfWork.Staff.GetStaffByPositionAsync(position);
            return View("Index", staffMembers);
        }

        private async Task<bool> StaffExists(string id)
        {
            var staff = await _unitOfWork.Staff.GetByIdAsync(id);
            return staff != null;
        }
    }
}
