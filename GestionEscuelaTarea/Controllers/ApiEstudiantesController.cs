using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionEscuelaTarea.Data;
using GestionEscuelaTarea.Models;

namespace GestionEscuelaTarea.Controllers
{
    // ESTO ES LO QUE ANGULAR NECESITA (RETORNA JSON, NO VISTAS)
    [Route("api/[controller]")]
    [ApiController]
    public class ApiEstudiantesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApiEstudiantesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ApiEstudiantes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes()
        {
            return await _context.Estudiantes.ToListAsync();
        }

        // GET: api/ApiEstudiantes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();
            return estudiante;
        }

        // POST: api/ApiEstudiantes
        [HttpPost]
        public async Task<ActionResult<Estudiante>> PostEstudiante(Estudiante estudiante)
        {
            _context.Estudiantes.Add(estudiante);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetEstudiante", new { id = estudiante.Id }, estudiante);
        }

        // PUT: api/ApiEstudiantes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEstudiante(int id, Estudiante estudiante)
        {
            if (id != estudiante.Id) return BadRequest();
            _context.Entry(estudiante).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/ApiEstudiantes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEstudiante(int id)
        {
            var estudiante = await _context.Estudiantes.FindAsync(id);
            if (estudiante == null) return NotFound();
            _context.Estudiantes.Remove(estudiante);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}