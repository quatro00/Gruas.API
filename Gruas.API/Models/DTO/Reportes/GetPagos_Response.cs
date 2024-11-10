namespace Gruas.API.Models.DTO.Reportes
{
    public class GetPagos_Response
    {
        public Guid id { get; set; }
        public int folio { get; set; }
        public string proveedor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime? fechaPago { get; set; }
        public string referencia { get; set; }
        public string concepto { get; set; }
        public string estatusPago { get; set; }
        public int cantidadServicios { get; set; }
        public decimal subTotal { get; set; }
        public decimal comision { get; set; }
        public decimal total { get; set; }
    }
}
