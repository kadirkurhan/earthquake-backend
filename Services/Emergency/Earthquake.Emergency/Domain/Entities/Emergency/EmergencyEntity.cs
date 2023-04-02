using System;
namespace Earthquake.Emergency.Domain.Entities.Emergency
{
	public class EmergencyEntity
	{
		public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportedByUserId { get; set; }
        public string Longidute { get; set; }
        public string Latidute { get; set; }
    }
}

