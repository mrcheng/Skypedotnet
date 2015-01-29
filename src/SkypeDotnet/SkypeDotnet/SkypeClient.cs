using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using HtmlAgilityPack;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        public IEnumerable<SkypeProfile> SearchContacts(string query)
        {
            return SendAndDeserialize<IEnumerable<SkypeProfile>>(SkypeApiUrls.SearchContactsUrlWithQuery(query), new SkypeProfileConverter());
        }

        private T SendAndDeserialize<T>(Uri url)
        {
            var response = httpClient.SendGet(url);

            return JsonConvert.DeserializeObject<T>(response.ResponseData);
        }

        private T SendAndDeserialize<T>(Uri url, JsonConverter converter)
        {
            var response = httpClient.SendGet(url);

            return JsonConvert.DeserializeObject<T>(response.ResponseData, converter);
        }
    }

    public class SkypeProfileConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jObject = JObject.Load(reader);
            var target = new SkypeProfile();

            target.CurrentLocation = new Location();
            serializer.Populate(jObject["ContactCards"]["Skype"].CreateReader(), target);
            serializer.Populate(jObject["ContactCards"]["CurrentLocation"].CreateReader(), target.CurrentLocation);

            return target;
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof (SkypeProfile);
        }
    }
}