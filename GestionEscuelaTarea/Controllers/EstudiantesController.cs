using GestionEscuelaTarea.Data;
using GestionEscuelaTarea.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// --- SOLUCIÓN DE ERRORES DE ITEXT ---
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using iText.Kernel.Geom;

// Alias para evitar confusión con EntityFramework
using PdfDocument = iText.Kernel.Pdf.PdfDocument;
using PdfTable = iText.Layout.Element.Table;
using PdfParagraph = iText.Layout.Element.Paragraph;

namespace GestionEscuelaTarea.Controllers
{
    [Authorize(Roles = "Administrador")]
    public class EstudiantesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Estudiantes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Estudiantes.ToListAsync());
        }

        // GET: Estudiantes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // GET: Estudiantes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Estudiantes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nombre,Apellido,Cedula,Email")] Estudiante estudiante)
        {
            if (ModelState.IsValid)
            {
                _context.Add(estudiante);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // POST: Estudiantes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nombre,Apellido,Cedula,Email")] Estudiante estudiante)
        {
            if (id != estudiante.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(estudiante);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EstudianteExists(estudiante.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(estudiante);
        }

        // GET: Estudiantes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var estudiante = await _context.Estudiantes.FirstOrDefaultAsync(m => m.Id == id);
            if (estudiante == null) return NotFound();
            return View(estudiante);
        }

        // POST: Estudiantes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante != null) _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EstudianteExists(int id)
        {
            return _context.Estudiantes.Any(e => e.Id == id);
        }

        // --- MÉTODO CORREGIDO PARA PDF ---
        [HttpGet] // Ruta accesible como /Estudiantes/DescargarPDF
        public async Task<IActionResult> DescargarPDF()
        {
            var estudiantes = await _context.Estudiantes.ToListAsync();

            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);

                // Usamos PageSize.A4 explícitamente
                Document document = new Document(pdf, PageSize.A4);

                // Usamos PdfParagraph (el alias que creamos arriba)
                document.Add(new PdfParagraph("Reporte de Estudiantes - UNIANDES")
                    .SetTextAlignment(TextAlignment.CENTER)
                    .SetFontSize(20));

                document.Add(new PdfParagraph("\n"));

                // Usamos PdfTable (el alias) para que no choque con la base de datos
                PdfTable table = new PdfTable(4, true);
                table.AddHeaderCell("Cédula");
                table.AddHeaderCell("Nombre");
                table.AddHeaderCell("Apellido");
                table.AddHeaderCell("Email");

                foreach (var item in estudiantes)
                {
                    table.AddCell(item.Cedula ?? "S/N");
                    table.AddCell(item.Nombre ?? "S/N");
                    table.AddCell(item.Apellido ?? "S/N");
                    table.AddCell(item.Email ?? "S/N");
                }

                document.Add(table);
                document.Close();

                return File(stream.ToArray(), "application/pdf", "ListaEstudiantes.pdf");
            }
        }
    }
}