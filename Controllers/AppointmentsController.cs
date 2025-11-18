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

            // Se passou pelas validações, adiciona a consulta e salva no banco
            _db.Appointments.Add(a);
            await _db.SaveChangesAsync();

            // Retorna 201 Created com o recurso criado e a rota para consultá‑lo
            return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
        }

        // PUT /Appointments/{id}
        // Atualiza uma consulta existente; o id da URL deve ser o mesmo do objeto enviado
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Appointment a)
        {
            // Garante que o recurso a ser atualizado é o mesmo indicado na URL
            if (id != a.Id) return BadRequest();

            // Marca a entidade como modificada e salva as alterações
            _db.Entry(a).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent(); // 204: atualização feita sem retornar conteúdo
        }

        // DELETE /Appointments/{id}
        // Remove uma consulta do banco, se ela existir
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            // Tenta buscar a consulta pelo Id
            var a = await _db.Appointments.FindAsync(id);
            if (a == null) return NotFound(); // 404 se não encontrar

            // Remove a consulta e salva a alteração no banco
            _db.Appointments.Remove(a);
            await _db.SaveChangesAsync();
            return NoContent(); // 204 indicando que foi excluído com sucesso
        }
    }
}
