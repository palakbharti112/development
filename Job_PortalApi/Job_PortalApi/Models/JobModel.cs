namespace Job_PortalApi.Models
{
    public class JobModel
    {
        public int Id { get; set; }

        public int? EmployerId { get; set; }

        public string? JobTitle { get; set; }

        public string? JobDiscription { get; set; }

        public string? Location { get; set; }

        public string? ComapnyName { get; set; }

        public long? SalaryRange { get; set; }

        public string? JobType { get; set; }
    }
}
