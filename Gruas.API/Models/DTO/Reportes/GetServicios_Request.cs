namespace Gruas.API.Models.DTO.Reportes
{
    public class GetServicios_Request
    {
        public Guid? proveedorId { get; set; }
        public int? estatusServicioId { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaTermino { get; set; }
    }
}
