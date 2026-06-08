namespace JobBoard.Domain.Entities
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string Location { get; set; }
        public DateTime CreatedAtUtc { get; set; }
        public DateTime? UpdatedAtUtc { get; set; }


        public ICollection<Job> Jobs { get; set; } = new List<Job>();
    }
}
