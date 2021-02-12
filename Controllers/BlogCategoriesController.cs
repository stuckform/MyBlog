using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    public class BlogCategoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;

        public BlogCategoriesController(ApplicationDbContext context, IImageService imageService)
        {
            _context = context;
            _imageService = imageService;
        }

        // GET: BlogCategories
        public async Task<IActionResult> Index()
        {
            ViewData["HeaderImage"] = "/Img/markus-spiske-unsplash.jpg";
            ViewData["HeaderText"] = "Categories";
            return View(await _context.BlogCategory.ToListAsync());
        }

  
        
        // GET: BlogCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["HeaderImage"] = "/Img/markus-spiske-unsplash.jpg";
            ViewData["HeaderText"] = "Categories";
            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            return View(blogCategory);
        }

        // GET: BlogCategories/Create
        // The Admin is only able to create categories
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["HeaderImage"] = "/Img/markus-spiske-unsplash.jpg";
            ViewData["HeaderText"] = "Create";
            return View();
        }

        // POST: BlogCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description")] BlogCategory blogCategory, IFormFile formFile)
        {
            ViewData["HeaderImage"] = "/Img/markus-spiske-unsplash.jpg";
            ViewData["HeaderText"] = "Create";

            if (ModelState.IsValid)
            {
                blogCategory.Created = DateTime.Now;
                blogCategory.ContentType = _imageService.RecordContentType(formFile);
                blogCategory.ImageData = await _imageService.EncodeFileAsync(formFile);
                _context.Add(blogCategory);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(blogCategory);
        }

        // GET: BlogCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["HeaderImage"] = "/Img/markus-spiske-unsplash.jpg";
            ViewData["HeaderText"] = "Edit";

            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory.FindAsync(id);
            if (blogCategory == null)
            {
                return NotFound();
            }
            return View(blogCategory);
        }

        // POST: BlogCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Created")] BlogCategory blogCategory, IFormFile formFile)
        {
            if (id != blogCategory.Id)
            {
                ViewData["HeaderImage"] = "/Img/karma-talukdar--unsplash.jpg";
                ViewData["HeaderText"] = "Edit";

                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {

                    blogCategory.Updated = DateTime.Now;
                    blogCategory.ContentType = _imageService.RecordContentType(formFile);
                    blogCategory.ImageData = await _imageService.EncodeFileAsync(formFile);
                    _context.Update(blogCategory);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BlogCategoryExists(blogCategory.Id))
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
            return View(blogCategory);
        }

        // GET: BlogCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["HeaderImage"] = "/Img/karma-talukdar--unsplash.jpg";
            ViewData["HeaderText"] = "Delete";

            if (id == null)
            {
                return NotFound();
            }

            var blogCategory = await _context.BlogCategory
                .FirstOrDefaultAsync(m => m.Id == id);
            if (blogCategory == null)
            {
                return NotFound();
            }

            return View(blogCategory);
        }

        // POST: BlogCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            ViewData["HeaderImage"] = "/Img/karma-talukdar--unsplash.jpg";
            ViewData["HeaderText"] = "Delete";
            var blogCategory = await _context.BlogCategory.FindAsync(id);
            _context.BlogCategory.Remove(blogCategory);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BlogCategoryExists(int id)
        {
            return _context.BlogCategory.Any(e => e.Id == id);
        }
    }
}
