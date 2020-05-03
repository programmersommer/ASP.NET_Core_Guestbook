using System;
using System.Text;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Guestbook.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Dapper.Contrib.Extensions;
using System.Web;

namespace Guestbook
{

    public class HomeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("defaultClient");
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View("Index");
        }

        public IActionResult AddMessagePage()
        {
            return View("AddMessage");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<bool> AddMessage(Guestbook.ViewModels.AddMessageViewModel model)
        {
            try
            {
                var parameters = new Dictionary<string, string> {
                { "secret", _configuration["ReCaptcha:SecretKey"] },
                { "response", model.Token } };
                var encodedContent = new FormUrlEncodedContent(parameters);
                var response = await _httpClient.PostAsync("https://www.google.com/recaptcha/api/siteverify", encodedContent);
                var result = JsonConvert.DeserializeObject<ReCaptchaResponse>(await response.Content.ReadAsStringAsync());

                if (result.success)
                {
                    using (IDbConnection connection = new SqlConnection(_configuration["SQLConnectionString"]))
                    {
                        await connection.InsertAsync<Message>(new Message { SenderName = model.Name, Email = model.Email, MessageText = model.MessageText, MessageDate = System.DateTime.UtcNow });

                        // without Contrib extension
                        // string sql = "INSERT INTO Messages([SenderName],[Email],[MessageText],[MessageDate]) values (@SenderName, @Email, @MessageText, @MessageDate)";
                        // var identity = connection.Execute(sql, new Message { SenderName = model.SenderName, Email = model.Email, MessageText = model.MessageText, MessageDate = System.DateTime.UtcNow});
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DataHandler(DataTableParameters model)
        {
            var start = Convert.ToInt32(HttpContext.Request.Form["start"].FirstOrDefault());
            var length = Convert.ToInt32(HttpContext.Request.Form["length"].FirstOrDefault());

            List<Message> messages;
            int total;
            using (IDbConnection connection = new SqlConnection(_configuration["SQLConnectionString"]))
            {
                total = connection.QuerySingle<int>("SELECT Count(*) FROM dbo.Messages");
                messages = connection.Query<Message>("SELECT * FROM dbo.Messages ORDER BY MessageDate DESC OFFSET " + start + " ROWS FETCH NEXT " + length + " ROWS ONLY").ToList();
            }

            var newData = messages.Select(m => new[]
            {
                    "<div class=\"row align-items-center justify-content-center mt-3\">"
                                  +"<span style=\"color:Orange;\">"+HttpUtility.HtmlEncode(m.SenderName)+"</span>"
                                          +"&nbsp; &nbsp; &nbsp;"
                                     + "<span style=\"color:DarkBlue;\">"+HttpUtility.HtmlEncode(m.Email)+"</span>"
                                         +"&nbsp; &nbsp; &nbsp;"
                                     +"<span style=\"color:BurlyWood;\">"+HttpUtility.HtmlEncode(m.MessageDate)+"</span>"
                                   +"</div>"
                                   +"<div class=\"row align-items-center justify-content-center mt-3\">"
                                      +"<div>"+DisplaySmiles(HttpUtility.HtmlEncode(m.MessageText))+"</div>"
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
            StringBuilder sb = new StringBuilder(text);

            sb = sb.Replace(":)", "<img alt='smile' src='images/emotions/smile.gif' />")
                       .Replace(";)", "<img alt='wink' src='images/emotions/wink.gif' />")
                       .Replace(":(", "<img alt='sad' src='images/emotions/sad.gif' />")
                       .Replace(":robot:", "<img alt='robot' src='images/emotions/robot.gif' />")
                       .Replace(":oops:", "<img alt='oops' src='images/emotions/oops.gif' />")
                       .Replace(":inLove:", "<img alt='love' src='images/emotions/inLove.gif' />")
                       .Replace(":fingerUp:", "<img alt='finger up' src='images/emotions/fingerUp.gif' />")
                       .Replace(":fingerDown:", "<img alt='finger down' src='images/emotions/fingerDown.gif' />")
                       .Replace(":angel:", "<img alt='angel' src='images/emotions/angel.gif' />")
                       .Replace(":angry:", "<img alt='angry' src='images/emotions/angry.gif' />");

            return sb.ToString();
        }

    }
}