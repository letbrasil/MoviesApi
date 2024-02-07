using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Data;
using MoviesApi.Data.Dtos;
using MoviesApi.Models;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private MovieContext _context;
        private IMapper _mapper;

        public MovieController(MovieContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        /// <summary>
        /// Recupera todos os filmes existentes no banco de dados
        /// </summary>
        /// <param name="skip"></param>
        /// <param name="take"></param>
        /// <returns>IEnumerable</returns>
        /// <response code="200">Sucesso</response>
        [HttpGet]
        public IEnumerable<ReadMovieDto> GetMovies([FromQuery] int skip = 0, [FromQuery] int take = 50)
        {
            return _mapper.Map<List<ReadMovieDto>>(_context.Movies.Skip(skip).Take(take));
        }

        /// <summary>
        /// Recupera filme com ID correspondente
        /// </summary>
        /// <param name="id"></param>
        /// <returns>IActionResult</returns>
        /// <response code="200">Sucesso</response>
        /// <response code="404">Caso o ID não seja encontrado</response>
        [HttpGet("{id}")]
        public IActionResult GetMovieById(int id)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
            if (movie == null) return NotFound();
            var movieDto = _mapper.Map<ReadMovieDto>(movie);
            return Ok(movieDto);
        }

        /// <summary>
        /// Adiciona um filme ao banco de dados
        /// </summary>
        /// <param name="movieDto">Objeto com os campos necessários para criação de um filme</param>
        /// <returns>IActionResult</returns>
        /// <response code="201">Caso inclusão seja feita com sucesso</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public IActionResult AddMovie([FromBody] CreateMovieDto movieDto)
        {
            Movie movie = _mapper.Map<Movie>(movieDto);
            _context.Movies.Add(movie);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetMovieById), new { id = movie.Id }, movie);
        }

        /// <summary>
        /// Atualiza todas as informações do filme correspondente a um ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="movieDto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public IActionResult UpdateMovie(int id, [FromBody] UpdateMovieDto movieDto)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
            if (movie == null) return NotFound();
            _mapper.Map(movieDto, movie);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Atualiza informação específica do filme correspondente a um ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="patch"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        public IActionResult UpdateMoviePartial(int id, JsonPatchDocument<UpdateMovieDto> patch)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
            if (movie == null) return NotFound();

            var movieToUpdate = _mapper.Map<UpdateMovieDto>(movie);
            patch.ApplyTo(movieToUpdate, ModelState);
            if (!TryValidateModel(movieToUpdate))
            {
                return ValidationProblem(ModelState);
            }
            _mapper.Map(movieToUpdate, movie);
            _context.SaveChanges();
            return NoContent();
        }

        /// <summary>
        /// Exclui do banco de dados filme com o ID correspondente
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public IActionResult DeleteMovie(int id)
        {
            var movie = _context.Movies.FirstOrDefault(movie => movie.Id == id);
            if (movie == null) return NotFound();
            _context.Remove(movie);
            _context.SaveChanges();
            return NoContent();
        }
    }
}
