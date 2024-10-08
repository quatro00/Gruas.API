namespace Gruas.API.Models.DTO.Auth
{
    public class ForgotPasswordRequestDto
    {
        public required string Email { get; set; }
        public required string Username { get; set; }
    }
}
