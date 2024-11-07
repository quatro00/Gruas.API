namespace Gruas.API.Models.DTO.Servicio
{
    public class GetServicios_Response
    {
        public Guid id { get; set; }
        public string folio { get; set; }
        public string cliente { get; set; }
        public string telefono { get; set; }
        public string estado { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string costo { get; set; }
        public string estatus { get; set; }
        public string proveedor { get; set; }
        public DateTime fechaCreacion { get; set; }
        public DateTime fecha { get; set; }
    }
}
