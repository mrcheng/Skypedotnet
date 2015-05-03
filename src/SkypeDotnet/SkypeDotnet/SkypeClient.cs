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
        private readonly string registrationToken;
        private readonly string messagesHost;

        public SkypeClient(IHttpClient httpClient, SkypeAuthParams authParams)
        {
            this.httpClient = httpClient;
            httpClient.UpdateSharedCustomHeader("X-Skypetoken", authParams.Token);
            this.requestToken = authParams.Token;
            this.registrationToken = authParams.RegistrationToken;
            this.messagesHost = authParams.MessagesHost;
        }

        public SkypeSelfProfile GetSelfProfile()
        {
            return SendAndDeserialize<SkypeSelfProfile>(SkypeApiUrls.DisplayNameUrl);
        }

        public IEnumerable<SkypeProfile> SearchContacts(string query)
        {
            return SendAndDeserialize<IEnumerable<SkypeProfile>>(SkypeApiUrls.SearchContactsUrlWithQuery(query), new SkypeProfileConverter());
        }

        public IEnumerable<SkypeFriend> GetFriends()
        {
            return SendAndDeserialize<IEnumerable<SkypeFriend>>(SkypeApiUrls.FriendsListUrl);
        }

        public IEnumerable<SkypeAuthRequest> GetAuthRequests()
        {
            return SendAndDeserialize<IEnumerable<SkypeAuthRequest>>(SkypeApiUrls.AuthRequestsUrl);
        }

        public void AcceptAuthRequest(string skypeName)
        {
            httpClient.SendPut(SkypeApiUrls.AcceptAuthRequestUrl(skypeName));
        }

        public int CreateSubscription()
        {
            var response = httpClient.SendPost(SkypeApiUrls.SubscriptionsUrl(messagesHost),
                JObject.FromObject(new
                {
                    channelType = "httpLongPoll",
                    interestedResources = new string[]
                    {
                        "/v1/users/ME/conversations/ALL/properties",
                        "/v1/users/ME/conversations/ALL/messages",
                        "/v1/users/ME/contacts/ALL",
                        "/v1/threads/ALL"
                    },
                    template = "raw"
                }),
                new Dictionary<string, string>
                {
                    { "RegistrationToken", registrationToken }
                });
            string location = response.ResponseHeaders["Location"];
            return int.Parse(location.Substring(location.LastIndexOf('/') + 1));
        }

        public SkypePoll PollSubscription(int subscription)
        {
            var response = httpClient.SendPost(SkypeApiUrls.PollSubscriptionUrl(messagesHost, subscription),
                "", "application/json",
                new Dictionary<string, string>
                {
                    { "RegistrationToken", registrationToken }
                });
            if (String.IsNullOrEmpty(response.ResponseData))
            {
                return null;
            }
            return JsonConvert.DeserializeObject<SkypePoll>(response.ResponseData);
        }

        public void SendMessage(string conversation, string content)
        {
            httpClient.SendPost(SkypeApiUrls.ConversationMessagesUrl(messagesHost, conversation),
                JObject.FromObject(new
                {
                    clientmessageid = "",
                    content = content,
                    messagetype = "RichText",
                    contenttype = "text"
                }),
                new Dictionary<string, string>
                {
                    { "RegistrationToken", registrationToken }
                });
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