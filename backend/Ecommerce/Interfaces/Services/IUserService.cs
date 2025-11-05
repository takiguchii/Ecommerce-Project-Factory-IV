using Ecommerce.DTOs;
using System.Threading.Tasks;

namespace Ecommerce.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserDto?> GetMyProfileAsync(string appUserId);
        Task<UserDto?> UpdateMyProfileAsync(string appUserId, UpdateUserDto dto);
    }
}