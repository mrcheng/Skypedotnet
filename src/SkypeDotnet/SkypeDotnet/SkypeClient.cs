using System;

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

            throw new NotImplementedException();
        }
    }
}