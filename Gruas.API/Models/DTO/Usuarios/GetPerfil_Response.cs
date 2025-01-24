namespace Gruas.API.Models.DTO.Usuarios
{
    public class GetPerfil_Response
    {
        public Guid id { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public string rfc {  get; set; }
        public string nombre { get; set; }
        public string nombreUsuario { get; set; }
        public string telefono { get; set; }
        public string correoEletronico { get; set; }

    }
}
