namespace Steady_Management.WebAPI.DTOs
{
    public class LoginResponseDTO
    {
        public string Token { get; init; } = string.Empty;
        public int UserId { get; init; }
        public string Username { get; init; } = string.Empty;
        public int RoleId { get; init; }
    }
}
