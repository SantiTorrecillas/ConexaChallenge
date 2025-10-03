using ConexaChallenge.Dtos;
using ConexaChallenge.Entities;

namespace ConexaChallenge.Interfaces
{
    public interface IAuthService
    {
        Task<User?> RegisterAsync(UserRequest user);
        Task<string?> LoginAsync(UserRequest user);
    }
}
