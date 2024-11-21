using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Presta.net_app.Data;
using Presta.net_app.Models;


namespace Presta.net_app.Controllers
{
    public class PrestatariosController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PrestatariosController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET: Prestatarios
        public async Task<IActionResult> Index(string filtro)
        {
            var prestatarios = _context.Prestatarios.AsQueryable();

            if (!string.IsNullOrEmpty(filtro))
            {
                if (int.TryParse(filtro, out int dni))
                {
                    prestatarios = prestatarios.Where(p => p.DNI == dni);
                }
                else
                {
                    prestatarios = prestatarios.Where(p =>
                        EF.Functions.Like(p.Nombre, $"%{filtro}%") ||
                        EF.Functions.Like(p.Apellido, $"%{filtro}%"));
                }
            }

            return View(await prestatarios.ToListAsync());
        }

            // GET: Prestatarios/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestatario = await _context.Prestatarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestatario == null)
            {
                return NotFound();
            }

            return View(prestatario);
        }

        // GET: Prestatarios/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Prestatarios/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Email,Telefono,FechaNacimiento,DNI,FotoFrenteDni,FotoDorsoDni,Provincia,Localidad,Direccion")] Prestatario prestatario)
        {
            if (ModelState.IsValid)
            {
                var archivos = HttpContext.Request.Form.Files;
                if (archivos != null && archivos.Count > 0)
                {
                    var archivoFotoFrente = archivos[0];
                    var pathDestinoFrente = Path.Combine(_env.WebRootPath, "images\\fotos\\frente");
                    if (archivoFotoFrente.Length > 0)
                    {
                        var archivoDestinoFrente = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFotoFrente.FileName);

                        using (var filestreamFrente = new FileStream(Path.Combine(pathDestinoFrente, archivoDestinoFrente), FileMode.Create))
                        {
                            archivoFotoFrente.CopyTo(filestreamFrente);
                            prestatario.FotoFrenteDni = archivoDestinoFrente;
                        }
                    }
                    var archivoFotoDorso = archivos[1];
                    var pathDestinoDorso = Path.Combine(_env.WebRootPath, "images\\fotos\\dorso");
                    if (archivoFotoDorso.Length > 0)
                    {
                        var archivoDestinoDorso = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFotoDorso.FileName);

                        using (var filestreamDorso = new FileStream(Path.Combine(pathDestinoDorso, archivoDestinoDorso), FileMode.Create))
                        {
                            archivoFotoDorso.CopyTo(filestreamDorso);
                            prestatario.FotoDorsoDni = archivoDestinoDorso;
                        }
                    }

                }

                await _context.Prestatarios.AddAsync(prestatario);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));

            }
            return View(prestatario);
        }

        // GET: Prestatarios/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["Error"] = "";
            if (id == null)
            {
                return NotFound();
            }

            var prestatario = await _context.Prestatarios.FindAsync(id);
            if (prestatario == null)
            {
                ViewData["Error"] = "Error existe el codigo del prestatario " + id;
                return NotFound();
            }
            return View(prestatario);
        }

        // POST: Prestatarios/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Email,Telefono,FechaNacimiento,DNI,FotoFrenteDni,FotoDorsoDni,Provincia,Localidad,Direccion")] Prestatario prestatario)
        {
            if (id != prestatario.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var archivos = HttpContext.Request.Form.Files;
                    if (archivos != null && archivos.Count > 0)
                    {
                        var archivoFotoFrente = archivos[0];
                        if (archivoFotoFrente.Length > 0)
                        {
                            var pathDestinoFrente = Path.Combine(_env.WebRootPath, "images\\fotos\\frente");
                            var archivoDestinoFrente = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFotoFrente.FileName);

                            using (var filestream = new FileStream(Path.Combine(pathDestinoFrente, archivoDestinoFrente), FileMode.Create))
                            {
                                archivoFotoFrente.CopyTo(filestream);

                                if (!string.IsNullOrEmpty(prestatario.FotoFrenteDni))
                                {
                                    string fotoAnteriorFrente = Path.Combine(pathDestinoFrente, prestatario.FotoFrenteDni);
                                    if (System.IO.File.Exists(fotoAnteriorFrente))
                                        System.IO.File.Delete(fotoAnteriorFrente);
                                }

                                prestatario.FotoFrenteDni = archivoDestinoFrente;
                            }
                        }
                        var archivoFotoDorso = archivos[1];
                        if (archivoFotoDorso.Length > 0)
                        {
                            var pathDestinoDorso = Path.Combine(_env.WebRootPath, "images\\fotos\\dorso");
                            var archivoDestinoDorso = Guid.NewGuid().ToString().Replace("-", "") + Path.GetExtension(archivoFotoDorso.FileName);

                            using (var filestream = new FileStream(Path.Combine(pathDestinoDorso, archivoDestinoDorso), FileMode.Create))
                            {
                                archivoFotoDorso.CopyTo(filestream);

                                if (!string.IsNullOrEmpty(prestatario.FotoFrenteDni))
                                {
                                    string fotoAnteriorDorso = Path.Combine(pathDestinoDorso, prestatario.FotoFrenteDni);
                                    if (System.IO.File.Exists(fotoAnteriorDorso))
                                        System.IO.File.Delete(fotoAnteriorDorso);
                                }

                                prestatario.FotoFrenteDni = archivoDestinoDorso;
                            }
                        }
                    }

                    _context.Update(prestatario);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PrestatarioExists(prestatario.Id))
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
            return View(prestatario);
        }

        // GET: Prestatarios/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prestatario = await _context.Prestatarios
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prestatario == null)
            {
                return NotFound();
            }

            return View(prestatario);
        }

        // POST: Prestatarios/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prestatario = await _context.Prestatarios.FindAsync(id);
            if (prestatario != null)
            {
                _context.Prestatarios.Remove(prestatario);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PrestatarioExists(int id)
        {
            return _context.Prestatarios.Any(e => e.Id == id);
        }
    }
}
