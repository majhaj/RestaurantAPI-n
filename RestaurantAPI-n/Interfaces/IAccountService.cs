using RestaurantAPI_n.Models;

namespace RestaurantAPI_n.Interfaces
{
    public interface IAccountService
    {
        void RegisterUser(RegisterUserDto dto);
        string GenerateJwt(LoginDto dto);
    }
}
