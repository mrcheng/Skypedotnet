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

            /*
             SkypeWebAccount *sa = user_data;
        PurpleAccount *account = sa->account;
        gchar *pie;
        gchar *etm;
        const gchar *login_url = "https://" SKYPEWEB_LOGIN_HOST;// "/login?client_id=578134&redirect_uri=https%3A%2F%2Fweb.skype.com";
        GString *postdata;
        gchar *request;
        struct timezone tz;
        guint tzhours, tzminutes;
        
        gettimeofday(NULL, &tz);
        tzminutes = tz.tz_minuteswest;
        if (tzminutes < 0) tzminutes = -tzminutes;
        tzhours = tzminutes / 60;
        tzminutes -= tzhours * 60;
        
        pie = skypeweb_string_get_chunk(url_text, len, "=\"pie\" value=\"", "\"");
        if (!pie) {
                return;
        }
        
        etm = skypeweb_string_get_chunk(url_text, len, "=\"etm\" value=\"", "\"");
        if (!etm) {
                return;
        }
        
        
        postdata = g_string_new("");
        g_string_append_printf(postdata, "username=%s&", purple_url_encode(purple_account_get_username(account)));
        g_string_append_printf(postdata, "password=%s&", purple_url_encode(purple_account_get_password(account)));
        g_string_append_printf(postdata, "timezone_field=%c|%d|%d&", (tz.tz_minuteswest < 0 ? '+' : '-'), tzhours, tzminutes);
        g_string_append_printf(postdata, "pie=%s&", purple_url_encode(pie));
        g_string_append_printf(postdata, "etm=%s&", purple_url_encode(etm));
        g_string_append_printf(postdata, "js_time=%" G_GINT64_FORMAT "&", skypeweb_get_js_time());
        g_string_append(postdata, "client_id=578134");
        g_string_append(postdata, "redirect_uri=https://web.skype.com/");
        
        request = g_strdup_printf("POST /login?client_id=578134&redirect_uri=https%%3A%%2F%%2Fweb.skype.com HTTP/1.0\r\n"
                        "Connection: close\r\n"
                        "Accept: /*\r\n"
                        "BehaviorOverride: redirectAs404\r\n"
                        "Host: " SKYPEWEB_LOGIN_HOST "\r\n"
                        "Content-Type: application/x-www-form-urlencoded; charset=UTF-8\r\n"
                        "Content-Length: %" G_GSIZE_FORMAT "\r\n\r\n%s",
                        strlen(postdata->str), postdata->str);
        
        purple_util_fetch_url_request(sa->account, login_url, TRUE, NULL, FALSE, request, TRUE, 524288, skypeweb_login_did_auth, sa);

        g_string_free(postdata, TRUE);
        g_free(request);
        
        purple_connection_update_progress(sa->pc, _("Authenticating"), 2, 4);
             */

            var httpClient = new HttpClient();

            var response = httpClient.SendGet(new Uri(Constants.SkypeWebLoginUrlFull));

            var postParameters = GetPostParameters(response.ResponseData);

            postParameters.Add("username", credentials.UserName);
            postParameters.Add("password", credentials.Password);

            var loginUrl = new Uri(Constants.SkypeWebLoginUrlFull + "?client_id=" + postParameters["client_id"] + "&redirect_uri=https%%3A%%2F%%2Fweb.skype.com");

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