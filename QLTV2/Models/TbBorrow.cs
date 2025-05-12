using System;
using System.Collections.Generic;

namespace QLTV2.Models;

public partial class TbBorrow
{
    public int BorrowId { get; set; }

    public int? AccountId { get; set; }

    public DateTime BorrowDate { get; set; }

    public DateTime? ReturnDate { get; set; }

    public string? Status { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual ICollection<TbBorrowDetail> TbBorrowDetails { get; set; } = new List<TbBorrowDetail>();
}
