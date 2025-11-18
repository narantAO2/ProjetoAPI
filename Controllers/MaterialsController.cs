using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OdontoApi.Data;
using OdontoApi.Models;

namespace OdontoApi.Controllers
{
    [ApiController]
    [Route("Materials")]
    public class MaterialsController : ControllerBase
    {
        private readonly AppDbContext _db;
        public MaterialsController(AppDbContext db) => _db = db;

        [HttpGet]
        public async Task<IEnumerable<Material>> Get() =>
            await _db.Materials.ToListAsync();


        [HttpPost]
        public async Task<ActionResult<Material>> Post(Material m)
        {
            _db.Materials.Add(m);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = m.Id }, m);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, Material m)
        {
            if (id != m.Id) return BadRequest();
            _db.Entry(m).State = EntityState.Modified;
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var m = await _db.Materials.FindAsync(id);
            if (m == null) return NotFound();
            _db.Materials.Remove(m);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
