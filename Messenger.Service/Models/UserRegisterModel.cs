using Messenger.Service.Models.Enums;

namespace Messenger.Service.Models
{
    public class UserRegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}
