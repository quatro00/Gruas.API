namespace Gruas.API.Models.DTO.Proveedor
{
    public class GetPagosProveedor_Result
    {
        public Guid id { get; set; }
        public int folio {  get; set; }
        public string razonSocial { get; set; }
        public string concepto { get; set; }
        public decimal monto { get; set; }
        public string referencia { get; set; }
        public DateTime? fechaPago { get; set; }
        public DateTime fechaRegistro { get; set; }
        public int estatusPagoId { get; set; }
        public string estatusPago {  get; set; }
        public decimal subTotal { get; set; }
        public decimal comision { get; set; }
        public decimal total { get; set; }
    }
}
