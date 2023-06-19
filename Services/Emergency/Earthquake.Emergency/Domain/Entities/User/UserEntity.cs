using System;
using Earthquake.Emergency.Domain.Entities.Emergency;

namespace Earthquake.Emergency.Domain.Entities.User
{
	public class UserEntity
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Address { get; set; }
		public EmergencyEntity Emergency { get; set; }
	}
}

