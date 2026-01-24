using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.DTOs.Blog;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;
using System.Security.Claims;

namespace Online_Learning_Platform_Ass1.Web.Controllers;

[Authorize]
public class BlogController : Controller
{
    private readonly IBlogService _blogService;

    public BlogController(IBlogService blogService)
    {
        _blogService = blogService;
    }

    // GET: Blog/Index - List all published blogs (public)
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        var result = await _blogService.GetAllPublishedBlogsAsync();
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<BlogReadDto>());
        }

        return View(result.Data);
    }

    // GET: Blog/Details/5 - View blog details (public)
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var result = await _blogService.GetBlogByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(Index));
        }

        return View(result.Data);
    }

    // GET: Blog/MyBlogs - List user's own blogs (Teacher/Admin only)
    public async Task<IActionResult> MyBlogs()
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return RedirectToAction("Login", "User");
        }

        var result = await _blogService.GetMyBlogsAsync(userId);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(new List<BlogReadDto>());
        }

        return View(result.Data);
    }

    // GET: Blog/Create - Show create form (Teacher/Admin only)
    public IActionResult Create()
    {
        return View();
    }

    // POST: Blog/Create - Handle blog creation (Teacher/Admin only)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BlogCreateDto dto)
    {
        if (!ModelState.IsValid)
        {
            return View(dto);
        }

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return RedirectToAction("Login", "User");
        }

        var result = await _blogService.CreateBlogAsync(dto, userId);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return View(dto);
        }

        TempData["Success"] = "Blog created successfully!";
        return RedirectToAction(nameof(MyBlogs));
    }

    // GET: Blog/Edit/5 - Show edit form (Teacher/Admin only, own blogs)
    public async Task<IActionResult> Edit(Guid id)
    {
        var result = await _blogService.GetBlogByIdAsync(id);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            return RedirectToAction(nameof(MyBlogs));
        }

        var userId = GetCurrentUserId();
        if (result.Data!.AuthorId != userId)
        {
            TempData["Error"] = "You can only edit your own blogs";
            return RedirectToAction(nameof(MyBlogs));
        }

        var updateDto = new BlogUpdateDto
        {
            Title = result.Data.Title,
            Content = result.Data.Content,
            Status = result.Data.Status
        };

        ViewBag.BlogId = id;
        return View(updateDto);
    }

    // POST: Blog/Edit/5 - Handle blog update (Teacher/Admin only, own blogs)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(Guid id, BlogUpdateDto dto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.BlogId = id;
            return View(dto);
        }

        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return RedirectToAction("Login", "User");
        }

        var result = await _blogService.UpdateBlogAsync(id, dto, userId);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
            ViewBag.BlogId = id;
            return View(dto);
        }

        TempData["Success"] = "Blog updated successfully!";
        return RedirectToAction(nameof(MyBlogs));
    }

    // POST: Blog/Delete/5 - Delete blog (Teacher/Admin only, own blogs)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(Guid id)
    {
        var userId = GetCurrentUserId();
        if (userId == Guid.Empty)
        {
            return RedirectToAction("Login", "User");
        }

        var result = await _blogService.DeleteBlogAsync(id, userId);
        
        if (!result.Success)
        {
            TempData["Error"] = result.Message;
        }
        else
        {
            TempData["Success"] = "Blog deleted successfully!";
        }

        return RedirectToAction(nameof(MyBlogs));
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}
