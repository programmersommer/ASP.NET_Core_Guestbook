using System.ComponentModel.DataAnnotations;

namespace Guestbook.ViewModels
{
    public class AddMessageViewModel
    {
        public string Token { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please leave your message")]
        public string MessageText { get; set; }
    }
}
