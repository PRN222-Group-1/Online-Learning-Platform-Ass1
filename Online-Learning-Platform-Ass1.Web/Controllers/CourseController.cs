using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

[Authorize]
public class CourseController(
    ICourseService courseService,
    IOrderService orderService) : Controller
{
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Guid? userId = null;
        if (!string.IsNullOrEmpty(userIdString) && Guid.TryParse(userIdString, out var parsedId))
        {
            userId = parsedId;
        }

        var course = await courseService.GetCourseDetailsAsync(id, userId);
        if (course == null) return NotFound();
        return View(course);
    }

    // GET: Confirm Purchase
    public async Task<IActionResult> Checkout(Guid id)
    {
        var course = await courseService.GetCourseDetailsAsync(id);
        if (course == null) return NotFound();

        // Check if user is logged in (Authorize attribute handles it but double check logic if needed)
        // Check if already enrolled (Service logic usually, but here we just show checkout)

        return View(course);
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var order = await orderService.CreateOrderAsync(userId, new CreateOrderDto { CourseId = id });
        if (order == null) return BadRequest("Could not create order.");

        // Redirect to VNPay
        return RedirectToAction("CreatePaymentUrl", "Payment", new { orderId = order.OrderId });
    }

    public IActionResult Success(Guid id)
    {
        ViewBag.OrderId = id;
        return View();
    }
    public async Task<IActionResult> MyCourses()
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return RedirectToAction("Login", "User");
        }

        var courses = await courseService.GetEnrolledCoursesAsync(userId);
        return View(courses);
    }
}
