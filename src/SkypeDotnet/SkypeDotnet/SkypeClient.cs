using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace SkypeDotnet
{
    public class SkypeClient
    {
        private readonly IHttpClient httpClient;
        private readonly string requestToken;

        public SkypeClient(IHttpClient httpClient, string requestToken)
        {
            this.httpClient = httpClient;
            this.requestToken = requestToken;
        }
    }
}