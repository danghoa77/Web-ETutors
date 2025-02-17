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
    public class PostsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Posts
        public async Task<IActionResult> Index()
        {
            var posts = await _unitOfWork.Posts.GetAllAsync();
            return View(posts);
        }

        // GET: Posts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.GetPostWithDetailsAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // GET: Posts/Create
        public async Task<IActionResult> Create(int? blogId)
        {
            if (blogId.HasValue)
            {
                ViewData["BlogId"] = blogId.Value;
                var blog = await _unitOfWork.Blogs.GetByIdAsync(blogId.Value);
                ViewData["BlogTitle"] = blog?.Title;
            }
            else
            {
                var blogs = await _unitOfWork.Blogs.GetAllAsync();
                ViewData["BlogId"] = new SelectList(blogs, "Id", "Title");
            }
            return View();
        }

        // POST: Posts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BlogId,Content")] Post post)
        {
            if (ModelState.IsValid)
            {
                post.Date = DateTime.UtcNow;
                await _unitOfWork.Posts.AddAsync(post);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Blogs", new { id = post.BlogId });
            }

            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            ViewData["BlogId"] = new SelectList(blogs, "Id", "Title", post.BlogId);
            return View(post);
        }

        // GET: Posts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.GetByIdAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            ViewData["BlogId"] = new SelectList(blogs, "Id", "Title", post.BlogId);
            return View(post);
        }

        // POST: Posts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Content,BlogId,Date")] Post post)
        {
            if (id != post.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Posts.UpdateAsync(post);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await PostExists(post.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Blogs", new { id = post.BlogId });
            }

            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            ViewData["BlogId"] = new SelectList(blogs, "Id", "Title", post.BlogId);
            return View(post);
        }

        // GET: Posts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var post = await _unitOfWork.Posts.GetPostWithDetailsAsync(id.Value);
            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // POST: Posts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post != null)
            {
                var blogId = post.BlogId;
                await _unitOfWork.Posts.DeleteAsync(post);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction("Details", "Blogs", new { id = blogId });
            }
            
            return RedirectToAction(nameof(Index));
        }

        // GET: Posts/Comments/5
        public async Task<IActionResult> Comments(int id)
        {
            var comments = await _unitOfWork.Posts.GetPostCommentsAsync(id);
            ViewData["PostId"] = id;
            return View(comments);
        }

        private async Task<bool> PostExists(int id)
        {
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            return post != null;
        }
    }
}
