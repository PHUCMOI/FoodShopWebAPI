using System;
using System.Collections.Generic;

namespace Fooding_Shop.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? ProductCategory { get; set; }

    public string? Description { get; set; }

    public decimal? Price { get; set; }

    public string? ImgUrl { get; set; }
    public string? HiddenImgUrl { get; set; }

    public int? CreateBy { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? UpdateBy { get; set; }

    public DateTime? UpdateDate { get; set; }

    public bool? IsDeleted { get; set; }
}
