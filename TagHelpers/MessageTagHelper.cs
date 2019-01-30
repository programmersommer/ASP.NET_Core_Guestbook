using Microsoft.AspNetCore.Razor.TagHelpers;
using System.Threading.Tasks;
using System;
using System.Text;

namespace Guestbook.TagHelpers
{
    // Not used anymore in project
    public class MessageTagHelper : TagHelper
    {
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            StringBuilder mess = new StringBuilder((await output.GetChildContentAsync()).GetContent());

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
            
            output.Content.SetHtmlContent(mess.ToString());
        }
    }
}