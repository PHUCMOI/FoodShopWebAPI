using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ModelLayer.Models;

namespace Fooding_Shop.Models;

public partial class FoodingShopContext : DbContext
{
    private IConfiguration _configuration;
    public FoodingShopContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public FoodingShopContext(DbContextOptions<FoodingShopContext> options, IConfiguration configuration)
        : base(options)
    {
        _configuration = configuration;
    }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Cart> Cart { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(_configuration.GetConnectionString("FoodingShopDB"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity
               .HasNoKey()
               .ToTable("Cart");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserId");
            entity.Property(e => e.ProductID).HasColumnName("ProductID");
            entity.Property(e => e.UserName).HasColumnName("UserName");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A2BCF6FEC45");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryId)
                .ValueGeneratedNever()
                .HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF8E310B3D");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId)
                .ValueGeneratedNever()
                .HasColumnName("OrderID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Message).HasMaxLength(255);
            entity.Property(e => e.PayMethod)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.TotalPrice).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("OrderDetail");

            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6ED5DCFBDFE");

            entity.Property(e => e.ProductId)
                .ValueGeneratedNever()
                .HasColumnName("ProductID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(255);
            entity.Property(e => e.ImgUrl)
                .HasMaxLength(255)
                .HasColumnName("ImgURL");
            entity.Property(e => e.Price).HasColumnType("money");
            entity.Property(e => e.ProductCategory).HasMaxLength(255);
            entity.Property(e => e.ProductName).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACF56C9012");

            entity.ToTable("User");

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("UserID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.Password).HasMaxLength(255);
            entity.Property(e => e.PhoneNumber).HasMaxLength(255);
            entity.Property(e => e.Role)
                .HasMaxLength(1)
                .IsUnicode(false);
            entity.Property(e => e.Status).HasMaxLength(255);
            entity.Property(e => e.UpdateDate).HasColumnType("datetime");
            entity.Property(e => e.UserName).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
