using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionDeleteConsumer : IConsumer<AuctionDeleted>
    {
        public async Task Consume(ConsumeContext<AuctionDeleted> context)
        {
            Console.WriteLine("---> Consumer Auction Delete", context.Message.Id);

            var result = await DB.DeleteAsync<Item>(context.Message.Id);


            if (!result.IsAcknowledged) 
            {
                throw new MessageException(typeof(AuctionDeleted),"problem delete");
            }
        }
    }
}
