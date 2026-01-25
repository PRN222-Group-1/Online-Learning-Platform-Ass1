using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Web.Controllers;

[Authorize]
public class LearningPathController(
    ILearningPathService learningPathService,
    ILearningPathEnrollmentService enrollmentService,
    IOrderService orderService) : Controller
{
    /// <summary>
    /// Browse all published learning paths (public)
    /// </summary>
    [AllowAnonymous]
    public async Task<IActionResult> Browse()
    {
        var paths = await learningPathService.GetPublishedPathsAsync();
        return View(paths);
    }

    /// <summary>
    /// Get featured learning paths (public)
    /// </summary>
    [AllowAnonymous]
    public async Task<IActionResult> Featured()
    {
        var paths = await learningPathService.GetFeaturedLearningPathsAsync();
        return View(paths);
    }

    /// <summary>
    /// View learning path details with optional user progress (public)
    /// </summary>
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var parsedId))
        {
            userId = parsedId;
        }

        var pathDetails = await learningPathService.GetPathDetailsWithProgressAsync(id, userId);
        if (pathDetails == null)
        {
            return NotFound();
        }

        return View(pathDetails);
    }

    /// <summary>
    /// Get user's enrolled learning paths
    /// </summary>
    public async Task<IActionResult> MyPaths()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return RedirectToAction("Login", "User");
        }

        var userPaths = await learningPathService.GetUserEnrolledPathsAsync(userId);
        return View(userPaths);
    }

    /// <summary>
    /// View user's progress on a specific learning path
    /// </summary>
    public async Task<IActionResult> Progress(Guid pathId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var pathProgress = await learningPathService.GetUserPathProgressAsync(userId, pathId);
        if (pathProgress == null)
        {
            TempData["ErrorMessage"] = "You are not enrolled in this learning path.";
            return RedirectToAction(nameof(MyPaths));
        }

        return View(pathProgress);
    }

    /// <summary>
    /// Prepare to purchase/checkout a learning path
    /// </summary>
    [AllowAnonymous]
    public async Task<IActionResult> Checkout(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var parsedId))
        {
            userId = parsedId;
        }

        var pathDetails = await learningPathService.GetPathDetailsWithProgressAsync(id, userId);
        if (pathDetails == null) return NotFound();

        return View(pathDetails);
    }

    /// <summary>
    /// Process checkout and create order for learning path
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var order = await orderService.CreateOrderAsync(userId, new CreateOrderDto { PathId = id });
        if (order == null)
        {
            TempData["ErrorMessage"] = "Could not create order. You may already own all courses in this path.";
            return RedirectToAction(nameof(Details), new { id });
        }

        // Redirect to VNPay payment
        return RedirectToAction("CreatePaymentUrl", "Payment", new { orderId = order.OrderId });
    }

    /// <summary>
    /// Drop a learning path enrollment
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> DropPath(Guid enrollmentId)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var success = await enrollmentService.DropPathAsync(enrollmentId);
        if (!success)
        {
            TempData["ErrorMessage"] = "Unable to drop learning path.";
            return RedirectToAction(nameof(MyPaths));
        }

        TempData["SuccessMessage"] = "You have successfully dropped this learning path.";
        return RedirectToAction(nameof(MyPaths));
    }
}
