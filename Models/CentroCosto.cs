using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class CentroCosto
{
    [Key]
    public int CentroCostoId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    public string Codigo { get; set; } = null!;

    [InverseProperty("CentroCosto")]
    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}