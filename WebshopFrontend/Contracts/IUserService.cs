using Microsoft.AspNetCore.Cors.Infrastructure;
using WebshopShared;
using WebshopShared.Validation;

namespace WebshopFrontend.Contracts
{
    public interface IUserService
    {
        public Task<UserDetailsDto> UpdateUserDetails(UserDetails userDetails);
        public Task<UserDetailsDto> GetUserDetails();
    }
}
