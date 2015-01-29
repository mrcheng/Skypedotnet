using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

using Newtonsoft.Json;

using SkypeDotnet.Abstract;
using SkypeDotnet.Model;

namespace SkypeDotnet
{
    public class SkypeClient : ISkypeClient
    {
        private readonly IHttpClient httpClient;
        private readonly string requestToken;

        public SkypeClient(IHttpClient httpClient, SkypeAuthParams authParams)
        {
            this.httpClient = httpClient;
            httpClient.UpdateSharedCustomHeader("X-Skypetoken", authParams.Token);
            this.requestToken = authParams.Token;
        }

        public SkypeSelfProfile GetSelfProfile()
        {
            var response = httpClient.SendGet(SkypeApiUrls.DisplayNameUrl);

            return JsonConvert.DeserializeObject<SkypeSelfProfile>(response.ResponseData);
        }
    }
}