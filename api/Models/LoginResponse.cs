namespace api.Models
{
    public class LoginResponse
    {
        public bool Success { get; set; }

        public string AccessToken { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public bool isAdmin { get; set; } = false;

        public string Message { get; set; } = string.Empty;
    }
}
