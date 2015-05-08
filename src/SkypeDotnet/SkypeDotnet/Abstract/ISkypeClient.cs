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

        void GetConversations();

        int CreateSubscription();

        SkypePoll PollSubscription(int subscription);

        void SendMessage(string conversation, string content);

        void SetConsumptionHorizon(string conversation, string originalArrivalTime, string clientMessageId);

        void SetEndpointPresence();

        void SetPresence(string status);
    }
}