using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI_n.Authorization
{
    public class CreatedMultipleRestauranstRequirement : IAuthorizationRequirement
    {
        public int MinimumRestaurantsCreated { get; }

        public CreatedMultipleRestauranstRequirement(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }
    }
}
