namespace Gruas.API.Models.DTO.Servicio
{
    public class GetServiciosDisponibles_Request
    {
        public Guid id { get; set; }
        public string folio { get; set; }
        public string estatus { get; set; }
        public string origen { get; set; }
        public string destino { get; set; }
        public decimal kms { get; set; }
        public string maniobras { get; set; }
        public string tipoServicio { get; set; }
        public string tipoVehiculo { get; set; }
        public decimal totalSugerido { get; set; }
        public decimal totalCotizado { get; set; }
        public decimal tiempoCotizado { get; set; }
        public string latOrigen { get; set; }
        public string lonOrigen { get; set; }
        public string fecha { get; set; }
        public string hora { get; set; }
        public Guid? cotizacionId { get; set; }
    }
}
