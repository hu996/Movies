using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Movies.Dtos;
using Movies.Models;
using System.Linq;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public GenresController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var result = _context.Genres.ToList();
            return Ok(result);
        }
        [HttpPost]
        public IActionResult Create(GenreDto Dto)
        {

            var genre=new Genre();
            genre.Name= Dto.Name;
            _context.Genres.Add(genre);
            _context.SaveChanges();
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public  IActionResult Update(byte id,GenreDto Dto)
        {
            var result=_context.Genres.SingleOrDefault(g=>g.ID==id);
            result.Name=Dto.Name;
            _context.Genres.Update(result);
            _context.SaveChanges();
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(byte id)
        {
            var result=_context.Genres.SingleOrDefault(g=>g.ID==id);
            _context.Genres.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }
    }
}
