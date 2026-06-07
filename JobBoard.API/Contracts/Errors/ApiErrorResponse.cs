namespace JobBoard.API.Contracts.Errors
{
    public sealed class ApiErrorResponse
    {
        public string TraceId { get; init; } = default!;
        public int Status { get; init; }
        public string Title { get; init; } = default!;
        public string? Detail { get; init; }
        public IDictionary<string, string[]>? Errors { get; init; }
    }
}
