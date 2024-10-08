using System;
using System.Collections.Generic;

namespace Job_PortalApi.Models;

public partial class Job
{
    public int Id { get; set; }

    public int? EmployerId { get; set; }

    public string? JobTitle { get; set; }

    public string? JobDiscription { get; set; }

    public string? Location { get; set; }

    public string? ComapnyName { get; set; }

    public long? SalaryRange { get; set; }

    public string? JobType { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User? Employer { get; set; }

    public virtual ICollection<JobApplication> JobApplications { get; set; } = new List<JobApplication>();
}
