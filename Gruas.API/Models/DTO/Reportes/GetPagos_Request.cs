namespace Gruas.API.Models.DTO.Reportes
{
    public class GetPagos_Request
    {
        public Guid? proveedorId { get; set; }
        public int? estatusPagoId { get; set; }
        public DateTime? fechaInicio { get; set; }
        public DateTime? fechaTermino { get; set; }
    }
}
