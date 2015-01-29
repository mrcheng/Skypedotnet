using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace SkypeDotnet.SampleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var credentials = new LoginCredentials();
            Console.Write("user: ");
            credentials.UserName = Console.ReadLine();
            Console.Write("pass: ");
            credentials.Password = SecureReadPassword();

            var httpClient = new HttpClient();
            var token = new SkypeLoginManager(httpClient, new SkypeHmacChipher()).Login(credentials);
            
            var customHeaders = new Dictionary<string, string> { {"X-Skypetoken", token.Token} };

            var response = httpClient.SendGet(SkypeApiUrls.DisplayNameUrl, customHeaders);

            response = httpClient.SendGet(SkypeApiUrls.AuthRequestsUrl, customHeaders);

            response = httpClient.SendGet(SkypeApiUrls.SearchContactsUrlWithQuery("querty"), customHeaders);

            var client = new SkypeClient(httpClient, token);
        }

        static string SecureReadPassword()
        {
            var str = new StringBuilder();
            var newKey = Console.ReadKey(true);
            while (newKey.Key != ConsoleKey.Enter)
            {
                if (newKey.Key == ConsoleKey.Backspace)
                {
                    str.Remove(str.Length - 1, 1);
                }
                str.Append(newKey.KeyChar);
                
                newKey = Console.ReadKey(true);
            }
            return str.ToString();
        }
    }
}
