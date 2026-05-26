using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class VentasController : Controller
    {
        private readonly SampleDbContext _context;

        public VentasController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: Ventas
        public async Task<IActionResult> Index()
        {
            var sampleDbContext = _context.Ventas.Include(v => v.CentroCosto).Include(v => v.Cliente);
            return View(await sampleDbContext.ToListAsync());
        }

        // GET: Ventas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.CentroCosto)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // GET: Ventas/Create
        public IActionResult Create()
        {
            // Cambiados los campos de texto para mostrar "Nombre" en lugar del ID numérico
            ViewData["CentroCostoId"] = new SelectList(_context.CentrosCostos, "CentroCostoId", "Nombre");
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre");

            // Carga la lista de productos con sus precios de catálogo para el selector dinámico de JavaScript
            ViewBag.ProductosSelectList = _context.Products
                .Select(p => new {
                    Value = p.ProductId,
                    Text = p.Name,
                    Precio = p.Price
                }).ToList();

            return View();
        }

        // POST: Ventas/CreatePOS (Lógica del Punto de Venta Dinámico)
        [HttpPost]
        public async Task<IActionResult> CreatePOS([FromBody] VentaPOSViewModel model)
        {
            if (model == null || model.Detalles == null || !model.Detalles.Any())
            {
                return BadRequest("La venta debe contener al menos un producto en el detalle.");
            }

            // 1. Crear y registrar la cabecera maestro de la Venta
            var nuevaVenta = new Venta
            {
                ClienteId = model.ClienteId,
                CentroCostoId = model.CentroCostoId,
                Fecha = model.Fecha,
                Total = model.Total
            };

            _context.Ventas.Add(nuevaVenta);
            await _context.SaveChangesAsync(); // SQL Server genera e inserta automáticamente el VentaId aquí

            // 2. Mapear e insertar el lote completo de detalles vinculados
            foreach (var item in model.Detalles)
            {
                var detalle = new DetalleVenta
                {
                    VentaId = nuevaVenta.VentaId, // Llave foránea sincronizada
                    ProductId = item.ProductId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.PrecioUnitario
                };
                _context.DetalleVentas.Add(detalle);
            }

            await _context.SaveChangesAsync(); // Impacta la base de datos con los registros del detalle

            return Ok();
        }

        // GET: Ventas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas.FindAsync(id);
            if (venta == null)
            {
                return NotFound();
            }
            ViewData["CentroCostoId"] = new SelectList(_context.CentrosCostos, "CentroCostoId", "Nombre", venta.CentroCostoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", venta.ClienteId);
            return View(venta);
        }

        // POST: Ventas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("VentaId,Fecha,Total,ClienteId,CentroCostoId")] Venta venta)
        {
            if (id != venta.VentaId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(venta);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!VentaExists(venta.VentaId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CentroCostoId"] = new SelectList(_context.CentrosCostos, "CentroCostoId", "Nombre", venta.CentroCostoId);
            ViewData["ClienteId"] = new SelectList(_context.Clientes, "ClienteId", "Nombre", venta.ClienteId);
            return View(venta);
        }

        // GET: Ventas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var venta = await _context.Ventas
                .Include(v => v.CentroCosto)
                .Include(v => v.Cliente)
                .FirstOrDefaultAsync(m => m.VentaId == id);
            if (venta == null)
            {
                return NotFound();
            }

            return View(venta);
        }

        // POST: Ventas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var venta = await _context.Ventas.FindAsync(id);
            if (venta != null)
            {
                _context.Ventas.Remove(venta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool VentaExists(int id)
        {
            return _context.Ventas.Any(e => e.VentaId == id);
        }
    }
}
