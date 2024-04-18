using MassTransit;
using Order.API.Context;
using Shared.OrderEvents;

namespace Order.API.Consumers
{
    public class OrderFailedEventConsumer : IConsumer<OrderFailedEvent>
    {
        OrderDbContext _db;

        public OrderFailedEventConsumer(OrderDbContext db)
        {
            _db = db;
        }

        public async Task Consume(ConsumeContext<OrderFailedEvent> context)
        {
            Order.API.Models.Order order = await _db.Orders.FindAsync(context);
            if (order != null) {
                order.OrderStatus = Enums.OrderStatus.Fail;
                await _db.SaveChangesAsync();
            }
        }
    }
}
