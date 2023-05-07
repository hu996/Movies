using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Dtos;
using Movies.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Movies.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> _AllowedExtentionPoster = new List<string> { ".jpg", ".png" };
        private long _AllowedSizePoster = 1048576;
        private readonly ApplicationDbContext _context;
        public MoviesController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpPost]
        public IActionResult Create([FromForm] MovieDto Dto)
        {
            if (!_AllowedExtentionPoster.Contains(Path.GetExtension(Dto.Poster.FileName.ToLower())))
            {
                return BadRequest("Only .jpg and .png Extentions are Allowed");
            }
            if(Dto.Poster.Length>_AllowedSizePoster)
            {
                return BadRequest("Only 1 Mb File Size is Allwoed");
            }
            var isvalidgenre = _context.Genres.SingleOrDefault(g => g.ID == Dto.GenreId);
            if(isvalidgenre==null)
            {
                return BadRequest("No selected genre, please select another one");
            }
           using var datastream = new MemoryStream();
            Dto.Poster.CopyTo(datastream);
            var movie = new Movie
            {
                GenreId = Dto.GenreId,
                Title =Dto.Title,
                Year=Dto.Year,
                Rate=Dto.Rate,
                Storeline=Dto.Storeline,
                Poster=datastream.ToArray(),
               };
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return Ok(movie);
        }
        [HttpGet]   
        public IActionResult GetAll()
        {
            var result = _context.Movies.Include(m=>m.Genre).OrderBy(g=>g.Year).ToList();
            return Ok(result);
        }
        [HttpGet("{id}")]
        public IActionResult GetById(int? id)
        {
            var result = _context.Movies.Include(m => m.Genre).SingleOrDefault(g => g.Id==id);
            return Ok(result);
        }
        [HttpDelete]
        public IActionResult Delete(int Id)
        {
            var result = _context.Movies.Include(m => m.Genre).SingleOrDefault(g => g.Id == Id);
            _context.Movies.Remove(result);
            _context.SaveChanges();
            return Ok(result);
        }
    }
}
