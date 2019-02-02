using Dapper;
using System.Data;
using System;
using Dapper.Contrib.Extensions;

namespace Guestbook.Models
{

    public class Message
    {
        [Key]
        public int ID { get; set; }
        public string SenderName { get; set; }
        public string Email { get; set; }
        public string MessageText { get; set; }
        public DateTime MessageDate { get; set; }
    }

}

// CREATE TABLE Messages (
//     ID int IDENTITY(1,1) PRIMARY KEY,
//     SenderName nvarchar(255),
//     Email nvarchar(255),
//     MessageText nvarchar(Max),
//     MessageDate smalldatetime
// ); 
