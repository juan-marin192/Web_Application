using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    public class CentroCostoesController : Controller
    {
        private readonly SampleDbContext _context;

        public CentroCostoesController(SampleDbContext context)
        {
            _context = context;
        }

        // GET: CentroCostoes
        public async Task<IActionResult> Index()
        {
            return View(await _context.CentrosCostos.ToListAsync());
        }

        // GET: CentroCostoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentrosCostos
                .FirstOrDefaultAsync(m => m.CentroCostoId == id);
            if (centroCosto == null)
            {
                return NotFound();
            }

            return View(centroCosto);
        }

        // GET: CentroCostoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: CentroCostoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CentroCostoId,Nombre,Codigo")] CentroCosto centroCosto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(centroCosto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(centroCosto);
        }

        // GET: CentroCostoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentrosCostos.FindAsync(id);
            if (centroCosto == null)
            {
                return NotFound();
            }
            return View(centroCosto);
        }

        // POST: CentroCostoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CentroCostoId,Nombre,Codigo")] CentroCosto centroCosto)
        {
            if (id != centroCosto.CentroCostoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(centroCosto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CentroCostoExists(centroCosto.CentroCostoId))
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
            return View(centroCosto);
        }

        // GET: CentroCostoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var centroCosto = await _context.CentrosCostos
                .FirstOrDefaultAsync(m => m.CentroCostoId == id);
            if (centroCosto == null)
            {
                return NotFound();
            }

            return View(centroCosto);
        }

        // POST: CentroCostoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var centroCosto = await _context.CentrosCostos.FindAsync(id);
            if (centroCosto != null)
            {
                _context.CentrosCostos.Remove(centroCosto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CentroCostoExists(int id)
        {
            return _context.CentrosCostos.Any(e => e.CentroCostoId == id);
        }
    }
}
