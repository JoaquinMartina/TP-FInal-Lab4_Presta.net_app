using System;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presta.net_app.Data;
using Presta.net_app.Models;

namespace Presta.net_app.Controllers
{
    public class PrestamosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrestamosController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Prestamos
        public async Task<IActionResult> Index(string filtro, int? pageNumber, int pageSize = 5)
        {
            var prestamos = _context.Prestamos
                .Include(p => p.Estado)
                .Include(p => p.PrestamoDetalles)
                .Include(p => p.Prestatario).AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                if (int.TryParse(filtro, out int dni))
                {
                    prestamos = prestamos.Where(p => p.Prestatario.DNI == dni);
                }
                else
                {
                    prestamos = prestamos.Where(p =>
                        EF.Functions.Like(p.Prestatario.Nombre, $"%{filtro}%") ||
                        EF.Functions.Like(p.Prestatario.Apellido, $"%{filtro}%"));
                }
            }

            int page = pageNumber ?? 1;
            int totalItems = await prestamos.CountAsync();
            var paginatedPrestamos = await prestamos
            .OrderBy(p => p.FechaInicio)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
            .ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            ViewBag.CurrentPage = page;

            return View(paginatedPrestamos);
        }

        // GET: Prestamos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
            .Include(p => p.Prestatario)
            .Include(p => p.Estado)
            .Include(p => p.PrestamoDetalles)
            .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // GET: Prestamos/Create
        public IActionResult Create()
        {
            ViewData["Estados"] = new SelectList(_context.Estados, "Id", "Descripcion");
            ViewData["Prestatarios"] = new SelectList(_context.Prestatarios, "Id", "Apellido");
            return View();
        }

        // POST: Prestamos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MontoCapital,CantidadCuotas,FechaInicio,EstadoId,PrestatarioId,InteresPorcentaje")] Prestamo prestamo)
        {
            if (ModelState.IsValid)
            {
                decimal montoPorCuota = (prestamo.MontoCapital + (prestamo.MontoCapital * prestamo.InteresPorcentaje / 100)) / prestamo.CantidadCuotas;
                for (int i = 1; i <= prestamo.CantidadCuotas; i++)
                {
                    prestamo.PrestamoDetalles.Add(new PrestamoDetalle
                    {
                        NroCuota = i,
                        MontoCuota = montoPorCuota,
                        FechaPago = prestamo.FechaInicio.AddMonths(i)
                    });
                }
                _context.Prestamos.Add(prestamo);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["Estados"] = new SelectList(_context.Estados, "Id", "Descripcion", prestamo.EstadoId);
            ViewData["Prestatarios"] = new SelectList(_context.Prestatarios, "Id", "Nombre",  prestamo.PrestatarioId);
            return View(prestamo);
        }

        // GET: Prestamos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo == null)
            {
                return NotFound();
            }
            return View(prestamo);
        }

        // POST: Prestamos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MontoCapital,CantidadCuotas,FechaInicio,EstadoId,PrestatarioId")] Prestamo prestamo)
        {
            if (id != prestamo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(prestamo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestamoExists(prestamo.Id))
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
            return View(prestamo);
        }

        // GET: Prestamos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestamo = await _context.Prestamos
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestamo == null)
            {
                return NotFound();
            }

            return View(prestamo);
        }

        // POST: Prestamos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestamo = await _context.Prestamos.FindAsync(id);
            if (prestamo != null)
            {
                _context.Prestamos.Remove(prestamo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestamoExists(int id)
        {
            return _context.Prestamos.Any(e => e.Id == id);
        }
    }
}
