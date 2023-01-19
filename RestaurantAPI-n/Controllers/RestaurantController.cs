using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI_n.Entities;
using RestaurantAPI_n.Interfaces;
using RestaurantAPI_n.Models;
using System.Security.Claims;

namespace RestaurantAPI_n.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPut("{id}")]
        public ActionResult Update([FromBody]UpdateRestuarantDto dto, [FromRoute]int id)
        {
            _restaurantService.Update(dto, id);

            return Ok();
        }

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute]int id)
        {
            _restaurantService.Delete(id);

            return NoContent();
        }

        [HttpPost]
        [Authorize(Roles = "Admin, Manager")]

        public ActionResult CreateRestaurant([FromBody]CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _restaurantService.Create(dto);

            return Created($"/api/restuarant/{id}", null);
        }

        [HttpGet]
        [Authorize(Policy = "CreatedAtLeast2Restaurants")]
        public ActionResult<PageResult<RestaurantDto>> GetAll([FromQuery]RestaurantQuery query)
        {
            var restaurantsDtos =_restaurantService.GetAll(query);
            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public ActionResult<RestaurantDto> Get([FromRoute]int id) 
        {
            var restaurant = _restaurantService.GetById(id);
           
            return Ok(restaurant);
        }
    }
}
