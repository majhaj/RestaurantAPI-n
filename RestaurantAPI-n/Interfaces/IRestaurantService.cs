using RestaurantAPI_n.Entities;
using RestaurantAPI_n.Models;
using System.Security.Claims;

namespace RestaurantAPI_n.Interfaces
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PageResult<RestaurantDto> GetAll(RestaurantQuery query);
        int Create(CreateRestaurantDto dto);
        void Delete(int id);
        void Update(UpdateRestuarantDto dto, int id);
    }
}
