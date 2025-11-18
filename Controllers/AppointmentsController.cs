using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoApi.Data;
using OdontoApi.Models;

namespace OdontoApi.Controllers
{
    // Controller responsável pelos endpoints da API para trabalhar com Appointment (consultas)
    // A rota base será: http://localhost:5083/Appointments
    [ApiController]
    [Route("Appointments")]
    public class AppointmentsController : ControllerBase
    {
        // Injeção de dependência do AppDbContext para acessar o banco de dados
        private readonly AppDbContext _db;
        public AppointmentsController(AppDbContext db) => _db = db;

        // GET /Appointments
        // Busca TODAS as consultas no banco, incluindo os dados relacionados de Dentist e Procedure
        [HttpGet]
        public async Task<IEnumerable<Appointment>> Get() =>
            await _db.Appointments
                .Include(a => a.Dentist)   // faz o join para trazer o dentista
                .Include(a => a.Procedure) // faz o join para trazer o procedimento
                 .Include(a => a.Material)
                .ToListAsync();

        // GET /Appointments/{id}
        // Busca UMA consulta específica pelo Id; retorna 404 se não encontrar
        [HttpGet("{id}")]
        public async Task<ActionResult<Appointment>> Get(int id)
        {
            var a = await _db.Appointments
                .Include(x => x.Dentist)    // inclui o dentista na consulta
                .Include(x => x.Procedure)  // inclui o procedimento na consulta
                .Include(x => x.Material)
                .FirstOrDefaultAsync(x => x.Id == id); // filtra pelo Id recebido

            return a == null ? NotFound() : a;
        }

        // POST /Appointments
        // Cria uma nova consulta (Appointment) no banco de dados
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
