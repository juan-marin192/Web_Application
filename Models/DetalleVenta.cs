using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models;

public partial class DetalleVenta
{
    [Key]
    public int DetalleVentaId { get; set; }

    public int VentaId { get; set; }

    public int ProductId { get; set; }

    public int Cantidad { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal PrecioUnitario { get; set; }

    [ForeignKey("VentaId")]
    [InverseProperty("Detalles")]
    public virtual Venta? Venta { get; set; }

    [ForeignKey("ProductId")]
    public virtual Product? Product { get; set; }
}