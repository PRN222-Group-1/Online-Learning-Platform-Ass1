using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Payment;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Controllers;

[Authorize]
public class PaymentController(
    IVnPayService vnPayService,
    IOrderRepository orderRepository,
    IOrderService orderService) : Controller
{
    public async Task<IActionResult> CreatePaymentUrl(Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return NotFound("Order not found");
        }
        
        // Ensure order is not already paid
        if (order.Status == "completed")
        {
            return RedirectToAction("Success", "Course", new { id = orderId });
        }

        // Create VnPayRequestModel
        var model = new VnPayRequestModel
        {
            OrderId = order.Id,
            Amount = order.TotalAmount,
            CreatedDate = DateTime.Now,
            Description = $"Payment for order {order.Id}",
            FullName = User.Identity?.Name ?? "Guest"
        };
        
        // This generates the full URL to redirect to VNPay
        var paymentUrl = vnPayService.CreatePaymentUrl(HttpContext.Connection.RemoteIpAddress?.ToString() ?? "127.0.0.1", model);
        
        return Redirect(paymentUrl);
    }

    public async Task<IActionResult> PaymentCallback()
    {
        var queryDictionary = Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
        var response = vnPayService.PaymentExecute(queryDictionary);

        if (response.Success && response.VnPayResponseCode == "00")
        {
            // Payment success. Update order status.
            if (Guid.TryParse(response.OrderId, out var orderId))
            {
                 // Assuming ProcessPaymentAsync inside OrderService might be doing the actual "Enroll" logic too
                 // We reuse OrderService.ProcessPaymentAsync(orderId) which we built earlier
                 // OR we manually update if that method assumes different flow. 
                 // Based on previous turn, ProcessPaymentAsync changes order to "completed" and enrolls.
                 
                var success = await orderService.ProcessPaymentAsync(orderId);
                if (success)
                {
                    return RedirectToAction("Success", "Course", new { id = orderId });
                }
            }
        }

        // Failure
        TempData["Message"] = $"Payment failed or cancelled. Response Code: {response.VnPayResponseCode}";
        return RedirectToAction("Index", "Home");
    }
}
