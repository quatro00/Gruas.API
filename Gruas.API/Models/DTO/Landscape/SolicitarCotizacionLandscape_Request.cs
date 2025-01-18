namespace Gruas.API.Models.DTO.Landscape
{
    public class SolicitarCotizacionLandscape_Request
    {
        public string nombre { get; set; }
        public string telefono { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public string tipoVehiculo { get; set; }
        public string fecha { get; set; }
    }
}
