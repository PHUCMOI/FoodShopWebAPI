using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Fooding_Shop.Models;

public partial class OrderDetail
{
    public int? OrderId { get; set; }

    public int? ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }
}
