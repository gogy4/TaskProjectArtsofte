namespace Domain.Entity
{
    public class Job
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int? AssignedUserId { get; set; }
        public int Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDeleted { get; set; }
        public int CreatedUserId { get; set; }

        public readonly List<JobHistory> History = new();

        public Job(int id, string title, string description, int status, DateTime createdAt, DateTime updatedAt,
            bool isDeleted, int createdUserId, int? assignedUserId = null)
        {
            Id = id;
            Title = title;
            Description = description;
            AssignedUserId = assignedUserId;
            Status = status;
            CreatedAt = createdAt;
            UpdatedAt = updatedAt;
            IsDeleted = isDeleted;
            CreatedUserId = createdUserId;
        }

        public Job()
        {
        }
    }
}