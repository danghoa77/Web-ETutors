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
    [Authorize] // Chat messages should only be accessible to authenticated users
    public class ChatMessagesController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatMessagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: ChatMessages
        public async Task<IActionResult> Index()
        {
            var currentUserId = User.Identity?.Name;
            if (currentUserId == null)
            {
                return Challenge();
            }

            var messages = await _unitOfWork.ChatMessages.GetUserMessagesAsync(currentUserId);
            return View(messages);
        }

        // GET: ChatMessages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMessage = await _unitOfWork.ChatMessages.GetByIdAsync(id.Value);
            if (chatMessage == null)
            {
                return NotFound();
            }

            // Only allow sender and receiver to view the message
            var currentUserId = User.Identity?.Name;
            if (currentUserId != chatMessage.SenderId && currentUserId != chatMessage.ReceiverId)
            {
                return Forbid();
            }

            return View(chatMessage);
        }

        // GET: ChatMessages/Create
        public async Task<IActionResult> Create(string? receiverId)
        {
            if (receiverId != null)
            {
                ViewData["ReceiverId"] = receiverId;
                // You might want to get receiver details to display in the view
                var receiver = await _unitOfWork.Students.GetByIdAsync(receiverId) 
                             ?? await _unitOfWork.Tutors.GetByIdAsync(receiverId) as dynamic;
                ViewData["ReceiverName"] = receiver?.FullName;
            }
            return View();
        }

        // POST: ChatMessages/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ReceiverId,Content")] ChatMessage chatMessage)
        {
            if (ModelState.IsValid)
            {
                chatMessage.SenderId = User.Identity?.Name ?? throw new InvalidOperationException("User not authenticated");
                chatMessage.SentDate = DateTime.UtcNow;

                await _unitOfWork.ChatMessages.AddAsync(chatMessage);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Conversation), new { userId = chatMessage.ReceiverId });
            }
            return View(chatMessage);
        }

        // GET: ChatMessages/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMessage = await _unitOfWork.ChatMessages.GetByIdAsync(id.Value);
            if (chatMessage == null)
            {
                return NotFound();
            }

            // Only allow sender to edit the message
            if (chatMessage.SenderId != User.Identity?.Name)
            {
                return Forbid();
            }

            return View(chatMessage);
        }

        // POST: ChatMessages/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,SenderId,ReceiverId,SentDate")] ChatMessage chatMessage)
        {
            if (id != chatMessage.Id)
            {
                return NotFound();
            }

            // Only allow sender to edit the message
            if (chatMessage.SenderId != User.Identity?.Name)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.ChatMessages.UpdateAsync(chatMessage);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await ChatMessageExists(chatMessage.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Conversation), new { userId = chatMessage.ReceiverId });
            }
            return View(chatMessage);
        }

        // GET: ChatMessages/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chatMessage = await _unitOfWork.ChatMessages.GetByIdAsync(id.Value);
            if (chatMessage == null)
            {
                return NotFound();
            }

            // Only allow sender to delete the message
            if (chatMessage.SenderId != User.Identity?.Name)
            {
                return Forbid();
            }

            return View(chatMessage);
        }

        // POST: ChatMessages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var chatMessage = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            if (chatMessage != null)
            {
                // Only allow sender to delete the message
                if (chatMessage.SenderId != User.Identity?.Name)
                {
                    return Forbid();
                }

                var receiverId = chatMessage.ReceiverId;
                await _unitOfWork.ChatMessages.DeleteAsync(chatMessage);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Conversation), new { userId = receiverId });
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: ChatMessages/Conversation/userId
        public async Task<IActionResult> Conversation(string userId)
        {
            var currentUserId = User.Identity?.Name;
            if (currentUserId == null)
            {
                return Challenge();
            }

            var messages = await _unitOfWork.ChatMessages.GetMessagesBetweenUsersAsync(currentUserId, userId);
            ViewData["UserId"] = userId;
            // You might want to get user details to display in the view
            var user = await _unitOfWork.Students.GetByIdAsync(userId) 
                      ?? await _unitOfWork.Tutors.GetByIdAsync(userId) as dynamic;
            ViewData["UserName"] = user?.FullName;
            
            return View(messages);
        }

        // GET: ChatMessages/Recent
        public async Task<IActionResult> Recent(int count = 10)
        {
            var currentUserId = User.Identity?.Name;
            if (currentUserId == null)
            {
                return Challenge();
            }

            var messages = await _unitOfWork.ChatMessages.GetRecentMessagesAsync(currentUserId, count);
            return View(messages);
        }

        private async Task<bool> ChatMessageExists(int id)
        {
            var chatMessage = await _unitOfWork.ChatMessages.GetByIdAsync(id);
            return chatMessage != null;
        }
    }
}
