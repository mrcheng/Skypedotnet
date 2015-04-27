using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Newtonsoft.Json;

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
            Console.WriteLine();

            Abstract.IHttpClient httpClient = new HttpClient();
            var token = new SkypeLoginManager(httpClient, new SkypeHmacChipher()).Login(credentials);

            Abstract.ISkypeClient client = new SkypeClient(httpClient, token);

            Console.WriteLine("Fetching friends list");
            var friends = client.GetFriends();
            foreach (var friend in friends)
            {
                Console.WriteLine(friend.FullName + " (" + friend.SkypeName + ")");
            }

            Console.WriteLine("Fetching auth requests");
            var authRequests = client.GetAuthRequests();
            foreach (var authRequest in authRequests)
            {
                Console.WriteLine("Accepting auth request from " +  authRequest.Sender);
                client.AcceptAuthRequest(authRequest.Sender);
            }

            Console.WriteLine("Setting up subscription");
            int subscription = client.CreateSubscription();

            for (;;)
            {
                Console.WriteLine("Polling subscription");
                var poll = client.PollSubscription(subscription);
                if (poll != null && poll.EventMessages != null)
                {
                    foreach (var msg in poll.EventMessages)
                    {
                        if (msg.ResourceType == "NewMessage")
                        {
                            string from = msg.Resource.From.Substring(msg.Resource.From.LastIndexOf("8:") + 2);
                            if (from != credentials.UserName)
                            {
                                Console.WriteLine("Message from {0}: {1}", from, msg.Resource.Content);
                                string conversation = msg.Resource.ConversationLink.Substring(
                                    msg.Resource.ConversationLink.LastIndexOf('/') + 1);
                                client.SendMessage(conversation, msg.Resource.Content);
                            }
                        }
                    }
                }
            }
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
