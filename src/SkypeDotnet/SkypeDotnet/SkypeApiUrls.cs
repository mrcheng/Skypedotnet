using System;
using System.Linq.Expressions;

namespace SkypeDotnet
{
    public class SkypeApiUrls
    {

        public const string LoginPath = "/login";

        public const string DisplayNamePath = "/users/self/displayname";

        public const string EndpointsPath = "/v1/users/ME/endpoints";

        public const string SubscriptionsPath = "/v1/users/ME/endpoints/SELF/subscriptions";

        public const string PollSubscriptionPath = "/v1/users/ME/endpoints/SELF/subscriptions/{0}/poll";

        public const string ConversationMessagesPath = "/v1/users/ME/conversations/{0}/messages";

        public const string SearchPath = "/search/users/any";

        public const string AuthRequestsPath = "/users/self/contacts/auth-request";

        public const string AcceptAuthRequestPath = "/users/self/contacts/auth-request/{0}/accept";

        public const string FriendProfilesPath = "/users/self/contacts/profiles";

        public const string FriendListPath = "/users/self/contacts";

        public const string ContactsHost = "api.skype.com";

        public const string MessagesHost = "client-s.gateway.messenger.live.com";

        public const string LoginHost = "login.skype.com";

        public const string VideomailHost = "vm.skype.com";       
        
        public const string Https = "https://";

        public const string Http = "http://";

        public const string Protocol = Https;

        public static readonly Uri WebLoginUrl = UrlForLoginHost(LoginPath);

        public static Uri WebLoginUrlWithQuery(string clientId)
        {
            return new Uri(string.Format("{0}?client_id={1}&redirect_uri=https://web.skype.com",WebLoginUrl, clientId));
        }

        public static readonly Uri EndpointsUrl = UrlForMessagesHost(EndpointsPath);

        public static Uri SubscriptionsUrl(string messagesHost)
        {
            return UrlForMessagesHost(messagesHost, SubscriptionsPath);
        }

        public static Uri PollSubscriptionUrl(string messagesHost, int subscription)
        {
            string path = String.Format(PollSubscriptionPath, subscription);
            return UrlForMessagesHost(messagesHost, path);
        }

        public static Uri ConversationMessagesUrl(string messagesHost, string conversation)
        {
            string path = String.Format(ConversationMessagesPath, conversation);
            return UrlForMessagesHost(messagesHost, path);
        }

        public static readonly Uri SearchContactsUrl = UrlForContactsHost(SearchPath);

        public static readonly Uri AuthRequestsUrl = UrlForContactsHost(AuthRequestsPath);

        public static Uri AcceptAuthRequestUrl(string skypeName)
        {
            string path = String.Format(AcceptAuthRequestPath, skypeName);
            return UrlForContactsHost(path);
        }

        public static readonly Uri DisplayNameUrl = UrlForContactsHost(DisplayNamePath);

        public static readonly Uri FriendsListUrl = UrlForContactsHost(FriendListPath);

        public static readonly Uri FriendProfilesUrl = UrlForContactsHost(FriendProfilesPath);

        public static Uri SearchContactsUrlWithQuery(string keyword)
        {
            return new Uri(string.Format("{0}?keyWord={1}&contacttypes[]=skype&", SearchContactsUrl, keyword));
        }

        private static Uri UrlForContactsHost(string path)
        {
            return new Uri(Protocol + ContactsHost + path);
        }

        private static Uri UrlForMessagesHost(string path)
        {
            return new Uri(Protocol + MessagesHost + path);
        }

        private static Uri UrlForMessagesHost(string messagesHost, string path)
        {
            return new Uri(Protocol + messagesHost + path);
        }

        private static Uri UrlForLoginHost(string path)
        {
            return new Uri(Protocol + LoginHost + path);
        }
    }
}