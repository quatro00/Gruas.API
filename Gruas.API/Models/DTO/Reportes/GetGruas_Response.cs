namespace Gruas.API.Models.DTO.Reportes
{
    public class GetGruas_Response
    {
        public Guid id { get; set; }
        public string proveedor { get; set; }
        public string tipoGrua { get; set; }
        public string placas { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int anio { get;set; }
        public int activo { get; set; }
    }
}
