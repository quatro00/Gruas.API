namespace Gruas.API.Models.Grua
{
    public class CreateGrua_Request
    {
        public Guid proveedorId { get; set; }
        public string placas { get; set; }
        public string marca { get; set; }
        public string modelo { get; set; }
        public int tipoGruaId { get; set; }
        public int anio { get; set; }
        public int activo { get; set; }
    }
}
