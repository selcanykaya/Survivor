using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Models;


namespace Survivor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompetitorsController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CompetitorsController(SurvivorDbContext context)
        {
            _context = context;
        }

        // GET /api/competitors
        [HttpGet]
        public IActionResult GetCompetitors()
        {
            var competitors = _context.Competitors
                .Where(c => !c.IsDeleted)
                .Include(c => c.Category)
                .ToList();

            return Ok(competitors);
        }

        // GET /api/competitors/{id}
        [HttpGet("{id:int}")]
        public IActionResult GetCompetitorById(int id)
        {
            var c = _context.Competitors
                .Include(c => c.Category)
                .FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted);

            if (c == null)
                return NotFound($"Competitor with ID {id} not found.");

           return Ok(c);

        }

        // GET /api/competitors/categories/{CategoryId}

        [HttpGet("category/{categoryId:int}")]
        public IActionResult GetCompetitorsByCategoryId(int categoryId)
        {
            var competitors = _context.Competitors
                .Where(c => c.CategoryId == categoryId && !c.IsDeleted)
                .Include(c => c.Category)
                .ToList();

            return Ok(competitors);
        }

        // POST /api/competitors

        [HttpPost]
        public IActionResult CreateCompetitor([FromBody] CompetitorEntity competitor)
        {
           
            var newcompetitor = new CompetitorEntity
            {
                FirstName = competitor.FirstName,
                LastName = competitor.LastName,
                CategoryId = competitor.CategoryId,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                IsDeleted = false
            };

            _context.Competitors.Add(competitor);
            _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompetitorById), new { id = competitor.Id }, competitor);
        }


        // PUT /api/competitors/{id}

        [HttpPut("{id:int}")]
        public IActionResult UpdateCompetitor(int id, [FromBody] CompetitorEntity competitor)
        {
            var existing = _context.Competitors.Find(id);

            if (existing == null || existing.IsDeleted)
                return NotFound($"Competitor with ID {id} not found.");

            existing.FirstName = competitor.FirstName;
            existing.LastName = competitor.LastName;
            existing.CategoryId = competitor.CategoryId;
            existing.ModifiedDate = DateTime.UtcNow;

           _context.SaveChanges();

       

            return Ok(competitor);
        }

        // DELETE /api/competitors/{id}

        [HttpDelete("{id:int}")]
        public IActionResult DeleteCompetitor(int id)
        {
            var competitor = _context.Competitors.Find(id);

            if (competitor == null || competitor.IsDeleted)
                return NotFound($"Competitor with ID {id} not found.");

            competitor.IsDeleted = true;
            competitor.ModifiedDate = DateTime.UtcNow;

         _context.SaveChanges();

            return NoContent();
        }
    }
}
