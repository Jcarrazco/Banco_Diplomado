using Banco.Core.Dtos;
using Banco.Core.IServices;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Banco.Servicios.Renapo
{
    public class CurpServicio: ICurpService
    {
        public async Task<string> GenerarCurp(SolicitudDto solicitud)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://utilidades.vmartinez84.xyz/api/Curp");
            request.Headers.Add("accept", "application/json");
            var content = new StringContent(JsonConvert.SerializeObject(solicitud), null, "application/json");
            request.Content = content;
            var response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var jsonResponse = await response.Content.ReadAsStringAsync();
                CurpDto result = JsonConvert.DeserializeObject<CurpDto>(jsonResponse);
                JObject resultado = JObject.Parse(jsonResponse);
                var curp = resultado["curp"]?.ToString();

                return result.Curp;
            }

            return string.Empty;

        }
    }
}
