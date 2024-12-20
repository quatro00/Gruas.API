using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace Gruas.API.Models.DTO.Reportes
{
    public class GetServiciosProveedor_Request
    {
        public int? estatusServicioId { get; set; }
        public int? tipoServicioId { get; set; }
        public string periodo {  get; set; }
    }
}
