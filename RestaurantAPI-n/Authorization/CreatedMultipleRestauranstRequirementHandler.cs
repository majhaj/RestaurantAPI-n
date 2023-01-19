using Microsoft.AspNetCore.Authorization;
using RestaurantAPI_n.Entities;
using System.Security.Claims;

namespace RestaurantAPI_n.Authorization
{
    public class CreatedMultipleRestauranstRequirementHandler : AuthorizationHandler<CreatedMultipleRestauranstRequirement>
    {
        private readonly RestaurantDbContext _dbContext;

        public CreatedMultipleRestauranstRequirementHandler(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestauranstRequirement requirement)
        {
            var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);

            var createdRestuarantsCount = _dbContext.Restaurants.Count(r => r.CreatedById== userId);

            if(createdRestuarantsCount >= requirement.MinimumRestaurantsCreated)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
