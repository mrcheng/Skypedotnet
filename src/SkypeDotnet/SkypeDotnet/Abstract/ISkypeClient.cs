using SkypeDotnet.Model;
using System.Collections.Generic;

namespace SkypeDotnet.Abstract
{
    public interface ISkypeClient
    {
        SkypeSelfProfile GetSelfProfile();

        IEnumerable<SkypeProfile> SearchContacts(string query);

        IEnumerable<SkypeFriend> GetFriends();

        IEnumerable<SkypeAuthRequest> GetAuthRequests();

        void AcceptAuthRequest(string skypeName);

        int CreateSubscription();

        SkypePoll PollSubscription(int subscription);

        void SendMessage(string conversation, string content);
    }
}