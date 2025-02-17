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
    public class RoomsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Rooms
        public async Task<IActionResult> Index()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            return View(rooms);
        }

        // GET: Rooms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _unitOfWork.Rooms.GetRoomWithDetailsAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // GET: Rooms/Create
        public async Task<IActionResult> Create()
        {
            var tutors = await _unitOfWork.Tutors.GetAllAsync();
            ViewData["TutorId"] = new SelectList(tutors, "Id", "FullName");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description,MeetingLink,TimeSlot,TutorId")] Room room)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.Rooms.AddAsync(room);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var tutors = await _unitOfWork.Tutors.GetAllAsync();
            ViewData["TutorId"] = new SelectList(tutors, "Id", "FullName", room.TutorId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _unitOfWork.Rooms.GetByIdAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            var tutors = await _unitOfWork.Tutors.GetAllAsync();
            ViewData["TutorId"] = new SelectList(tutors, "Id", "FullName", room.TutorId);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,MeetingLink,TimeSlot,TutorId")] Room room)
        {
            if (id != room.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Rooms.UpdateAsync(room);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await RoomExists(room.Id))
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
            var tutors = await _unitOfWork.Tutors.GetAllAsync();
            ViewData["TutorId"] = new SelectList(tutors, "Id", "FullName", room.TutorId);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var room = await _unitOfWork.Rooms.GetRoomWithDetailsAsync(id.Value);
            if (room == null)
            {
                return NotFound();
            }

            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            if (room != null)
            {
                await _unitOfWork.Rooms.DeleteAsync(room);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RoomExists(int id)
        {
            var room = await _unitOfWork.Rooms.GetByIdAsync(id);
            return room != null;
        }

        // Additional methods for room management
        public async Task<IActionResult> Documents(int id)
        {
            var documents = await _unitOfWork.Rooms.GetRoomDocumentsAsync(id);
            return View(documents);
        }

        public async Task<IActionResult> MeetingRecords(int id)
        {
            var records = await _unitOfWork.Rooms.GetRoomMeetingRecordsAsync(id);
            return View(records);
        }
    }
}
