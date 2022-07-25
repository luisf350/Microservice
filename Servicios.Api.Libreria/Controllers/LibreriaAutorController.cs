using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Libreria.Core.Entities;
using Servicios.Api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaAutorController : ControllerBase
    {
        private readonly IMongoRepository<Autor> _autorRepository;

        public LibreriaAutorController(IMongoRepository<Autor> autorRepository)
        {
            _autorRepository = autorRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAll()
        {
            return Ok(await _autorRepository.GetAll());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Autor>> GetById(string id)
        {
            return Ok(await _autorRepository.GetById(id));
        }

        [HttpPost]
        public async Task Post(Autor autor)
        {
            await _autorRepository.InsertDocument(autor);
        }


        [HttpPut("{id}")]
        public async Task Put(string id, Autor autor)
        {
            autor.Id = id;
            await _autorRepository.UpdateDocument(autor);
        }

        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _autorRepository.DeleteById(id);
        }

        [HttpPost("pagination")]
        public async Task<ActionResult<PaginationEntity<Autor>>> Pagination(PaginationEntity<Autor> pagination)
        {
            return await _autorRepository.PaginationByFilter(pagination);
        }
    }
}
