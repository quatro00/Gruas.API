namespace Gruas.API.Models.DTO.Usuarios
{
    public class UpdateUsuario_Request
    {
        public string correoElectronico { get; set; }
        public string nombreUsuario { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string telefono { get; set; }
        public Guid proveedorId { get; set; }
    }
}
