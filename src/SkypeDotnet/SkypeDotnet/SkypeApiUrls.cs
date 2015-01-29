using System;

namespace SkypeDotnet
{
    public class SkypeApiUrls
    {

        public const string LoginPath = "/login";

        public const string DisplayNamePath = "/users/self/displayname";

        public const string EndpointsPath = "/v1/users/ME/endpoints";

        public const string SearchPath = "/search/users/any";

        public const string AuthRequestsPath = "/users/self/contacts/auth-request";

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

        public static readonly Uri SearchContactsUrl = UrlForContactsHost(SearchPath);

        public static readonly Uri AuthRequestsUrl = UrlForContactsHost(AuthRequestsPath);

        public static readonly Uri DisplayNameUrl = UrlForContactsHost(DisplayNamePath);

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

        private static Uri UrlForLoginHost(string path)
        {
            return new Uri(Protocol + LoginHost + path);
        }
    }
}