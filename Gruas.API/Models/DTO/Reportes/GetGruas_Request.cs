namespace Gruas.API.Models.DTO.Reportes
{
    public class GetGruas_Request
    {
        public Guid? proveedorId { get; set; }
        public int? tipoGruaId { get; set; }
        public string? placas { get; set; }
    }
}
