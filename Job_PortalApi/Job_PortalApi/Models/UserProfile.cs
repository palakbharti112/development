using System;
using System.Collections.Generic;

namespace Job_PortalApi.Models;

public partial class UserProfile
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Resume { get; set; }

    public string? Bio { get; set; }

    public string? Location { get; set; }

    public string? Skills { get; set; }

    public string? Experience { get; set; }

    public DateTime? CreatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }

    public virtual User? User { get; set; }
}
