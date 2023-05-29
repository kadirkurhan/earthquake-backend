using System;
namespace Earthquake.Emergency.Domain.Entities.Emergency
{
	public class EmergencyEntity
	{
		public int Id { get; set; }
        public int UserId { get; set; }
        public int ReportedByUserId { get; set; }
        public float Longidute { get; set; }
        public float Latidute { get; set; }
    }
}

