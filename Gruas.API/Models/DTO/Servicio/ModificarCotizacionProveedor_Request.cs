namespace Gruas.API.Models.DTO.Servicio
{
    public class ModificarCotizacionProveedor_Request
    {
        public Guid cotizacionId { get; set; }
        public Guid gruaId { get; set; }
        public decimal costo { get; set; }
        public decimal tiempo { get; set; }
    }
}
