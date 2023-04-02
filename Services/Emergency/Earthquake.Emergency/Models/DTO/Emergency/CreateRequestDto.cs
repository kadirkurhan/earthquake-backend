using System;
using System.Text.Json.Serialization;

namespace Earthquake.Emergency.Models.DTO.Emergency
{
    public class CreateRequestDto
    {
        public string SenderName { get; set; }
        public IEnumerable<MessageDto> Receivers { get; set; }
    }

    public class MessageDto
    {
        public int UserId { get; set; }
        public string ReceiverName { get; set; }
        public string PhoneNumber { get; set; }
        public string Longidute { get; set; }
        public string Latidute { get; set; }
        public string Time { get; set; }
    }
}

