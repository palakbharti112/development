using System;
using System.Collections.Generic;

namespace Job_PortalApi.Models;

public partial class Employer
{
    public int Id { get; set; }

    public string? EmployerName { get; set; }

    public string? CompanyDetail { get; set; }

    public int? UserId { get; set; }

    public string? Location { get; set; }

    public string? GstNumber { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public bool? Isdeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? User { get; set; }
}
