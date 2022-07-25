using Microsoft.AspNetCore.Mvc;
using Servicios.Api.Libreria.Core.Entities;
using Servicios.Api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.Api.Libreria.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibreriaServicioController : ControllerBase
    {
        private readonly IMongoRepository<Autor> _autorRepository;
        private readonly IMongoRepository<Empleado> _empleadoRepository;


        public LibreriaServicioController(IMongoRepository<Autor> autorRepository, IMongoRepository<Empleado> empleadoRepository)
        {
            _autorRepository = autorRepository;
            _empleadoRepository = empleadoRepository;
        }

        [HttpGet("autores")]
        public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
        {
            var autores = await _autorRepository.GetAll();

            return Ok(autores);
        }

        [HttpGet("empleados")]
        public async Task<ActionResult<IEnumerable<Empleado>>> GetEmpleados()
        {
            var empleados = await _empleadoRepository.GetAll();

            return Ok(empleados);
        }
    }
}
