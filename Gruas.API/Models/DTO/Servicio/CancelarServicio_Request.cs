namespace Gruas.API.Models.DTO.Servicio
{
    public class CancelarServicio_Request
    {
        public Guid servicioId { get; set; }
        public string motivo { get; set; }
    }
}
