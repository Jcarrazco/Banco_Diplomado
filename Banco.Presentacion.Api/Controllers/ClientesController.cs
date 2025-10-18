using Banco.Core.Dtos;
using Banco.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banco.Presentacion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientesController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpPost]
        public async Task<IActionResult> AgregarAsync(ClienteDtoIn cliente)
        {
           

            return Created();
        }

        /// <summary>
        /// Generación de token
        /// </summary>
        /// <param name="inicioDeSesion"></param>
        /// <returns></returns>
        [HttpPost("InicioDeSesiones")]
        [AllowAnonymous]
        [ProducesResponseType<TokenDto>(201)]
        [Produces("application/json")]
        public async Task<IActionResult> IniciarSesionAsync(InicioDeSesionDto inicioDeSesion)
        {
            return Ok();
        }
    }
}
