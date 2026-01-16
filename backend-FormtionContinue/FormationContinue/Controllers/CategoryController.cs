using FormationContinue.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FormationContinue.Dtos.Category;
using Microsoft.EntityFrameworkCore;
using FormationContinue.Models;


namespace FormationContinue.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    public class CategoryController : ControllerBase
    {

        private readonly AppDbContext _context;
        public CategoryController(AppDbContext context)
        {

            _context = context;
        }
        [HttpGet("categories")]
        public async Task<ActionResult<List<CategoryResponseDto>>> GetAll()
        {
            var categories = await _context.Categories.AsNoTracking().Select(c => new CategoryResponseDto { Id = c.Id, Libelle = c.Libelle }).ToListAsync();
            return Ok(categories);
            
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult<CategoryResponseDto>> GetById(int id)
        {
           
            var category = await _context.Categories.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
            if (category == null )
            {
                return NotFound();
            }
            var CatDto = new CategoryResponseDto
            {
                Id = category.Id,
                Libelle = category.Libelle,
            };
            return Ok(CatDto);
        }

        [HttpPost("categories")]
        public async Task<ActionResult<CategoryResponseDto>> Create(CategoryCreateDto dto)
        {
            var cat = new Category
            {
                Libelle = dto.Libelle,
            };
            var LibExist = (await _context.Categories.CountAsync(c => c.Libelle == cat.Libelle)) > 0;

            if (LibExist) {
                return BadRequest("Category Already exist");
            }
            _context.Categories.Add(cat);
            await _context.SaveChangesAsync();
            var ResponseDto = new CategoryResponseDto
            {
                Id = cat.Id,
                Libelle = cat.Libelle,
            };
            return CreatedAtAction(
                nameof(GetById),
                new { id = cat.Id },
                ResponseDto
             );
        }

        [HttpPut("categories/{id}")]
        public async Task<IActionResult> Update(int id, CategoryUpdateDto dto)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var CatExist = (await _context.Categories.CountAsync(c => c.Libelle == dto.Libelle && c.Id != id)) > 0;
            if (CatExist)
            {
                return BadRequest("Category Already exist");
            }
            category.Libelle = dto.Libelle;
            await _context.SaveChangesAsync();

            return NoContent();

        }



        [HttpDelete("categories/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();
            return NoContent();
            
        }


    } 
}
