using Banco.Core.Dtos;
using Banco.Core.Interfaces;
using Banco.Core.Repositorio.Entidades;
using Banco.Core.Repositorio.Interfaces;
using Banco.ReglasDeNegocio.Helpers;

namespace Banco.ReglasDeNegocio.Reglas
{
    internal class AhorroReglaDeNegocio(IRepositorio repositorio) : IAhorroReglasDeNegocio
    {
        private readonly IRepositorio _repositorio = repositorio;

        public async Task<IdDto> AgregarAsync(AhorroDtoIn ahorro)
        {
            AhorroEntidad entidad;
            ClienteEntidad clienteEntidad;

            clienteEntidad = await _repositorio.Cliente.ObtenerAsync(ahorro.ClienteId);
            entidad = new AhorroEntidad
            {
                ClienteEncodedkey = clienteEntidad.EncodedKey,
                ClienteId = clienteEntidad.Id,
                Encodedkey = ahorro.Encodedkey,
                EstaActivo = true,
                FechaDeRegistro = DateTime.Now,
                Nombre = "Deposito"                
            };
            entidad.Id = await _repositorio.Ahorro.AgregarAsync(entidad);

            return new IdDto
            {
                Id = entidad.Id,
                Encodedkey = entidad.Encodedkey,
                FechaDeRegistro = DateTime.Now,
                Mensaje = "Ahorro registrado"
            };
        }

        public Task<IdDto> DepositarAsync(MovimientoDtoIn deposito)
        {
            throw new NotImplementedException();
        }

        public async Task<AhorroDto> ObtenerAsync(string encodedkey)
        {
            AhorroEntidad entidad;

            entidad = await _repositorio.Ahorro.ObtenerAsync(encodedkey);

            return entidad.ToDto();
        }

        public Task<IdDto> RetirarAsync(MovimientoDtoIn retiro)
        {
            throw new NotImplementedException();
        }
    }
}
