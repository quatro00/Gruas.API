namespace Gruas.API.Models.DTO.Servicio
{
    public class GetCotizaciones_Response
    {
        public Guid id { get; set; }
        public decimal tiempoCotizado { get; set; }
        public decimal costoCotizado { get; set; }
        public int seleccionada { get; set; }
        public string razonSocial { get; set; }
        public string proveedorTelefono1 { get; set; }
        public string proveedorTelefono2 { get; set; }
        public string proveedorCorreo { get; set; }
        public string gruaPlacas { get; set; }
        public string gruaMarca { get; set; }
        public string gruaTipo { get; set; }
        public int gruaAnio { get; set; }
    }
}
