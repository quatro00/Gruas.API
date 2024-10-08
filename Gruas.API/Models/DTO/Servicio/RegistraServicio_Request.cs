namespace Gruas.API.Models.DTO.Servicio
{
    public class RegistraServicio_Request
    {
        public string? telefono { get; set; }
        public string? correoElectronico { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public string? tipoVehiculo { get; set; }
        public string? tipoVehiculoEspecificacion { get; set; }
        public string? origen { get; set; }
        public string? referenciaOrigen { get; set; }
        public string? destino { get; set; }
        public string? referenciaDestino { get; set; }
        public int accidente { get; set; }
        public int fugaCombustible { get; set; }
        public int llantasGiran { get; set; }
        public int puedeNeutral { get; set; }
        public string? lugarUbicuidad { get; set; }
        public int cantidadPersonas { get; set; }
        public string? carril { get; set; }
        public string? km { get; set; }
        public string? ciudad { get; set; }
        public string? tipoEstacionamiento { get; set; }
        public string? piso { get; set; }
        public string? marca { get; set; }
        public string? modelo { get; set; }
        public int anio { get; set; }
        public string? color { get; set; }
        public string permiso { get; set; }
        public string? placaPermiso { get; set; }
        public int modificaciones { get; set; }
        public string? modificacionesEspecificacion { get; set; }
        public decimal? LatOrigen { get; set; }
        public decimal? LatDestino { get; set; }
        public decimal? lngOrigen { get; set; }
        public decimal? lngDestino { get; set; }
        public string? municipioOrigen { get; set; }
        public string? municipioDestino { get; set; }
        public DateTime fecha { get; set; }



    }
}
