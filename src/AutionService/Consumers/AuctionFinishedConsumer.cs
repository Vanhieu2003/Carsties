using AutionService.Data;
using AutionService.Entities;
using Contracts;
using MassTransit;

namespace AutionService.Consumers
{
    public class AuctionFinishedConsumer : IConsumer<AuctionFinished>
    {
        private readonly AuctionDbContext _dbcontext;

        public AuctionFinishedConsumer(AuctionDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }
        public async Task Consume(ConsumeContext<AuctionFinished> context)
        {
           var auction = await _dbcontext.Auctions.FindAsync(context.Message.AuctionId);

            if (context.Message.ItemSold)
            {
                auction.Winner = context.Message.Winner;
                auction.Seller = context.Message.Seller;
            }

            auction.Status = auction.SoldAmount > auction.ReservePrice ? Status.Finished : Status.ReserveNotMet;

            await _dbcontext.SaveChangesAsync();
        }
    }
}
