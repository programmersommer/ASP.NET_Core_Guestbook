using System.ComponentModel.DataAnnotations;

namespace Guestbook.ViewModels
{

    public class AddMessageViewModel
    {
        public string Token { get; set; }
        [Required]
        public string SenderName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string MessageText { get; set; }
    }
}
