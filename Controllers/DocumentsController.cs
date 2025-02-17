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
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace eTutoring.Controllers
{
    public class DocumentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public DocumentsController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Documents
        public async Task<IActionResult> Index()
        {
            var documents = await _unitOfWork.Documents.GetAllAsync();
            return View(documents);
        }

        // GET: Documents/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _unitOfWork.Documents.GetDocumentWithDetailsAsync(id.Value);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // GET: Documents/Create
        public async Task<IActionResult> Create()
        {
            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name");
            return View();
        }

        // POST: Documents/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RoomId")] Document document, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                document.FileName = file.FileName;
                document.FilePath = uniqueFileName;
                document.UploadedDate = DateTime.UtcNow;
                document.UploadedBy = User.Identity?.Name ?? "Unknown"; // You might want to get this from your authentication system

                if (ModelState.IsValid)
                {
                    await _unitOfWork.Documents.AddAsync(document);
                    await _unitOfWork.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", document.RoomId);
            return View(document);
        }

        // GET: Documents/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _unitOfWork.Documents.GetByIdAsync(id.Value);
            if (document == null)
            {
                return NotFound();
            }

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", document.RoomId);
            return View(document);
        }

        // POST: Documents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FileName,RoomId")] Document document, IFormFile? file)
        {
            if (id != document.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existingDocument = await _unitOfWork.Documents.GetByIdAsync(id);
                    if (existingDocument == null)
                    {
                        return NotFound();
                    }

                    if (file != null && file.Length > 0)
                    {
                        // Delete old file if it exists
                        var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", existingDocument.FilePath);
                        if (System.IO.File.Exists(oldFilePath))
                        {
                            System.IO.File.Delete(oldFilePath);
                        }

                        // Save new file
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
                        var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                        existingDocument.FileName = file.FileName;
                        existingDocument.FilePath = uniqueFileName;
                    }

                    existingDocument.RoomId = document.RoomId;
                    await _unitOfWork.Documents.UpdateAsync(existingDocument);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await DocumentExists(document.Id))
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

            var rooms = await _unitOfWork.Rooms.GetAllAsync();
            ViewData["RoomId"] = new SelectList(rooms, "Id", "Name", document.RoomId);
            return View(document);
        }

        // GET: Documents/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _unitOfWork.Documents.GetDocumentWithDetailsAsync(id.Value);
            if (document == null)
            {
                return NotFound();
            }

            return View(document);
        }

        // POST: Documents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(id);
            if (document != null)
            {
                // Delete the physical file
                var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.FilePath);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }

                await _unitOfWork.Documents.DeleteAsync(document);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Documents/Download/5
        public async Task<IActionResult> Download(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var document = await _unitOfWork.Documents.GetByIdAsync(id.Value);
            if (document == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", document.FilePath);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
            }
            memory.Position = 0;

            return File(memory, GetContentType(document.FileName), document.FileName);
        }

        private async Task<bool> DocumentExists(int id)
        {
            var document = await _unitOfWork.Documents.GetByIdAsync(id);
            return document != null;
        }

        private string GetContentType(string fileName)
        {
            var ext = Path.GetExtension(fileName).ToLowerInvariant();
            return ext switch
            {
                ".pdf" => "application/pdf",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                ".png" => "image/png",
                ".jpg" => "image/jpeg",
                ".jpeg" => "image/jpeg",
                _ => "application/octet-stream",
            };
        }
    }
}
