using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Necesario para Identity
using Microsoft.AspNetCore.Identity; // Necesario para IdentityUser
using WebApplication1.Models;

namespace WebApplication1.Data;

// Cambiamos de DbContext a IdentityDbContext<IdentityUser>
public partial class SampleDbContext : IdentityDbContext<IdentityUser>
{
    public SampleDbContext()
    {
    }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    // Tus tablas del sistema POS
    public virtual DbSet<Category> Categories { get; set; }
    public virtual DbSet<CentroCosto> CentrosCostos { get; set; }
    public virtual DbSet<Cliente> Clientes { get; set; }
    public virtual DbSet<Product> Products { get; set; }
    public virtual DbSet<Venta> Ventas { get; set; }
    public virtual DbSet<DetalleVenta> DetalleVentas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=DESKTOP-UCJMTU8\\SQLEXPRESS;Database=SampleDb;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ESTO ES OBLIGATORIO para que las tablas de Identity se creen
        base.OnModelCreating(modelBuilder);

        // 1. Configuración de Categorías
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__19093A0B5AC9E400");
            entity.ToTable("Categories");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        // 2. Configuración de Centros de Costo
        modelBuilder.Entity<CentroCosto>(entity =>
        {
            entity.HasKey(e => e.CentroCostoId);
            entity.ToTable("CentrosCostos");
            entity.Property(e => e.Codigo).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
        });

        // 3. Configuración de Clientes
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(e => e.ClienteId);
            entity.ToTable("Clientes");
            entity.Property(e => e.Documento).HasMaxLength(50);
            entity.Property(e => e.Nombre).HasMaxLength(100);
            entity.Property(e => e.Telefono).HasMaxLength(20);
        });

        // 4. Configuración de Productos
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Products__B40CC6CD9E1FC148");
            entity.ToTable("Products");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .HasConstraintName("FK__Products__Catego__4BAC3F29");
        });

        // 5. Configuración de Ventas (Cabecera)
        modelBuilder.Entity<Venta>(entity =>
        {
            entity.HasKey(e => e.VentaId);
            entity.ToTable("Ventas");
            entity.Property(e => e.Fecha).HasColumnType("datetime2");
            entity.Property(e => e.Total).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CentroCosto)
                .WithMany(p => p.Ventas)
                .HasForeignKey(d => d.CentroCostoId);

            entity.HasOne(d => d.Cliente)
                .WithMany(p => p.Ventas)
                .HasForeignKey(d => d.ClienteId);
        });

        // 6. Configuración del Detalle de Venta
        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.HasKey(e => e.DetalleVentaId);
            entity.ToTable("DetalleVentas");
            entity.Property(e => e.PrecioUnitario).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Venta)
                .WithMany(p => p.Detalles)
                .HasForeignKey(d => d.VentaId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
