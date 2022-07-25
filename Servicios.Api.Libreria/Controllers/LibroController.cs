using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Libreria.Core.Entities;
using Servicios.Api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibroController : ControllerBase
    {
        private readonly IMongoRepository<Libro> _libroRepository;

        public LibroController(IMongoRepository<Libro> libroRepository)
        {
            _libroRepository = libroRepository;
        }

        [HttpPost]
        public async Task Post(Libro libro)
        {
            await _libroRepository.InsertDocument(libro);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Libro>>> GetAll()
        {
            return Ok(await _libroRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Libro>> GetById(string id)
        {
            return Ok(await _libroRepository.GetById(id));
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<Libro>>> Pagination(PaginationEntity<Libro> pagination)
        {
            return Ok(await _libroRepository.PaginationByFilter(pagination));
        }


    }
}
