using Online_Learning_Platform_Ass1.Data.Database;
using Online_Learning_Platform_Ass1.Data.Database.Entities;
using Online_Learning_Platform_Ass1.Data.Repositories.Interfaces;

namespace Online_Learning_Platform_Ass1.Data.Repositories;

public class OrderRepository(OnlineLearningContext context) : IOrderRepository
{
    public async Task<Order?> GetByIdAsync(Guid id)
    {
        return await context.Orders
            .Include(o => o.Course)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task AddAsync(Order order)
    {
        await context.Orders.AddAsync(order);
    }

    public async Task AddEnrollmentAsync(Enrollment enrollment)
    {
        await context.Enrollments.AddAsync(enrollment);
    }

    public async Task AddTransactionAsync(Transaction transaction)
    {
        await context.Transactions.AddAsync(transaction);
    }

    public async Task<int> SaveChangesAsync() => await context.SaveChangesAsync();
}
