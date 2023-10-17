using Models_Layer.ModelRequest;
using System;
using System.Collections.Generic;

namespace Fooding_Shop.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Address { get; set; }

    public string? TotalPrice { get; set; }

    public string? PayMethod { get; set; }

    public string? Status { get; set; }

    public string? Message { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }
}
