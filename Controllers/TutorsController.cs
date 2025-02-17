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
    public class TutorsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TutorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Tutors
        public async Task<IActionResult> Index()
        {
            var tutors = await _unitOfWork.Tutors.GetAllAsync();
            return View(tutors);
        }

        // GET: Tutors/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _unitOfWork.Tutors.GetTutorWithDetailsAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // GET: Tutors/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tutors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FullName,Education,Expertise,TeachingExperience,TutoringType")] Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Tutors.AddAsync(tutor);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tutor);
        }

        // GET: Tutors/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _unitOfWork.Tutors.GetByIdAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }
            return View(tutor);
        }

        // POST: Tutors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FullName,Education,Expertise,TeachingExperience,TutoringType")] Tutor tutor)
        {
            if (id != tutor.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Tutors.UpdateAsync(tutor);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TutorExists(tutor.Id))
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
            return View(tutor);
        }

        // GET: Tutors/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutor = await _unitOfWork.Tutors.GetTutorWithDetailsAsync(id);
            if (tutor == null)
            {
                return NotFound();
            }

            return View(tutor);
        }

        // POST: Tutors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var tutor = await _unitOfWork.Tutors.GetByIdAsync(id);
            if (tutor != null)
            {
                await _unitOfWork.Tutors.DeleteAsync(tutor);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TutorExists(string id)
        {
            var tutor = await _unitOfWork.Tutors.GetByIdAsync(id);
            return tutor != null;
        }
    }
}
