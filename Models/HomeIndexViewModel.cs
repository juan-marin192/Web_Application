using System.Collections.Generic;

namespace WebApplication1.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Category> Categories { get; set; } = new List<Category>();
        public IEnumerable<Product> Products { get; set; } = new List<Product>();

        // --- NUEVAS PROPIEDADES ---
        public IEnumerable<Cliente> Clientes { get; set; } = new List<Cliente>();
        public IEnumerable<CentroCosto> CentrosCostos { get; set; } = new List<CentroCosto>();
        public IEnumerable<Venta> Ventas { get; set; } = new List<Venta>();
    }
}