using Banco.Core.Interfaces;


namespace Banco.ReglasDeNegocio.Reglas
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAhorroReglasDeNegocio Ahorro { get; }

        public IClienteReglasDeNEgocio Cliente { get; }

        public IEstadoReglasDeNegocio Estado { get; }

        public UnitOfWork(IAhorroReglasDeNegocio ahorro, IClienteReglasDeNEgocio cliente, IEstadoReglasDeNegocio estado)
        {
            Ahorro = ahorro;
            Cliente = cliente;
            Estado = estado;
        }
    }
}
