using Banco.Core.Dtos;

namespace Banco.Core.Interfaces
{
    public interface IClienteReglasDeNEgocio
    {
        Task<IdDto> AgregarAsync(ClienteDtoIn cliente);
        Task<TokenDto> IniciarSesionAsync(InicioDeSesionDto inicioDeSesion);
        Task<ClienteDto> ObtenerAsync(ClienteDtoIn cliente);
    }
}
