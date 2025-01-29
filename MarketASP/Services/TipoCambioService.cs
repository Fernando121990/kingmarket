using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class TipoCambio
{
    public decimal Compra { get; set; }
    public decimal Venta { get; set; }
    public string Fecha { get; set; }
}

public class TipoCambioService
{
    private readonly string _apiUrl;

    public TipoCambioService()
    {
        // Leer la URL del servicio desde el web.config
        _apiUrl = ConfigurationManager.AppSettings["TipoCambioApiUrl"];
    }

    public async Task<TipoCambio> ObtenerTipoCambioAsync()
    {
        using (HttpClient client = new HttpClient())
        {
            var response = await client.GetAsync(_apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string json = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TipoCambio>(json);
            }

            return null; // Manejo de error: retorna null si la solicitud falla
        }
    }
}
