namespace Earthquake.Emergency.Entities
{
    public class Emergency
    {
        public Int64 Id { get; set; }
        public Int64 UserId { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public bool IsActive { get; set; }
    }
}