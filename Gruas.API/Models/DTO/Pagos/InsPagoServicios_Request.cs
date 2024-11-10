namespace Gruas.API.Models.DTO.Pagos
{
    public class InsPagoServicios_Request
    {
        public Guid proveedorId { get; set; }
        public string concepto { get; set; }
        public List<InsPagoServiciosDet_Request> servicios { get; set; } = new List<InsPagoServiciosDet_Request>();
    }
    public class InsPagoServiciosDet_Request
    {
        public Guid servicioId { get; set; }
        public decimal subTotal { get; set; }
        public decimal comision { get; set; }
        public decimal total { get; set; }
    }
}
