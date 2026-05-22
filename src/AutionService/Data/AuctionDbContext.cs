using AutionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutionService.Data
{
    public class AuctionDbContext : DbContext
    {
        public AuctionDbContext(DbContextOptions options) : base(options)
        {
        }

       public DbSet<Auction> Auctions { get; set; }
    }
}
