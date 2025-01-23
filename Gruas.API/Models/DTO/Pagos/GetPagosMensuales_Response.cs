namespace Gruas.API.Models.DTO.Pagos
{
    public class GetPagosMensuales_Response
    {
        public Guid id { get; set; }
        public int folio { get; set; }
        public decimal monto { get; set; }
        public string concepto { get; set; }
        public int estatusPagoId { get; set; }
        public string estatusPago {  get; set; }
        public string referencia { get; set; }
        public DateTime? fechaPago { get; set; }
        public DateTime fechaCreacion { get; set; }
    }
}
