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
    public class TutorRequestsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TutorRequestsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: TutorRequests
        public async Task<IActionResult> Index()
        {
            var tutorRequests = await _unitOfWork.TutorRequests.GetAllAsync();
            return View(tutorRequests);
        }

        // GET: TutorRequests/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id.Value);
            if (tutorRequest == null)
            {
                return NotFound();
            }

            return View(tutorRequest);
        }

        // GET: TutorRequests/Create
        public async Task<IActionResult> Create()
        {
            var students = await _unitOfWork.Students.GetAllAsync();
            ViewData["StudentId"] = new SelectList(students, "Id", "FullName");
            return View();
        }

        // POST: TutorRequests/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,Subject,TutoringType")] TutorRequest tutorRequest)
        {
            if (ModelState.IsValid)
            {
                tutorRequest.Status = "Pending";
                tutorRequest.CreatedAt = DateTime.UtcNow;
                
                await _unitOfWork.TutorRequests.AddAsync(tutorRequest);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            var students = await _unitOfWork.Students.GetAllAsync();
            ViewData["StudentId"] = new SelectList(students, "Id", "FullName", tutorRequest.StudentId);
            return View(tutorRequest);
        }

        // GET: TutorRequests/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id.Value);
            if (tutorRequest == null)
            {
                return NotFound();
            }

            var students = await _unitOfWork.Students.GetAllAsync();
            ViewData["StudentId"] = new SelectList(students, "Id", "FullName", tutorRequest.StudentId);
            ViewData["StatusList"] = new SelectList(new[] { "Pending", "Approved", "Rejected" }, tutorRequest.Status);
            
            return View(tutorRequest);
        }

        // POST: TutorRequests/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StudentId,Subject,TutoringType,Status,CreatedAt")] TutorRequest tutorRequest)
        {
            if (id != tutorRequest.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.TutorRequests.UpdateAsync(tutorRequest);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await TutorRequestExists(tutorRequest.Id))
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

            var students = await _unitOfWork.Students.GetAllAsync();
            ViewData["StudentId"] = new SelectList(students, "Id", "FullName", tutorRequest.StudentId);
            ViewData["StatusList"] = new SelectList(new[] { "Pending", "Approved", "Rejected" }, tutorRequest.Status);
            
            return View(tutorRequest);
        }

        // GET: TutorRequests/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id.Value);
            if (tutorRequest == null)
            {
                return NotFound();
            }

            return View(tutorRequest);
        }

        // POST: TutorRequests/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id);
            if (tutorRequest != null)
            {
                await _unitOfWork.TutorRequests.DeleteAsync(tutorRequest);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // POST: TutorRequests/UpdateStatus/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id);
            if (tutorRequest == null)
            {
                return NotFound();
            }

            tutorRequest.Status = status;
            await _unitOfWork.TutorRequests.UpdateAsync(tutorRequest);
            await _unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TutorRequestExists(int id)
        {
            var tutorRequest = await _unitOfWork.TutorRequests.GetByIdAsync(id);
            return tutorRequest != null;
        }
    }
}
