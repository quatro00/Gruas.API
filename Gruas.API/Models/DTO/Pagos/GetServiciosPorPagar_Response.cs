namespace Gruas.API.Models.DTO.Pagos
{
    public class GetServiciosPorPagar_Response
    {
        public Guid id { get; set; }
        public int folio { get; set; }
        public string cliente { get; set; }
        public string telefono { get; set; }
        public DateTime fecha { get; set; }
        public string grua { get; set; }
        public string tipo { get; set; }
        public string estatus { get; set; }
        public decimal subTotal { get; set; }
        public decimal comision { get; set; }
        public decimal total { get; set; }
    }
}
