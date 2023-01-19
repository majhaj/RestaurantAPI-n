using RestaurantAPI_n.Models;

namespace RestaurantAPI_n.Interfaces
{
    public interface IDishService
    {
        int Create(int restuarantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        void RemoveAll(int restaurantId);
        void RemoveById(int restaurantId, int dishId);
    }
}
