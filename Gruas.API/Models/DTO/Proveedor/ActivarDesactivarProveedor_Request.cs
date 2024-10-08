using OfficeOpenXml.FormulaParsing.Excel.Functions;

namespace Gruas.API.Models.DTO.Proveedor
{
    public class ActivarDesactivarProveedor_Request
    {
        public Guid id { get; set; }
        public bool activo { get; set; }
    }
}
