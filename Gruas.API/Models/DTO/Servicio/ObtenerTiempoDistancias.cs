namespace Gruas.API.Models.DTO.Servicio
{
    public class ObtenerTiempoDistancias
    {
        public double congestionPercentage { get; set; }
        public int kmTotales { get; set; }
        public int minsNomal { get; set; }
        public int minsTrafico { get; set; }
    }
}
