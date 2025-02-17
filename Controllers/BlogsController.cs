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
    public class BlogsController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public BlogsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: Blogs
        public async Task<IActionResult> Index()
        {
            var blogs = await _unitOfWork.Blogs.GetAllAsync();
            return View(blogs);
        }

        // GET: Blogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _unitOfWork.Blogs.GetBlogWithDetailsAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // GET: Blogs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Blogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Title,Content,AuthorId")] Blog blog)
        {
            if (ModelState.IsValid)
            {
                blog.PostedDate = DateTime.UtcNow;
                await _unitOfWork.Blogs.AddAsync(blog);
                await _unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blog);
        }

        // GET: Blogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _unitOfWork.Blogs.GetByIdAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }
            return View(blog);
        }

        // POST: Blogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,PostedDate,AuthorId")] Blog blog)
        {
            if (id != blog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _unitOfWork.Blogs.UpdateAsync(blog);
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await BlogExists(blog.Id))
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
            return View(blog);
        }

        // GET: Blogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var blog = await _unitOfWork.Blogs.GetBlogWithDetailsAsync(id.Value);
            if (blog == null)
            {
                return NotFound();
            }

            return View(blog);
        }

        // POST: Blogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            if (blog != null)
            {
                await _unitOfWork.Blogs.DeleteAsync(blog);
                await _unitOfWork.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> BlogExists(int id)
        {
            var blog = await _unitOfWork.Blogs.GetByIdAsync(id);
            return blog != null;
        }

        // Additional methods for blog management
        public async Task<IActionResult> Posts(int id)
        {
            var posts = await _unitOfWork.Blogs.GetBlogPostsAsync(id);
            return View(posts);
        }
    }
}
