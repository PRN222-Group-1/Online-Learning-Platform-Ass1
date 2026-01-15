using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

[Authorize]
public class LearningPathController(
    ILearningPathService learningPathService,
    IOrderService orderService) : Controller
{
    [AllowAnonymous]
    public async Task<IActionResult> Details(Guid id)
    {
        var path = await learningPathService.GetLearningPathDetailsAsync(id);
        if (path == null) return NotFound();
        return View(path); // Needs a View, but for now just backend logic
    }

    public async Task<IActionResult> Checkout(Guid id)
    {
        var path = await learningPathService.GetLearningPathDetailsAsync(id);
        if (path == null) return NotFound();

        return View(path); // Reusing a simple view or creating new one
    }

    [HttpPost]
    public async Task<IActionResult> ProcessCheckout(Guid id)
    {
        var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userIdString) || !Guid.TryParse(userIdString, out var userId))
        {
            return Unauthorized();
        }

        var order = await orderService.CreateOrderAsync(userId, new CreateOrderDto { PathId = id });
        if (order == null) return BadRequest("Could not create order.");

        var success = await orderService.ProcessPaymentAsync(order.OrderId);
        if (success)
        {
            return RedirectToAction("Success", "Course", new { id = order.OrderId }); // Reuse Course Success page
        }

        ModelState.AddModelError("", "Payment failed.");
        var path = await learningPathService.GetLearningPathDetailsAsync(id);
        return View("Checkout", path);
    }
}
