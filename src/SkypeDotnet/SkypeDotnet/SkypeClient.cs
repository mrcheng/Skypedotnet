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
            return SendAndDeserialize<SkypeSelfProfile>(SkypeApiUrls.DisplayNameUrl);
        }

        public IEnumerable<SearchInfo> SearchContacts(string query)
        {
            return SendAndDeserialize<IEnumerable<SearchInfo>>(SkypeApiUrls.SearchContactsUrlWithQuery(query));
        }

        private T SendAndDeserialize<T>(Uri url)
        {
            var response = httpClient.SendGet(url);

            return JsonConvert.DeserializeObject<T>(response.ResponseData);
        }
    }
}