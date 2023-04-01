namespace Earthquake.Emergency.Models.Shared
{
    public record ResponseModel<T>
    {
        public bool IsError { get; init; }
        public T? Payload { get; init; }
        public string? Message { get; init; }
    }
}