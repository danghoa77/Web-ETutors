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
using Microsoft.AspNetCore.Authorization;

namespace eTutoring.Controllers
{
    [Authorize] // Meeting records should only be accessible to authenticated users
    public class MeetingRecordsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public MeetingRecordsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: MeetingRecords
        public async Task<IActionResult> Index()
        {
            var meetingRecords = await _unitOfWork.MeetingRecords.GetAllAsync();
            return View(meetingRecords);
        }

        // GET: MeetingRecords/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingRecord = await _unitOfWork.MeetingRecords.GetMeetingRecordWithDetailsAsync(id.Value);
            if (meetingRecord == null)
            {
                return NotFound();
            }

            return View(meetingRecord);
        }

        // GET: MeetingRecords/Create
        public async Task<IActionResult> Create(int? roomId)
        {
            if (roomId.HasValue)
            {
                var room = await _unitOfWork.Rooms.GetByIdAsync(roomId.Value);
                if (room == null)
                {
                    return NotFound();
                }
                ViewData["RoomId"] = roomId.Value;
                ViewData["RoomName"] = room.Name;
            }
            else
            {
                var rooms = await _unitOfWork.Rooms.GetAllAsync();
                ViewData["RoomId"] = new SelectList(rooms, "Id", "Name");
            }
            return View();
        }

        // POST: MeetingRecords/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId,StartTime,EndTime,RecordingLink,GoogleDriveFileId,Notes")] MeetingRecord meetingRecord)
        {
            if (ModelState.IsValid)
            {
                await _unitOfWork.MeetingRecords.AddAsync(meetingRecord);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Rooms", new { id = meetingRecord.RoomId });
            }

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", meetingRecord.RoomId);
            return View(meetingRecord);
        }

        // GET: MeetingRecords/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingRecord = await _unitOfWork.MeetingRecords.GetByIdAsync(id.Value);
            if (meetingRecord == null)
            {
                return NotFound();
            }

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", meetingRecord.RoomId);
            return View(meetingRecord);
        }

        // POST: MeetingRecords/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,RoomId,StartTime,EndTime,RecordingLink,GoogleDriveFileId,Notes")] MeetingRecord meetingRecord)
        {
            if (id != meetingRecord.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.MeetingRecords.UpdateAsync(meetingRecord);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await MeetingRecordExists(meetingRecord.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Rooms", new { id = meetingRecord.RoomId });
            }

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", meetingRecord.RoomId);
            return View(meetingRecord);
        }

        // GET: MeetingRecords/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meetingRecord = await _unitOfWork.MeetingRecords.GetMeetingRecordWithDetailsAsync(id.Value);
            if (meetingRecord == null)
            {
                return NotFound();
            }

            return View(meetingRecord);
        }

        // POST: MeetingRecords/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meetingRecord = await _unitOfWork.MeetingRecords.GetByIdAsync(id);
            if (meetingRecord != null)
            {
                var roomId = meetingRecord.RoomId;
                await _unitOfWork.MeetingRecords.DeleteAsync(meetingRecord);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Rooms", new { id = roomId });
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: MeetingRecords/ByDateRange
        public async Task<IActionResult> ByDateRange(DateTime? startDate, DateTime? endDate)
        {
            if (!startDate.HasValue || !endDate.HasValue)
            {
                return View("Index", await _unitOfWork.MeetingRecords.GetAllAsync());
            }

            var records = await _unitOfWork.MeetingRecords.GetMeetingRecordsByDateRangeAsync(startDate.Value, endDate.Value);
            ViewData["StartDate"] = startDate.Value.ToString("yyyy-MM-dd");
            ViewData["EndDate"] = endDate.Value.ToString("yyyy-MM-dd");
            return View("Index", records);
        }

        // GET: MeetingRecords/ByRoom/5
        public async Task<IActionResult> ByRoom(int roomId)
        {
            var records = await _unitOfWork.MeetingRecords.GetMeetingRecordsByRoomAsync(roomId);
            var room = await _unitOfWork.Rooms.GetByIdAsync(roomId);
            ViewData["RoomName"] = room?.Name;
            ViewData["RoomId"] = roomId;
            return View("Index", records);
        }

        private async Task<bool> MeetingRecordExists(int id)
        {
            var meetingRecord = await _unitOfWork.MeetingRecords.GetByIdAsync(id);
            return meetingRecord != null;
        }
    }
}
