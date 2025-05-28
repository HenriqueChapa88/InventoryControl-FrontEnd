using InventoryControl.Core.Entities;
using InventoryControl.Core.Interfaces;
using InventoryControl.Infrastructure.Security;

namespace InventoryControl.Infrastructure.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly TokenService _tokenService;

        public AuthService(IUserRepository userRepository, TokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<User> Register(User user, string password)
        {
            if (await UserExists(user.Email))
                throw new ArgumentException("Email já está em uso");

            user.PasswordHash = PasswordHasher.HashPassword(password);
            return await _userRepository.AddAsync(user);
        }

        public async Task<string> Login(string email, string password)
        {
            var user = await _userRepository.GetByEmailAsync(email) 
                ?? throw new UnauthorizedAccessException("Credenciais inválidas");

            if (!PasswordHasher.VerifyPassword(password, user.PasswordHash))
                throw new UnauthorizedAccessException("Credenciais inválidas");

            return _tokenService.CreateToken(user);
        }

        public async Task<bool> UserExists(string email)
        {
            return await _userRepository.GetByEmailAsync(email) != null;
        }
    }
}