using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.DataAccess.Entities;

namespace HappyWarehouse.BusinessLogic
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Warehouse, WarehouseDto>();
            CreateMap<Item, ItemDto>();
            CreateMap<ApplicationUser, UserDto>();
        }
    }
}
