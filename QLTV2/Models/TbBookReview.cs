using System;
using System.Collections.Generic;

namespace QLTV2.Models;

public partial class TbBookReview
{
    public int BookReviewId { get; set; }

    public int? AccountId { get; set; }

    public int? BookId { get; set; }

    public int? Rating { get; set; }

    public string? Comment { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual TbAccount? Account { get; set; }

    public virtual TbBook? Book { get; set; }
}
