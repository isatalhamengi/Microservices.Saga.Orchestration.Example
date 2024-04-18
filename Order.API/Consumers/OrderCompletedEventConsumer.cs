using MassTransit;
using Order.API.Context;
using Order.API.Models;
using Shared.OrderEvents;

namespace Order.API.Consumers
{
    public class OrderCompletedEventConsumer : IConsumer<OrderCompletedEvent>
    {
        OrderDbContext _db;

        public OrderCompletedEventConsumer(OrderDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<OrderCompletedEvent> context)
        {
            Order.API.Models.Order order = await _db.Orders.FindAsync(context.Message.OrderId);
            if (order != null)
            {
                order.OrderStatus = Enums.OrderStatus.Completed;
                await _db.SaveChangesAsync();
            }

        }
    }
}
