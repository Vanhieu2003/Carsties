using AutoMapper;
using Contracts;
using SearchService.Models;

namespace SearchService.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<AuctionCreated, Item>();
        }
    }
}
