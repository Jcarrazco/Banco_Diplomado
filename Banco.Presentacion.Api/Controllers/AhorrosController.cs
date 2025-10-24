using Banco.Core.Dtos;
using Banco.Core.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Banco.Presentacion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Cliente")]
    public class AhorrosController(IUnitOfWork unitOfWork) : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        [HttpPost]
        public async Task<IActionResult> AgregarAsync(AhorroDtoIn ahorro)
        {
            AhorroDto ahorroDto;

            ahorroDto = await _unitOfWork.Ahorro.ObtenerAsync(ahorro.Encodedkey);
            if (ahorroDto is not null)
            {
                return StatusCode(208, new IdDto
                {
                    Encodedkey = ahorroDto.Encodedkey,
                    FechaDeRegistro = ahorroDto.FechaDeRegistro,
                    Id = ahorroDto.Id,
                    Mensaje = "Ahorro registrado previamente"
                });
            }

            IdDto idDto;

            idDto = await _unitOfWork.Ahorro.AgregarAsync(ahorro);

            return Created(string.Empty, idDto);
        }

        [HttpPost("/Depositos")]  
        public async Task<IActionResult> DespositarAsync(MovimientoDtoIn deposito)
        {
            IdDto idDto;
            idDto = await _unitOfWork.Ahorro.DepositarAsync(deposito);

            return Created(string.Empty, idDto);
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerAhorroAsync()
        {
            string id;
            id = User.Claims.FirstOrDefault(c => c.Type == "ClienteId")?.Value;

            AhorroDto ahorroDto = await _unitOfWork.Ahorro.ObtenerAhorroPorClienteIdAsync(id);

            return Ok(ahorroDto);
        }
    }
}
