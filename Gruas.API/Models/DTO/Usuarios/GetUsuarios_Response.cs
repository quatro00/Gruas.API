namespace Gruas.API.Models.DTO.Usuarios
{
    public class GetUsuarios_Response
    {
        public Guid id { get; set; }
        public string userName { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string proveedor { get; set; }
        public int activo { get; set; }
    }
}
