using System.ComponentModel.DataAnnotations; 
using Microsoft.AspNetCore.Mvc;  
using System.Data;
using System;

namespace Guestbook.ViewModels
{

    public class AddMessageViewModel
    {
        public String Token { get; set; } 
        public string SenderName { get; set; }
        public string Email { get; set; }
        public string MessageText { get; set; }
    }

}
