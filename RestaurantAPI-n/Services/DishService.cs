using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using RestaurantAPI_n.Entities;
using RestaurantAPI_n.Exceptions;
using RestaurantAPI_n.Interfaces;
using RestaurantAPI_n.Models;

namespace RestaurantAPI_n.Services
{
    public class DishService : IDishService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;   
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);
            dishEntity.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dishEntity);
            _dbContext.SaveChanges();

            return dishEntity.Id;
        }

        public DishDto GetById(int restaurantId, int dishId)
        {
            var dish = GetDishById(restaurantId, dishId);

            var dishDto = _mapper.Map<DishDto>(dish);
            return dishDto;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishDtos = _mapper.Map<List< DishDto>>(restaurant.Dishes);
            return dishDtos;

        }

        public void RemoveAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        public void RemoveById(int restaurantId, int dishId)
        {
            var dish = GetDishById(restaurantId, dishId);

            _dbContext.Remove(dish);
            _dbContext.SaveChanges();
        }

        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == restaurantId);
            if (restaurant == null)
                throw new NotFoundException("Restaurant not found.");

            return restaurant;
        }

        private Dish GetDishById(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);
            var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);

            if (dish == null || dish.RestaurantId != restaurantId)
                throw new NotFoundException("Dish not found.");

            return dish;
        }
    }
}
