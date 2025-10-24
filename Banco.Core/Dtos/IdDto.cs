namespace Banco.Core.Dtos
{
    public class IdDto
    {
        public IdDto()
        {
            this.FechaDeRegistro = DateTime.Now;
        }

        public int Id { get; set; }

        public string Encodedkey { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaDeRegistro { get; set; } = DateTime.Now;
    }
}
