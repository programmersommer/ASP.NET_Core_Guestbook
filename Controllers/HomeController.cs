
using System;
using System.Text;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Guestbook.Models;

namespace Guestbook
{

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            // List<Message> messages;
            // using (IDbConnection db = new SqlConnection(_configuration["SQLConnectionString"]))
            // {
            //     messages = db.Query<Message>("SELECT * FROM Messages2").ToList();
            // }

            return View("Index");
        }

          public IActionResult AddMessagePage()
        {

           return View("AddMessage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
          public string AddMessage(Guestbook.ViewModels.AddMessageViewModel model)
        {

           return model.Token;
        }


        [ValidateAntiForgeryToken]
        public IActionResult DataHandler(DataTableParameters model, string texttype, string projectid, DateTime? datefrom, DateTime? datetill)
        {
            var start = Convert.ToInt32(HttpContext.Request.Form["start"].FirstOrDefault());
            var length = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault());

            List<Message> messages;
            int total;
            using (IDbConnection db = new SqlConnection(_configuration["SQLConnectionString"]))
            {
                total = db.QuerySingle<int>("SELECT Count(*) FROM Messages2");
                messages = db.Query<Message>("SELECT * FROM Messages2 ORDER BY MessageDate ASC OFFSET "+start+" ROWS FETCH NEXT "+length+" ROWS ONLY").ToList();
            }
                
                var newData = messages.Select(m => new[]
                {
                    "<div class=\"row align-items-center justify-content-center mt-3\">"
                                  +"<span style=\"color:Orange;\">"+m.SenderName+"</span>"
                                          +"&nbsp; &nbsp; &nbsp;"
                                     + "<span style=\"color:DarkBlue;\">"+m.Email+"</span>"
                                         +"&nbsp; &nbsp; &nbsp;"
                                     +"<span style=\"color:BurlyWood;\">"+m.MessageDate+"</span>"
                                   +"</div>"
                                   +"<div class=\"row align-items-center justify-content-center mt-3\">"
                                      +"<div>"+DisplaySmiles(m.MessageText)+"</div>"
                                   +"</div>"
                }).ToList();


                var result = new
                {
                    aaData = newData,
                    iTotalDisplayRecords = total,
                    iTotalRecords = total
                };

                return Json(result);           
        }

          public string DisplaySmiles(string text)
        {
            StringBuilder mess = new StringBuilder(text);

            mess = mess.Replace(":)", "<img alt='smile' src='images/emotions/smile.gif' />");
            mess = mess.Replace(";)", "<img alt='wink' src='images/emotions/wink.gif' />");
            mess = mess.Replace(":(", "<img alt='sad' src='images/emotions/sad.gif' />");
            mess = mess.Replace(":robot:", "<img alt='robot' src='images/emotions/robot.gif' />");
            mess = mess.Replace(":oops:", "<img alt='oops' src='images/emotions/oops.gif' />");
            mess = mess.Replace(":inLove:", "<img alt='love' src='images/emotions/inLove.gif' />");
            mess = mess.Replace(":fingerUp:", "<img alt='finger up' src='images/emotions/fingerUp.gif' />");
            mess = mess.Replace(":fingerDown:", "<img alt='finger down' src='images/emotions/fingerDown.gif' />");
            mess = mess.Replace(":angel:", "<img alt='angel' src='images/emotions/angel.gif' />");
            mess = mess.Replace(":angry:", "<img alt='angry' src='images/emotions/angry.gif' />");      
            mess = mess.Replace("&lt;br /&gt;", "<br />");
            
            return mess.ToString();
        }

    }

}