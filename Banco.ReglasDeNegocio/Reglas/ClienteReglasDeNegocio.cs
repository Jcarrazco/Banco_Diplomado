using Banco.Core.Dtos;
using Banco.Core.Interfaces;
using Banco.Core.Repositorio.Entidades;
using Banco.Core.Repositorio.Interfaces;
using Banco.ReglasDeNegocio.Helpers;

namespace Banco.ReglasDeNegocio.Reglas
{
    internal class ClienteReglasDeNegocio(IRepositorio repositorio) : IClienteReglasDeNEgocio
    {
        private readonly IRepositorio _repositorio = repositorio;   

        public async Task<IdDto> AgregarAsync(ClienteDtoIn cliente)
        {            
            return new IdDto();
        }

        private async Task agregarAhorro(ClienteEntidad entidad)
        {
            AhorroEntidad ahorroEntidad;

            ahorroEntidad = new AhorroEntidad
            {
                Nombre = "Ahorro",
                ClienteEncodedkey = entidad.EncodedKey,
                ClienteId = entidad.Id
            };

            await _repositorio.Ahorro.AgregarAsync(ahorroEntidad);
        }

        private async Task<string> ObtenerCurpAsync(ClienteDtoIn cliente)
        {
            SolicitudDeCurpDtoIn solicitudDeCurpDtoIn;
            string curp;

            curp = string.Empty;

            return curp;
        }        

        public async Task<TokenDto> IniciarSesionAsync(InicioDeSesionDto inicioDeSesion)
        {
            DateTime fechaDeExpiracion = DateTime.Now.AddMinutes(20);

            return null;
        }

        public async Task<ClienteDto> ObtenerAsync(ClienteDtoIn cliente)
        {
            ClienteDto clienteDto;
            ClienteEntidad clienteEntidad;

            clienteEntidad = await _repositorio.Cliente.ObtenerAsync(cliente.EncodedKey);
            if(clienteEntidad is null)
            {
                string curp;

                curp = await ObtenerCurpAsync(cliente);
                clienteEntidad = await _repositorio.Cliente.ObtenerPorCurpAsync(curp);
                if(clienteEntidad is null)
                {
                    return null;
                }
            }
            clienteDto = clienteEntidad.ToDto();

            return clienteDto;
        }        
    }
}
