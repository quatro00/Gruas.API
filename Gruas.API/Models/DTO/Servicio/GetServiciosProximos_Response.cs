
namespace Gruas.API.Models.DTO.Servicio
{
    public class GetServiciosProximos_Response
    {
        public Guid id { get; set; }
        public string folio {  get; set; }
        public string tipoServicio { get; set; }
        public DateTime fecha { get; set; }
        public int fechaDia { get; set; }
        public string fechaMes { get; set; }
        public string fechaHora { get; set; }
        public string cliente { get; set; }
        public string clienteTelefono { get; set; }
        public string origen { get; set; }
        public string origenMunicipio { get; set; }
        public string destino { get; set; }
        public string destinoMunicipio { get; set; }
        public string origenLat { get; set; }
        public string origenLon { get; set; }
        public string destinoLat { get; set; }
        public string destinoLon { get; set; }
        public string origenReferencia { get; set; }
        public string destinoReferencia { get;set; }
        public bool vehiculoAccidentado { get; set; }
        public bool fugaCombustible { get; set; }
        public bool llantasGiran { get; set; }
        public bool puedeNeutral { get; set; }
        public int personasEnVehiculo { get; set; }
        public string lugarUbicuidad { get; set; }
        public decimal carreteraCarril {  get; set; }
        public decimal carreteraKm { get; set; }
        public string carreteraDestino { get; set; }
        public string estacionamientoTipo { get; set; }
        public string estacionamientoPiso { get; set; }
        public string vehiculoMarca { get; set; }
        public string vehiculoModelo { get; set; }
        public int vehiculoAnio { get; set; }
        public string vehiculoColor {  get; set; }
        public string vehiculoCuentaPlacas { get; set; }
        public string vehiculoPlacas { get; set; }
        public bool vehiculoCuentaModificaciones { get; set; }
        public string vehiculoDescripcionModificaciones { get; set; }
        public decimal distancia { get; set; }
        public decimal cuotaKm { get; set; }
        public decimal banderazo { get; set; }
        public bool maniobras {  get; set; }
        public decimal maniobrasCosto { get; set; }
        public decimal totalSugerido { get; set; }
        public string estatusServicio { get; set; }
        public string razonSocial { get; set; }
        public string proveedorTelefono { get; set; }
        public string grua { get; set; }
        public string gruaPlacas { get; set; }
        public decimal total {  get; set; }
        public decimal tiempoLlegada { get; set; }
        public decimal direccionCongestion { get; set; }
        public decimal direccionKmTotales { get; set; }
        public decimal direccionMinsNormal { get; set; }
        public decimal direccionMinsTrafico { get; set; }
    }
}
