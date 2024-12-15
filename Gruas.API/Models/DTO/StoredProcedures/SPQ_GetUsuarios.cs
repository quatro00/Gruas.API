namespace Gruas.API.Models.DTO.StoredProcedures
{
    public class SPQ_GetUsuarios
    {
        public Guid id { get; set; }
        public string userName { get; set; }
        public string nombre { get; set; }
        public string apellidos { get; set; }
        public string correoElectronico { get; set; }
        public string telefono { get; set; }
        public string razonSocial { get; set; }
        public string Roles { get; set; }
        public int activo { get; set; }
    }
}
