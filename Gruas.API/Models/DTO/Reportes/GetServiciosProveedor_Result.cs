namespace Gruas.API.Models.DTO.Reportes
{
    public class GetServiciosProveedor_Result
    {
        public Guid id { get; set; }
        public int folio { get; set; }
        public DateTime fecha { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public decimal distancia { get; set; }
        public decimal cuotaKm { get; set; }
        public decimal banderazo { get; set; }
        public decimal total {  get; set; }
        public decimal comision {  get; set; }
        public string placas {  get; set; }
        public string estatus { get; set; }
    }
}
