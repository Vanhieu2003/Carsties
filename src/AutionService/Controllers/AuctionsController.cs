using AutionService.Data;
using AutionService.DTOs;
using AutionService.Entities;
using AutoMapper;
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
        public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions()
        {
            var auctions = await _context.Auctions
                                .Include(x => x.Item)
                                .OrderBy(x => x.Item.Make)
                                .ToListAsync();

            return _mapper.Map<List<AuctionDto>>(auctions);
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
    }
}
