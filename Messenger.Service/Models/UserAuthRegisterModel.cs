using Messenger.Service.Models.Enums;

namespace Messenger.Service.Models
{
    public class UserAuthRegisterModel
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public UserRole Role { get; set; }
    }
}