using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SkypeDotnet
{
    public class SkypeClient
    {
        private SkypeClient()
        {
            
        }

        public static SkypeClient Login(LoginCredentials credentials)
        {

            var httpClient = new HttpClient();

            var response = httpClient.SendGet(new Uri(Constants.SkypeWebLoginUrlFull));

            var postParameters = GetPostParameters(response.ResponseData);

            postParameters.Add("username", credentials.UserName);
            postParameters.Add("password", credentials.Password);

            var loginUrl = new Uri(Constants.SkypeWebLoginUrlFull + "?client_id=" + postParameters["client_id"] + "&redirect_uri=https://web.skype.com");

            response = httpClient.SendPost(loginUrl, postParameters);
            

            throw new NotImplementedException();
        }

        private static Dictionary<string,string> GetPostParameters(string responseData)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(responseData);

            var node = doc.DocumentNode.SelectSingleNode("//form[@id=\"loginForm\"]");

            var pie = node.SelectSingleNode("//input[@type=\"hidden\" and @id=\"pie\"]");
            var etm = node.SelectSingleNode("//input[@type=\"hidden\" and @id=\"etm\"]");
            var timeZone = GetTimezone();
            var jsTime = GetJsTime();

            

            var result = new Dictionary<string, string>();
            result.Add("pie", pie.Attributes["value"].Value);
            result.Add("etm", etm.Attributes["value"].Value);
            result.Add("timezone_field", timeZone);
            result.Add("js_time", jsTime);
            result.Add("client_id", "578134");
            result.Add("redirect_uri", "https://web.skype.com");

            return result;
        }

        private static string GetJsTime()
        {
            return (((double)DateTime.Now.ToUnixTimestapm())).ToString("F2");
        }

        private static string GetTimezone()
        {
            //todo implement timezone detection!
            return "+03|00";
        }
    }
}