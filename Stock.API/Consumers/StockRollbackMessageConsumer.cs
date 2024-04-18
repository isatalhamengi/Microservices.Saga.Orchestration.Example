using MassTransit;
using MongoDB.Driver;
using Shared.Messages;
using Stock.API.Services;

namespace Stock.API.Consumers
{
    public class StockRollbackMessageConsumer : IConsumer<StockRollBackMessage>
    {
        MongoDbService _dbService;

        public StockRollbackMessageConsumer(MongoDbService dbService)
        {
            _dbService = dbService;
        }

        public async Task Consume(ConsumeContext<StockRollBackMessage> context)
        {
            var stockCollection = _dbService.GetCollection<Stock.API.Models.Stock>();
            foreach (var orderItem in context.Message.OrderItems)
            {
                var stock = await (await stockCollection.FindAsync(x => x.ProductId == orderItem.ProductId)).FirstOrDefaultAsync();

                stock.Count += orderItem.Count;
                await stockCollection.FindOneAndReplaceAsync(x=> x.ProductId == orderItem.ProductId,stock);
            }
        }
    }
}
