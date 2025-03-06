using WebshopShared;

namespace WebshopFrontend.Services.Interfaces
{
    public interface IUserService
    {
        public Task<bool> RegisterUser(RegisterUserDto user);
        public Task<bool> LogInUser(LoginDto user);
        public Task<bool> LogOutUser();
        public Task<bool> GetIfLoggedIn();
    }
}
