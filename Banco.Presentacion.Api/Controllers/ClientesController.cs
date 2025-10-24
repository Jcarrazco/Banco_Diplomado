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
            ClienteDto clienteDto = await _unitOfWork.Cliente.ObtenerAsync(cliente);
            IdDto idDto;
            if (clienteDto is not null)
            {
                idDto = new IdDto
                {
                    Id = clienteDto.Id,
                    Encodedkey = clienteDto.EncodedKey,
                    Mensaje = "Registro previo"
                };

                return StatusCode(208, idDto);
            }

            idDto = await _unitOfWork.Cliente.AgregarAsync(cliente);

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
            TokenDto tokenDto = await _unitOfWork.Cliente.IniciarSesionAsync(inicioDeSesion);

            if (tokenDto == null)
            {
                return StatusCode(400, new IdDto { Mensaje = "Credenciales no validas"}); 
            }

            return Ok(tokenDto);
        }
    }
}
