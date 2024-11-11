namespace Gruas.API.Models.DTO.Reportes
{
    public class GetProveedores_Response
    {
        public Guid id { get; set; }
        public int noProveedor { get; set; }
        public string razonSocial { get; set; }
        public string direccion { get; set; }
        public string telefono_1 { get; set; }
        public string telefono_2 { get; set; }
        public string rfc { get; set; }
        public string banco { get; set; }
        public string cuenta { get; set; }
        public string estado { get; set; }
        public decimal comision { get; set; }
        public bool activo { get; set; }
    }
}
