using Gruas.API.Models.DTO.Servicio;

namespace Gruas.API.Utils
{
    public class Helpers
    {
        private static readonly string apiKey = "AIzaSyAgZMnTv1dLh9cMz12KUrAuPk3VO22tzt4";

        public static async Task<ObtenerTiempoDistancias> GetDrivingDistance(string origin, string destination)
        {
            /*
            string originLatLng = "40.712776,-74.005974"; // Ejemplo: Nueva York, USA
            string destinationLatLng = "34.052235,-118.243683"; // Ejemplo: Los Ángeles, USA
             */
            using (HttpClient client = new HttpClient())
            {
                string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?units=metric&origins={origin}&destinations={destination}&departure_time=now&key={apiKey}";

                // Hacer la solicitud
                HttpResponseMessage response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // Leer la respuesta
                string responseBody = await response.Content.ReadAsStringAsync();

                // Procesar el JSON para obtener los datos necesarios
                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);
                string distanceText = jsonResponse.rows[0].elements[0].distance.text;
                
                double distanceValue = jsonResponse.rows[0].elements[0].distance.value;
                double distanceInKilometers = distanceValue / 1000;
                

                double durationNormal = jsonResponse.rows[0].elements[0].duration.value; // Duración normal en segundos
                double durationInTraffic = jsonResponse.rows[0].elements[0].duration_in_traffic.value; // Duración en tráfico en segundos

                // Calcular el porcentaje de congestión
                double congestionPercentage = ((durationInTraffic - durationNormal) / durationNormal) * 100;
                int kmTotales = (int)Math.Ceiling(distanceInKilometers);
                int minsNomal = (int)Math.Ceiling(durationNormal / 60);
                int minsTrafico = (int)Math.Ceiling(durationInTraffic / 60);


                return new ObtenerTiempoDistancias()
                {
                    kmTotales = kmTotales,
                    minsNomal = minsNomal,
                    minsTrafico = minsTrafico,
                    congestionPercentage = congestionPercentage,
                };
            }
        }

    }
}
