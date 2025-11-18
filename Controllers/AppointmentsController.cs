using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoApi.Data;
using OdontoApi.Models;

namespace OdontoApi.Controllers
{
    [ApiController]
    [Route("Appointments")]
    public class AppointmentsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;
        
        [HttpGet]
        public async Task<IEnumerable<Appointment>> Get() =>
            await _db.Appointments
                .Include(a => a.Dentist)
                .Include(a => a.Procedure)   // faz o join para trazer os parametros 
                 .Include(a => a.Material)
                .ToListAsync();
                
        [HttpPost]
        public async Task<ActionResult<Appointment>> Post(Appointment a)
        {
            if (!_db.Dentists.Any(d => d.Id == a.DentistId))
                return BadRequest("DentistId inválido.");
                
            if (!_db.Procedures.Any(p => p.Id == a.ProcedureId))
                return BadRequest("ProcedureId inválido.");          // Valida se existe algo com o Id informado

            if (!_db.Materials.Any(m => m.Id == a.MaterialId))
                return BadRequest("MaterialId inválido.");
                
            _db.Appointments.Add(a);
            await _db.SaveChangesAsync();
            
            return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Appointment a)
        {
            if (id != a.Id) return BadRequest();

            _db.Entry(a).State = EntityState.Modified;   // Modo de otimização
            await _db.SaveChangesAsync();
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
        
            var a = await _db.Appointments.FindAsync(id);
            if (a == null) return NotFound();
            
            _db.Appointments.Remove(a); 
            await _db.SaveChangesAsync(); 
        }
    }
}



