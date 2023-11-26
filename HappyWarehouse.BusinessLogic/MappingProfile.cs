using AutoMapper;
using HappyWarehouse.BusinessLogic.DTOs;
using HappyWarehouse.DataAccess.Entities;
using HappyWarehouse.Shared.Common;

namespace HappyWarehouse.BusinessLogic
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WarehouseDto, Warehouse>();
            CreateMap<Warehouse, WarehouseDto>()
                .ForMember(dto=> dto.CountryName, opt=> opt.MapFrom(entity=> entity.Country.Name));

            CreateMap<PaginatedList<Warehouse>, PaginatedList<WarehouseDto>>();
            CreateMap<ItemDto, Item>().ReverseMap();
            CreateMap<Item, ItemDto>()
                .ForMember(dto => dto.WarehouseName, opt => opt.MapFrom(entity => entity.Warehouse.Name));

            CreateMap<PaginatedList<Item>, PaginatedList<ItemDto>>();
            CreateMap<Country, CountryDto>().ReverseMap();
            CreateMap<RoleDto, ApplicationRole>().ReverseMap();
            CreateMap<UserDto, ApplicationUser>().ReverseMap();

            CreateMap<PaginatedList<ApplicationUser>, PaginatedList<UserDto>>();
        }
    }
}
