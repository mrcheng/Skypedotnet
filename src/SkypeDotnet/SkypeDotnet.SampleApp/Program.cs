using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            credentials.Password = Console.ReadLine();

            var client = SkypeClient.Login(credentials);
        }
    }
}
