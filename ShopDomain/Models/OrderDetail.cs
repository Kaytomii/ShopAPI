using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ShopDomain.Models;

[Table("order_details")]
public class OrderDetail
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Column("order_id")]
    public Guid OrderId { get; set; }

    [Column("product_id")]
    public Guid ProductId { get; set; }

    [Column("price")]
    public decimal Price { get; set; }

    [Column("count")]
    public int Count { get; set; }

    public Order Order { get; set; }
}