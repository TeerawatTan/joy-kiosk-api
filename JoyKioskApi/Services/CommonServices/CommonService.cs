using System.Net.Http.Headers;
using System.Text;

namespace JoyKioskApi.Services.CommonServices;

public class CommonService : ICommonService
{
    private readonly IConfiguration _configuration;
    private static HttpClient _client = new();

    public CommonService(IConfiguration configuration)
    {
        _configuration = configuration;
        _client.BaseAddress = new Uri(_configuration["CrmApi"]!.ToString());
        _client.DefaultRequestHeaders.Accept.Clear();
        _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_configuration["AuthToken"]!.ToString());

        var byteArray = Encoding.ASCII.GetBytes("admin:1234");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));

    }

    public async Task<HttpResponseMessage> CrmGetAsync(string endpoint) => await _client.GetAsync(endpoint);

    public async Task<HttpResponseMessage> CrmPostAsync(string endpoint, object data) => await _client.PostAsJsonAsync(endpoint, data);

    public async Task<HttpResponseMessage> CrmPutAsync(string endpoint, object data) => await _client.PutAsJsonAsync(endpoint, data);

    public async Task<HttpResponseMessage> DeleteProductAsync(string endpoint) => await _client.DeleteAsync(endpoint);

}