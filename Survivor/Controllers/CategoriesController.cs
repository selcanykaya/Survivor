using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Survivor.Data;
using Survivor.Models;


namespace Survivor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly SurvivorDbContext _context;

        public CategoriesController(SurvivorDbContext context)
        {
            _context = context;
        }

        // GET /api/categories
        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _context.Categories
                .Where(c => !c.IsDeleted)
                .Include(c => c.Competitors)
                .ToList();
          return Ok(categories);
        }

        // GET /api/categories/{id}

        [HttpGet("{id:int}")]
        public IActionResult GetCategoryById(int id)
        {
            var c = _context.Categories
                .Include(c => c.Competitors)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);

            if (c == null)
                return NotFound($"Category with ID {id} not found.");

            return Ok(c);
        }

        // POST /api/categories
        [HttpPost]
        public IActionResult CreateCategory([FromBody] CategoryEntity category)
        {

            category.CreatedDate = DateTime.Now;
            category.ModifiedDate = DateTime.Now;
            category.IsDeleted = false;
           

            _context.Categories.Add(category);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetCategoryById), new { id = category.Id }, category);
        }

        // PUT /api/categories/{id}

        [HttpPut("{id:int}")]
        public IActionResult UpdateCategory(int id, [FromBody] CategoryEntity updated)
        {

            var category = _context.Categories.Find(id);

            if (category == null || category.IsDeleted)
                return NotFound($"Category with ID {id} not found.");

            category.Name = updated.Name;
            category.ModifiedDate = DateTime.Now;

           _context.SaveChanges();

            return Ok(category);
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var category = _context.Categories.Find(id);

            if (category == null || category.IsDeleted)
                return NotFound($"Category with ID {id} not found.");

            category.IsDeleted = true;
            category.ModifiedDate = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }
    }
}
