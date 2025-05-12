using System;
using System.Collections.Generic;

namespace QLTV2.Models;

public partial class TbAccount
{
    public int AccountId { get; set; }

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    public virtual TbRole? Role { get; set; }

    public virtual ICollection<TbBookReview> TbBookReviews { get; set; } = new List<TbBookReview>();

    public virtual ICollection<TbBorrow> TbBorrows { get; set; } = new List<TbBorrow>();
}
