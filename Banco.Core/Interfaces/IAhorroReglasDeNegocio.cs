using Banco.Core.Dtos;

namespace Banco.Core.Interfaces
{
    public interface IAhorroReglasDeNegocio
    {
        Task<IdDto> AgregarAsync(AhorroDtoIn ahorro);

        Task<IdDto> DepositarAsync(MovimientoDtoIn deposito);
        Task<AhorroDto> ObtenerAhorroPorClienteIdAsync(string id);
        Task<AhorroDto> ObtenerAsync(string encodedkey);
        Task<IdDto> RetirarAsync(MovimientoDtoIn retiro);
    }
}
