using System;
using System.Collections.Generic;
using System.Linq;
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

            var client = SkypeClient.Login(credentials);
        }

        static string SecureReadPassword()
        {
            var str = new StringBuilder();
            var newKey = Console.ReadKey(true);
            while (newKey.Key != ConsoleKey.Enter)
            {
                str.Append(newKey.KeyChar);
                newKey = Console.ReadKey(true);
            }
            return str.ToString();
        }
    }
}
