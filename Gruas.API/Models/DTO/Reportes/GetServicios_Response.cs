
namespace Gruas.API.Models.DTO.Reportes
{
    public class GetServicios_Response
    {
        public Guid id { get; set; }
        public int folio { get; set; }
        public string cliente { get; set; }
        public string telefono { get; set; }
        public string estado { get; set; }
        public string municipio { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public decimal totalSugerido { get; set; }
        public decimal total { get; set; }
        public decimal comision { get; set; }
        public string estatus { get; set; }
        public string proveedor { get; set; }
        public string grua { get; set; }
        public string tipo { get; set; }
        public int numCotizaciones { get; set; }
    }
}
