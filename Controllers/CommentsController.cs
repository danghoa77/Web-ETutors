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
    public class CommentsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Comments
        public async Task<IActionResult> Index()
        {
            var comments = await _unitOfWork.Comments.GetAllAsync();
            return View(comments);
        }

        // GET: Comments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _unitOfWork.Comments.GetCommentWithDetailsAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            return View(comment);
        }

        // GET: Comments/Create
        public async Task<IActionResult> Create(int? postId)
        {
            if (postId.HasValue)
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(postId.Value);
                if (post == null)
                {
                    return NotFound();
                }
                ViewData["PostId"] = postId.Value;
                ViewData["PostContent"] = post.Content;
                return View();
            }

            return NotFound();
        }

        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PostId,Content,AuthorId")] Comment comment)
        {
            if (ModelState.IsValid)
            {
                comment.Date = DateTime.UtcNow;
                await _unitOfWork.Comments.AddAsync(comment);
                await _unitOfWork.SaveChangesAsync();

                var post = await _unitOfWork.Posts.GetByIdAsync(comment.PostId);
                if (post != null)
                {
                    return RedirectToAction("Details", "Posts", new { id = comment.PostId });
                }
            }
            return View(comment);
        }

        // GET: Comments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _unitOfWork.Comments.GetCommentWithDetailsAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the current user is the author of the comment
            if (comment.AuthorId != User.Identity?.Name)
            {
                return Forbid();
            }

            return View(comment);
        }

        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,PostId,AuthorId,Date")] Comment comment)
        {
            if (id != comment.Id)
            {
                return NotFound();
            }

            // Check if the current user is the author of the comment
            var existingComment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (existingComment?.AuthorId != User.Identity?.Name)
            {
                return Forbid();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Comments.UpdateAsync(comment);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await CommentExists(comment.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { id = comment.PostId });
            }
            return View(comment);
        }

        // GET: Comments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comment = await _unitOfWork.Comments.GetCommentWithDetailsAsync(id.Value);
            if (comment == null)
            {
                return NotFound();
            }

            // Check if the current user is the author of the comment
            if (comment.AuthorId != User.Identity?.Name)
            {
                return Forbid();
            }

            return View(comment);
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment != null)
            {
                // Check if the current user is the author of the comment
                if (comment.AuthorId != User.Identity?.Name)
                {
                    return Forbid();
                }

                var postId = comment.PostId;
                await _unitOfWork.Comments.DeleteAsync(comment);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Posts", new { id = postId });
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Comments/ByAuthor
        public async Task<IActionResult> ByAuthor(string authorId)
        {
            var comments = await _unitOfWork.Comments.GetCommentsByAuthorAsync(authorId);
            return View("Index", comments);
        }

        // GET: Comments/ByPost
        public async Task<IActionResult> ByPost(int postId)
        {
            var comments = await _unitOfWork.Comments.GetCommentsByPostAsync(postId);
            ViewData["PostId"] = postId;
            return View("Index", comments);
        }

        private async Task<bool> CommentExists(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            return comment != null;
        }
    }
}
