using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoApi.Data;
using OdontoApi.Models;

namespace OdontoApi.Controllers
{
    [ApiController]
    [Route("Procedures")]
    public class ProceduresController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProceduresController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Procedure>> Get() =>
            await _db.Procedures.ToListAsync();

        [HttpPost]
        public async Task<ActionResult<Procedure>> Post(Procedure p)
        {
            _db.Procedures.Add(p);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = p.Id }, p);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Procedure p)
        {
            if (id != p.Id) return BadRequest();
            _db.Entry(p).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var p = await _db.Procedures.FindAsync(id);
            if (p == null) return NotFound();
            _db.Procedures.Remove(p);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}

