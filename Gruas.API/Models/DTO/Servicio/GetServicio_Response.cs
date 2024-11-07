namespace Gruas.API.Models.DTO.Servicio
{
    public class GetServicio_Response
    {
        public Guid id { get; set; }
        //informacion del vehiculo
        public string tipo { get; set; }
        public string placas { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int anio { get; set; }
        public int numPersonas { get; set; }

        //informacion del traslado
        public string origen { get; set; }
        public string destino { get; set; }
        public string estado { get; set; }
        public string municipio { get; set; }
        public decimal kilometros { get; set; }
        public decimal tiempoEstimado { get; set; }

        //datos adicionales
        public string fugaCombustible { get; set; }
        public string llantasGiran { get; set; }
        public string esPosibleNeutral { get; set; }
        public int personasEnVehiculo { get; set; }
        public string lugar { get; set; }
        public int carril { get; set; }
        public int kilometro{ get; set; }
        public string tipoDeEstacionamiento { get; set; }
        public string pisoEstacionamiento { get; set; }
        public string vehiculoAccidentado { get; set; }
        

        //datos de pago
        //public decimal kilometros { get; set; }
        public decimal tarifaInicial { get; set; }
        public decimal costoPorKm { get; set; }
        public decimal tarifaDinamica { get; set; }
        public decimal maniobras { get; set; }
        public decimal totalSugerido { get; set; }

        public string estatus { get; set; }

        //datos del proveedor
        public string razonSocial { get; set; }
        public string telefono_1 { get; set; }
        public string telefono_2 { get; set; }

    }
}
