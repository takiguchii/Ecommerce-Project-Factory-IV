using Ecommerce.DTOs;
using Ecommerce.Interfaces.Repositories;
using Ecommerce.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Ecommerce.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<IdentityUser> _userManager;

        public UserService(IUserRepository userRepository, UserManager<IdentityUser> userManager)
        {
            _userRepository = userRepository;
            _userManager = userManager;
        }

        public async Task<UserDto?> GetMyProfileAsync(string appUserId)
        {
            // 1. Busca os dados de login (Identity)
            var appUser = await _userManager.FindByIdAsync(appUserId);
            if (appUser == null) return null;

            // 2. Busca os dados de negócio (Perfil)
            var userProfile = await _userRepository.GetByAppUserIdAsync(appUserId);
            // Se o usuário foi registrado corretamente, userProfile nunca será null
            if (userProfile == null) return null; 

            // 3. Combina os dados (Identity + Perfil) em um DTO
            return new UserDto
            {
                // Dados do Identity
                Email = appUser.Email,
                Username = appUser.UserName,
                
                // Dados do nosso Perfil 'User'
                FullName = userProfile.FullName,
                DocumentCpfCnpj = userProfile.DocumentCpfCnpj,
                Phone = userProfile.Phone,
                PostalCode = userProfile.PostalCode,
                AddressLine = userProfile.AddressLine,
                AddressComplement = userProfile.AddressComplement,
                Neighborhood = userProfile.Neighborhood,
                City = userProfile.City,
                State = userProfile.State
            };
        }

        public async Task<UserDto?> UpdateMyProfileAsync(string appUserId, UpdateUserDto dto)
        {
            // 1. Busca o perfil de negócio
            var userProfile = await _userRepository.GetByAppUserIdAsync(appUserId);
            if (userProfile == null) return null;

            // 2. Atualiza os campos
            userProfile.FullName = dto.FullName;
            userProfile.DocumentCpfCnpj = dto.DocumentCpfCnpj;
            userProfile.Phone = dto.Phone;
            userProfile.PostalCode = dto.PostalCode;
            userProfile.AddressLine = dto.AddressLine;
            userProfile.AddressComplement = dto.AddressComplement;
            userProfile.Neighborhood = dto.Neighborhood;
            userProfile.City = dto.City;
            userProfile.State = dto.State;

            // 3. Salva no banco
            _userRepository.Update(userProfile);
            _userRepository.SaveChanges();

            // 4. Retorna o perfil atualizado
            return await GetMyProfileAsync(appUserId);
        }
    }
}