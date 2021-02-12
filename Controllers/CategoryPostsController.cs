using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MyBlog.Data;
using MyBlog.Models;
using MyBlog.Services;

namespace MyBlog.Controllers
{
    public class CategoryPostsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISlugService _slugService;
        private readonly IImageService _imageService;

        
        public CategoryPostsController(
            ApplicationDbContext context,
            ISlugService slugService,
            IImageService imageService)
        {
            _context = context;
            _slugService = slugService;
            _imageService = imageService;
        }

        // GET: CategoryPosts
        public async Task<IActionResult> Index(int? pageNumber, string searchString)
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";
            ViewData["SearchString"] = searchString;
            //I want to look at the incoming pageNumber variable and either use it or force it to be 1 and then use 1
            pageNumber = pageNumber == null || pageNumber <= 0 ? 1 : pageNumber;
            ViewData["PageNumber"] = pageNumber;

            //Define an arbitrary page size
            int pageSize = 3, ttlRecords = 0, ttlPages = 0;

            IQueryable<CategoryPost> result = null;
            if(!string.IsNullOrEmpty(searchString))
            {
                result = _context.CategoryPost.AsQueryable();
                searchString = searchString.ToLower();

                result = result.Where(p => p.Title.ToLower().Contains(searchString) ||
                                      p.PostBody.ToLower().Contains(searchString) ||
                                      p.Abstract.ToLower().Contains(searchString) ||
                                      p.PostComments.Any(c => c.CommentBody.ToLower().Contains(searchString) ||
                                                         c.BlogUser.FirstName.ToLower().Contains(searchString) ||
                                                         c.BlogUser.LastName.ToLower().Contains(searchString) ||
                                                         c.BlogUser.DisplayName.ToLower().Contains(searchString) ||
                                                         c.BlogUser.Email.ToLower().Contains(searchString)));

                //Once this LINQ query executes I can ask how many records there are and then I can determine how many
                //pages there are
                ttlRecords = (await result.ToListAsync()).Count;
            }
            else
            {
                result = _context.CategoryPost.AsQueryable();
                ttlRecords = (await result.ToListAsync()).Count;
            }

            if(ttlRecords == 0)
            {
                ttlPages = ttlRecords;
            }
            else if((ttlRecords % pageSize) > 0)
            {
                ttlPages = Convert.ToInt32(ttlRecords / pageSize) + 1;
            }
            else
            {
                ttlPages = Convert.ToInt32(ttlRecords / pageSize);
            }

            ViewData["TtlPages"] = ttlPages;

            pageNumber = pageNumber > ttlPages ? ttlPages : pageNumber;

            if(ttlPages > 0)
            {
                ViewData["PageXofY"] = $"Page {pageNumber} of {ttlPages}";
            }
            else
            {
                ViewData["PageXofY"] = $"Your search string yielded no results";
            }

            var skipCount = ((int)pageNumber - 1) * pageSize;

            //Define a sentence telling the User what page their on
            //ViewData["Page"] = $"You are viewing page [pageNumber} of {}"
            var posts = (await result.OrderByDescending(p => p.Created).ToListAsync()).Skip(skipCount).Take(pageSize);
            return View(posts);
            //var applicationDbContext = _context.CategoryPost.Include(c => c.BlogCategory);
           
            //return View(await applicationDbContext.ToListAsync());
        }
        //Just show the blog posts for a given category.
        public IActionResult CategoryIndex(int? id)
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";

            if (id == null)
            {
                return NotFound();
            }

            //Write a Linq statement that uses the Id to get all of the Blog Posts with the Category Id Fk = id
            var posts = _context.CategoryPost.Where(cp => cp.BlogCategoryId == id).Include(c => c.BlogCategory).ToList();

            //Once I have my Blog posts I want to display them in the Index View
            return View("Index", posts);
        }
        // GET: CategoryPosts/Details/

        public async Task<IActionResult> Details(string slug)
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";

            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name");


            var categoryPost = await _context.CategoryPost
                .Include(c => c.BlogCategory)
                .Include(c => c.PostComments)
                .ThenInclude(p => p.BlogUser)
                .FirstOrDefaultAsync(mbox => mbox.Slug == slug);
            if (categoryPost == null)
            {
                return NotFound();
            }
            return View(categoryPost);
        }

        //Here is the old code
        //public async Task<IActionResult> Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var categoryPost = await _context.CategoryPost
        //        .Include(c => c.BlogCategory)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (categoryPost == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(categoryPost);
        //}


        // GET: CategoryPosts/Create
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";

            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name");
            return View();
        }

        // POST: CategoryPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,BlogCategoryId,Title,Abstract,PostBody,IsReady")] CategoryPost categoryPost, IFormFile formFile)
        {

            if (ModelState.IsValid)
            {
                categoryPost.Created = DateTime.Now;

                categoryPost.ContentType = _imageService.RecordContentType(formFile);
                categoryPost.ImageData = await _imageService.EncodeFileAsync(formFile);
                
                //using slugs
                var slug = _slugService.URLFriendly(categoryPost.Title);
                if(_slugService.Isunique(_context, slug))
                {
                    categoryPost.Slug = slug;
                }
                else
                {
                    ModelState.AddModelError("Title", "This Title has a duplicate slug!");
                    ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name");
                    return View(categoryPost);
                     
                }
                _context.Add(categoryPost);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));            
            }
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name", categoryPost.BlogCategoryId);
            return View(categoryPost);
        }
        [Authorize(Roles = "Administrator")]
        // GET: CategoryPosts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";

            if (id == null)
            {
                return NotFound();
            }

            var categoryPost = await _context.CategoryPost.FindAsync(id);
            if (categoryPost == null)
            {
                return NotFound();
            }
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name", categoryPost.BlogCategoryId);
            return View(categoryPost);
        }

        // POST: CategoryPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,BlogCategoryId,Title,Abstract,PostBody,IsReady,Created, Slug, ImageData, ContentType")] CategoryPost categoryPost, IFormFile formFile)
        {
            if (id != categoryPost.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if(formFile != null)
                    {
                        categoryPost.ContentType = _imageService.RecordContentType(formFile);
                        categoryPost.ImageData = await _imageService.EncodeFileAsync(formFile);
                    }

                    var slug = _slugService.URLFriendly(categoryPost.Title);
                    if (slug != categoryPost.Slug)
                    {
                        if (_slugService.Isunique(_context, slug))
                        {
                            categoryPost.Slug = slug;
                        }
                        else
                        {
                            ModelState.AddModelError("Title", "This Title has a duplicate slug!");
                            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name");
                            return View(categoryPost);

                        }

                    }
                    categoryPost.Updated = DateTime.Now;
                    _context.Update(categoryPost);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryPostExists(categoryPost.Id))
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
            ViewData["BlogCategoryId"] = new SelectList(_context.BlogCategory, "Id", "Name", categoryPost.BlogCategoryId);
            return View(categoryPost);
        }
        [Authorize(Roles = "Administrator")]
        // GET: CategoryPosts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            ViewData["HeaderImage"] = "/Img/photobg.jpg";
            ViewData["HeaderText"] = "Blog Posts";

            if (id == null)
            {
                return NotFound();
            }

            var categoryPost = await _context.CategoryPost
                .Include(c => c.BlogCategory)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (categoryPost == null)
            {
                return NotFound();
            }

            return View(categoryPost);
        }
        [Authorize(Roles = "Administrator")]
        // POST: CategoryPosts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var categoryPost = await _context.CategoryPost.FindAsync(id);
            _context.CategoryPost.Remove(categoryPost);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryPostExists(int id)
        {
            return _context.CategoryPost.Any(e => e.Id == id);
        }
    }
}
