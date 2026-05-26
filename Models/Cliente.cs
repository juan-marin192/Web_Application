using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models;

public partial class Cliente
{
    [Key]
    public int ClienteId { get; set; }

    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(50)]
    public string Documento { get; set; } = null!;

    [StringLength(20)]
    public string Telefono { get; set; } = null!;

    [InverseProperty("Cliente")]
    public virtual ICollection<Venta> Ventas { get; set; } = new List<Venta>();
}