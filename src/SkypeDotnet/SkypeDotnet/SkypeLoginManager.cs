using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Web.Management;
using System.Web.UI.WebControls;

using HtmlAgilityPack;

using SkypeDotnet.Abstract;
using SkypeDotnet.Utils;

namespace SkypeDotnet
{
    public class SkypeLoginManager : ISkypeLoginManager
    {
        private readonly IHttpClient httpClient;

        public SkypeLoginManager(IHttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public SkypeAuthParams Login(LoginCredentials credentials)
        {
            var response = httpClient.SendGet(new Uri(Constants.SkypeWebLoginUrlFull));

            var postParameters = GetPostParameters(response.ResponseData);

            postParameters.Add("username", credentials.UserName);
            postParameters.Add("password", credentials.Password);

            var loginUrl = new Uri(Constants.SkypeWebLoginUrlFull + "?client_id=" + postParameters["client_id"] + "&redirect_uri=https://web.skype.com");

            response = httpClient.SendPost(loginUrl, postParameters);

            var authParams = new SkypeAuthParams();
            authParams.Token = GetToken(response.ResponseData);

            var customHeaders = new Dictionary<string, string>();
            
            //todo get lockandkey!
            customHeaders["LockAndKey"] = GetLockAndKey();

            //todo get registration tocken!
            
            throw new NotImplementedException();

        }

        private string GetLockAndKey()
        {
            //appId=" SKYPEWEB_LOCKANDKEY_APPID "; time=%s; lockAndKeyResponse=%s\r\n"
            var resultString = "appId={0}; time={1}; lockAndKeyResponse={2}";
            return string.Format(resultString, Constants.SkypewebLockandkeyAppid, (int)DateTime.Now.ToUnixTimestapm(), string.Empty);
        }

        private string GetToken(string responseData)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(responseData);

            var node = doc.DocumentNode.SelectSingleNode("//form[@id=\"redirectForm\"]").SelectSingleNode("//input[@name=\"skypetoken\"]");
            return node.Attributes["value"].Value;
        }

        private static Dictionary<string, string> GetPostParameters(string responseData)
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
            return DateTime.Now.ToUniversalTime().ToUnixTimestapm().ToString("F2");
        }

        private static string GetTimezone()
        {
            //todo implement timezone detection!
            return "+03|00";
        }
    }
}