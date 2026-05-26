using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly SampleDbContext _context;

        public HomeController(ILogger<HomeController> logger, SampleDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // 1. Consultas originales
            var categories = await _context.Categories.ToListAsync();
            var products = await _context.Products.Include(p => p.Category).ToListAsync();

            // 2. Nuevas consultas para el Tablero
            var clientes = await _context.Clientes.ToListAsync();
            var centrosCostos = await _context.CentrosCostos.ToListAsync();
            var ventas = await _context.Ventas
                .Include(v => v.Cliente)       // Carga el Cliente de la venta para ver su nombre
                .Include(v => v.CentroCosto)   // Carga el Centro de Costo de la venta
                .ToListAsync();

            // 3. Construcción del ViewModel unificado
            var vm = new HomeIndexViewModel
            {
                Categories = categories,
                Products = products,
                Clientes = clientes,
                CentrosCostos = centrosCostos,
                Ventas = ventas
            };

            return View(vm);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}