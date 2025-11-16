using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoApi.Data;
using OdontoApi.Models;

namespace OdontoApi.Controllers
{
    [ApiController]
    [Route("Dentists")]
    public class DentistsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public DentistsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Dentist>> Get() =>
            await _db.Dentists.ToListAsync();

        [HttpGet("{id}")]
        public async Task<ActionResult<Dentist>> Get(int id)
        {
            var d = await _db.Dentists.FindAsync(id);
            return d == null ? NotFound() : d;
        }

        [HttpPost]
        public async Task<ActionResult<Dentist>> Post(Dentist d)
        {
            _db.Dentists.Add(d);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = d.Id }, d);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Dentist d)
        {
            if (id != d.Id) return BadRequest();
            _db.Entry(d).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var d = await _db.Dentists.FindAsync(id);
            if (d == null) return NotFound();
            _db.Dentists.Remove(d);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
