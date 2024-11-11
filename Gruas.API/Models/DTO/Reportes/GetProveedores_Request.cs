namespace Gruas.API.Models.DTO.Reportes
{
    public class GetProveedores_Request
    {
        public Guid? estadoId { get; set; }
        public string? descripcion { get; set; } = "";
    }
}
