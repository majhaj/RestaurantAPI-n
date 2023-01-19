using AutoMapper;
using RestaurantAPI_n.Entities;
using RestaurantAPI_n.Models;

namespace RestaurantAPI_n
{
    public class RestaurantMappingProfile : Profile
    {
        public RestaurantMappingProfile()
        {
            CreateMap<Restaurant, RestaurantDto>()
                .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
                .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
                .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));

            CreateMap<Dish, DishDto>();

            CreateMap<CreateRestaurantDto, Restaurant>()
                .ForMember(m => m.Address, c => c.MapFrom(dto => new Address() 
                {
                    City = dto.City,
                    Street = dto.Street,
                    PostalCode = dto.PostalCode
                }));

            CreateMap<CreateDishDto, Dish>();

        }
    }
}
