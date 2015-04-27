using System;
using System.Collections.Generic;

namespace SkypeDotnet.Model
{
    public class SkypePoll
    {
        public IEnumerable<SkypeEventMessage> EventMessages { get; set; }
    }

    public class SkypeEventMessage
    {
        public int Id { get; set; }

        public string Type { get; set; }

        public string ResourceType { get; set; }

        public DateTime Time { get; set; }

        public string ResourceLink { get; set; }

        public SkypeResource Resource { get; set; }
    }

    public class SkypeResource
    {
        public string ClientMessageId { get; set; }

        public string ThreadTopic { get; set; }

        public string MessageType { get; set; }

        public DateTime OriginalArrivalTime { get; set; }

        public bool IsActive { get; set; }

        public string From { get; set; }

        public string Id { get; set; }

        public string ConversationLink { get; set; }

        public string Type { get; set; }

        public string IMDisplayName { get; set; }

        public string AckRequired { get; set; }

        public string Content { get; set; }

        public DateTime ComposeTime { get; set; }
    }
}
