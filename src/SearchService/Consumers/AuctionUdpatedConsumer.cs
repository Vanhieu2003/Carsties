using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers
{
    public class AuctionUdpatedConsumer : IConsumer<AuctionUpdated>
    {
        private readonly IMapper _mapper;

        public AuctionUdpatedConsumer(IMapper mapper)
        {
            _mapper = mapper;
        }

        public async Task Consume(ConsumeContext<AuctionUpdated> context)
        {
            Console.WriteLine(" ---> Cosumer Auction updated" + context.Message.Id);

            var auction = _mapper.Map<Item>(context.Message);

            var item = await DB.Update<Item>()
        .MatchID(context.Message.Id)
        .ModifyExcept(b => new { b.Make, b.Model, b.Color, b.Year, b.Mileage }, auction)
        .ExecuteAsync();

            if (item != null)
            {
                throw new MessageException(typeof(AuctionUpdated), "Problem Updated");
            }
        }
    }
}
