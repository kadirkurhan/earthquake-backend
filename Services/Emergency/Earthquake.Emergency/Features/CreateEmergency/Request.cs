using System;
namespace Earthquake.Emergency.Features.CreateEmergency
{
	public class Request
	{
        public class RequestModel
        {
            public int SenderUserId { get; set; }
            public IEnumerable<Message> Receivers { get; set; }
        }

        public class Message
        {
            public int UserId { get; set; }
            public string ReceiverName { get; set; }
            public string PhoneNumber { get; set; }
            public string Longidute { get; set; }
            public string Latidute { get; set; }
            public string Time { get; set; }
        }
    }
}

