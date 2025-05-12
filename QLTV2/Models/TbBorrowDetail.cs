using System;
using System.Collections.Generic;

namespace QLTV2.Models;

public partial class TbBorrowDetail
{
    public int BorrowDetailId { get; set; }

    public int? BorrowId { get; set; }

    public int? BookId { get; set; }

    public int? Quantity { get; set; }

    public virtual TbBook? Book { get; set; }

    public virtual TbBorrow? Borrow { get; set; }
}
