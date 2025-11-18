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

        // Busca TODAS as consultas no banco, incluindo os dados relacionados de Dentist e Procedure
        [HttpGet]
        public async Task<IEnumerable<Appointment>> Get() =>
            await _db.Appointments
                .Include(a => a.Dentist)   // faz o join para trazer o dentista
                .Include(a => a.Procedure) // faz o join para trazer o procedimento
                .ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> Get(int id)
        {
            var a = await _db.Appointments
                .Include(x => x.Dentist)    // inclui o dentista na consulta
                .Include(x => x.Procedure)  // inclui o procedimento na consulta
                .FirstOrDefaultAsync(x => x.Id == id); // filtra pelo Id recebido

            return a == null ? NotFound() : a;
        }

        // Antes de salvar, valida se o DentistId e o ProcedureId existem
        [HttpPost]
        public async Task<ActionResult<Appointment>> Post(Appointment a)
        {
            // Valida se existe um dentista com o Id informado
            if (!_db.Dentists.Any(d => d.Id == a.DentistId))
                return BadRequest("DentistId inválido.");

            // Valida se existe um procedimento com o Id informado
            if (!_db.Procedures.Any(p => p.Id == a.ProcedureId))
                return BadRequest("ProcedureId inválido.");

            // Se passou pelas validações, adiciona a consulta e salva no banco
            _db.Appointments.Add(a);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = a.Id }, a);
        }

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
