﻿namespace Gruas.API.Models.DTO.Servicio
{
    public class AsignarGrua_Request
    {
        public Guid servicioId { get; set; }
        public Guid gruaId {get; set; }
        public decimal costo { get; set; }
        public decimal tiempo { get; set; }
    }
}
