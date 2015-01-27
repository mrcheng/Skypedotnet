using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

using SkypeDotnet.Abstract;

namespace SkypeDotnet
{
    public class SkypeClient : ISkypeClient
    {
        private readonly IHttpClient httpClient;
        private readonly string requestToken;

        public SkypeClient(IHttpClient httpClient, string requestToken)
        {
            this.httpClient = httpClient;
            this.requestToken = requestToken;
        }

        public SkypeProfile GetSelfProfile()
        {
            throw new NotImplementedException();
        }
    }

    public interface ISkypeClient
    {
        SkypeProfile GetSelfProfile();
    }

    public class SkypeProfile
    {
        
    }
}