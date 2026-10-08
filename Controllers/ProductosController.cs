using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_WEB.Data;
using Proyecto_WEB.Models;

namespace Proyecto_WEB.Controllers
{
    [Authorize]
    public class ProductosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProductosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.OrderBy(p => p.Nombre).ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nombre,Categoria,Precio,Stock,Descripcion")] Producto producto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();

                TempData["Mensaje"] = "Producto creado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Categoria,Precio,Stock,Descripcion")] Producto producto)
        {
            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    // Si otro usuario eliminó el producto mientras lo editábamos.
                    if (!await _context.Productos.AnyAsync(p => p.Id == producto.Id)) return NotFound();
                    throw;
                }

                TempData["Mensaje"] = "Producto actualizado correctamente.";
                return RedirectToAction(nameof(Index));
            }

            return View(producto);
        }

        // DELETE
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var producto = await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
            if (producto == null) return NotFound();

            return View(producto);
        }

        // DELETE
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = "Producto eliminado correctamente.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
