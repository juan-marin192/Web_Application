using System;
using System.Collections.Generic; // <-- Asegúrate de tener este using para las listas
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class Venta
{
    [Key]
    public int VentaId { get; set; }

    public DateTime Fecha { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; }

    public int? ClienteId { get; set; }

    public int? CentroCostoId { get; set; }

    [ForeignKey("ClienteId")]
    [InverseProperty("Ventas")]
    public virtual Cliente? Cliente { get; set; }

    [ForeignKey("CentroCostoId")]
    [InverseProperty("Ventas")]
    public virtual CentroCosto? CentroCosto { get; set; }

    // NUEVO: Esto conecta la venta con la lista de productos que se van a agregar
    [InverseProperty("Venta")]
    public virtual ICollection<DetalleVenta> Detalles { get; set; } = new List<DetalleVenta>();
}