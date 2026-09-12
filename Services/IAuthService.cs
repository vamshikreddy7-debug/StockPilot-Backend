using StockPilot.API.DTOs;

namespace StockPilot.API.Services;

public interface IAuthService
{
    Task<AuthResponse> RegisterAsync(RegisterRequest request);
    Task<AuthResponse> LoginAsync(LoginRequest request);
    Task<UserDto?> GetUserByIdAsync(int userId);
    string GenerateJwtToken(int userId, string email);
}
