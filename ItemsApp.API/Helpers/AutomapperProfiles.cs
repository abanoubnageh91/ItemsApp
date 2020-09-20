using AutoMapper;
using ItemsApp.API.Dtos;
using ItemsApp.API.Models;

namespace ItemsApp.API.Helpers
{
    public class AutomapperProfiles : Profile
    {
        public AutomapperProfiles()
        {
            CreateMap<Item, ItemForListDto>();
            CreateMap<ItemForCreationDto, Item>().ReverseMap();
            CreateMap<Item, ItemToReturnDto>();

        }

    }
}