using System;

namespace SkypeDotnet
{
    public class SkypeAuthParams
    {
        public string Token { get; set; }

        public string RegistrationToken { get; set; }

        public Guid EndpointId { get; set; }

        public string MessagesHost { get; set; }
    }
}