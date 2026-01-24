using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;
using Online_Learning_Platform_Ass1.Service.DTOs.Order;
using Online_Learning_Platform_Ass1.Service.Services.Interfaces;

namespace Online_Learning_Platform_Ass1.Service.Services;

public class OrderService(
    IOrderRepository orderRepository, 
    ICourseRepository courseRepository,
    ILearningPathRepository learningPathRepository) : IOrderService
{
    public async Task<OrderViewModel?> CreateOrderAsync(Guid userId, CreateOrderDto dto)
    {
        decimal amount = 0;
        string title = "";

        if (dto.CourseId.HasValue)
        {
            var course = await courseRepository.GetByIdAsync(dto.CourseId.Value);
            if (course == null) return null;
            amount = course.Price;
            title = course.Title;
        }
        else if (dto.PathId.HasValue)
        {
            var path = await learningPathRepository.GetByIdAsync(dto.PathId.Value);
            if (path == null) return null;
            amount = path.Price;
            title = path.Title;
        }
        else
        {
            return null; // Invalid request
        }

        // Create Order
        var order = new Order
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            CourseId = dto.CourseId,
            PathId = dto.PathId,
            TotalAmount = amount,
            Status = "pending",
            CreatedAt = DateTime.UtcNow
        };

        await orderRepository.AddAsync(order);
        await orderRepository.SaveChangesAsync();

        return new OrderViewModel
        {
            OrderId = order.Id,
            CourseId = dto.CourseId ?? Guid.Empty, // Or handle nullability in VM
            CourseTitle = title, // Reused prop for title
            Amount = order.TotalAmount,
            Status = order.Status
        };
    }

    public async Task<OrderViewModel?> GetOrderByIdAsync(Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order == null) return null;

        return new OrderViewModel
        {
            OrderId = order.Id,
            CourseId = order.CourseId ?? Guid.Empty,
            CourseTitle = order.Course?.Title ?? order.Path?.Title ?? "Unknown",
            Amount = order.TotalAmount,
            Status = order.Status
        };
    }

    public async Task<bool> ProcessPaymentAsync(Guid orderId)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order == null || order.Status == "completed") return false;

        // 1. Mock Payment Gateway Success
        bool paymentSuccess = true; 

        if (paymentSuccess)
        {
            // 2. Create Transaction
            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                PaymentMethod = "Credit Card", // Mock
                TransactionGateId = "TXN-" + Guid.NewGuid().ToString().Substring(0, 8),
                Amount = order.TotalAmount,
                Status = "success",
                CreatedAt = DateTime.UtcNow
            };
            await orderRepository.AddTransactionAsync(transaction);

            // 3. Create Enrollment(s)
            
            if (order.CourseId.HasValue)
            {
                await CreateEnrollment(order.UserId, order.CourseId.Value);
            }
            else if (order.PathId.HasValue)
            {
                var path = await learningPathRepository.GetByIdAsync(order.PathId.Value);
                if (path != null)
                {
                    foreach (var pc in path.PathCourses)
                    {
                         await CreateEnrollment(order.UserId, pc.CourseId);
                    }
                }
            }

            // 4. Update Order Status
            order.Status = "completed";
            
            await orderRepository.SaveChangesAsync();
            return true;
        }

        return false;
    }

    private async Task CreateEnrollment(Guid userId, Guid courseId)
    {
         var enrollment = new Enrollment
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                CourseId = courseId, 
                EnrolledAt = DateTime.UtcNow,
                Status = "active"
            };
            await orderRepository.AddEnrollmentAsync(enrollment);
    }
}
