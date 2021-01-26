using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;

namespace MyBlog.Controllers
{
    public class PostCommentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: PostComments
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.PostComment.Include(p => p.BlogUser).Include(p => p.CategoryPost);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: PostComments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComment
                .Include(p => p.BlogUser)
                .Include(p => p.CategoryPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postComment == null)
            {
                return NotFound();
            }

            return View(postComment);
        }

        // GET: PostComments/Create
        public IActionResult Create()
        {
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["CategoryPostId"] = new SelectList(_context.CategoryPost, "Id", "Id");
            return View();
        }

        // POST: PostComments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CategoryPostId,BlogUserId,CommentBody,Created,Updated,Moderated,ModReason,ModBody")] PostComment postComment)
        {
            if (ModelState.IsValid)
            {
                _context.Add(postComment);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", postComment.BlogUserId);
            ViewData["CategoryPostId"] = new SelectList(_context.CategoryPost, "Id", "Id", postComment.CategoryPostId);
            return View(postComment);
        }

        // GET: PostComments/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComment.FindAsync(id);
            if (postComment == null)
            {
                return NotFound();
            }
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", postComment.BlogUserId);
            ViewData["CategoryPostId"] = new SelectList(_context.CategoryPost, "Id", "Id", postComment.CategoryPostId);
            return View(postComment);
        }

        // POST: PostComments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryPostId,BlogUserId,CommentBody,Created,Updated,Moderated,ModReason,ModBody")] PostComment postComment)
        {
            if (id != postComment.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(postComment);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PostCommentExists(postComment.Id))
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
            ViewData["BlogUserId"] = new SelectList(_context.Users, "Id", "Id", postComment.BlogUserId);
            ViewData["CategoryPostId"] = new SelectList(_context.CategoryPost, "Id", "Id", postComment.CategoryPostId);
            return View(postComment);
        }

        // GET: PostComments/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var postComment = await _context.PostComment
                .Include(p => p.BlogUser)
                .Include(p => p.CategoryPost)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (postComment == null)
            {
                return NotFound();
            }

            return View(postComment);
        }

        // POST: PostComments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var postComment = await _context.PostComment.FindAsync(id);
            _context.PostComment.Remove(postComment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PostCommentExists(int id)
        {
            return _context.PostComment.Any(e => e.Id == id);
        }
    }
}
