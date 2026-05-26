using System;
using System.Collections.Generic;

namespace WebApplication1.Models;

public class VentaPOSViewModel
{
    public int? ClienteId { get; set; }
    public int? CentroCostoId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal Total { get; set; }
    public List<DetalleVentaDTO> Detalles { get; set; } = new();
}

public class DetalleVentaDTO
{
    public int ProductId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
}