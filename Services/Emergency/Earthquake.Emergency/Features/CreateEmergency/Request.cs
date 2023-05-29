using System;
using System.Text.Json.Serialization;

namespace Earthquake.Emergency.Features.CreateEmergency
{
	public class Request
	{
        public class RequestModel
        {
            [JsonPropertyName("senderUserId")]
            public int SenderUserId { get; set; }
            [JsonPropertyName("receivers")]
            public IEnumerable<Message> Receivers { get; set; }
        }

        public class Message
        {
            [JsonPropertyName("userId")]
            public int UserId { get; set; }
            [JsonPropertyName("receiverName")]
            public string ReceiverName { get; set; }
            [JsonPropertyName("phoneNumber")]
            public string PhoneNumber { get; set; }
            [JsonPropertyName("longidute")]
            public float Longidute { get; set; }
            [JsonPropertyName("latidute")]
            public float Latidute { get; set; }
            [JsonPropertyName("time")]
            public string Time { get; set; }
        }
    }
}

