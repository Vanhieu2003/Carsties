using AutionService.Data;
using AutionService.DTOs;
using AutionService.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AutionService.Controllers
{
    [ApiController]
    [Route("/api/auctions")]
    public class AuctionsController : Controller
    {
        private readonly AuctionDbContext _context;
        private readonly IMapper _mapper;
        public AuctionsController( AuctionDbContext context, IMapper mapper) 
        {
            _context = context;
            _mapper = mapper;
            
        }

        [HttpGet]
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
        {
            var query = _context.Auctions.OrderBy(x => x.Item.Make).AsQueryable();

            if (!string.IsNullOrEmpty(date))
            {
                query = query.Where(x => x.UpdatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
            }

            return await query.ProjectTo<AuctionDto>(_mapper.ConfigurationProvider).ToListAsync();
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<AuctionDto>>GetAuctionById(Guid id)
        {
            var auction = await _context.Auctions.Include(x => x.Item)
                                                .FirstOrDefaultAsync(x => x.Id == id);

            if(auction == null) return NotFound();

            return _mapper.Map<AuctionDto >(auction);
        }


        [HttpPost]
        public async Task<ActionResult<AuctionDto>> CreateAuction (CreateAuctionDto dto)
        {
            var auction = _mapper.Map<Auction>(dto);

            auction.Seller = "test";

            _context.Auctions.Add(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not save changes to the db");

            return CreatedAtAction(nameof(GetAuctionById),
                new { auction.Id }, _mapper.Map<AuctionDto>(auction));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult> UpdatedAuction(Guid id,UpdateAuctionDto dto)
        {
            var aution = await _context.Auctions.Include(x => x.Item)
                                                .FirstOrDefaultAsync(x => x.Id == id);


            if (aution == null) return NotFound();

            aution.Item.Make = dto.Make ?? aution.Item.Make;
            aution.Item.Model = dto.Model ?? aution.Item.Model;
            aution.Item.Color = dto.Color ?? aution.Item.Color;
            aution.Item.Mileage = dto.Mileage ?? aution.Item.Mileage;
            aution.Item.Year = dto.Year ?? aution.Item.Year;

            var result = await _context.SaveChangesAsync() > 0;

            if (result) return Ok();

            return BadRequest("Problem saving changes");
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteAuction(Guid id)
        {
            var auction = await _context.Auctions.FindAsync(id);

            if (auction == null) return NotFound();

            _context.Remove(auction);

            var result = await _context.SaveChangesAsync() > 0;

            if (!result) return BadRequest("Could not update db");

            return Ok();
        }
    }
}
