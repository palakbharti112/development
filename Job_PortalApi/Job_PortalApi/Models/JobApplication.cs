using System;
using System.Collections.Generic;

namespace Job_PortalApi.Models;

public partial class JobApplication
{
    public int ApplicationId { get; set; }

    public int? JobId { get; set; }

    public int? UserId { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual Job? Job { get; set; }

    public virtual User? User { get; set; }
}
