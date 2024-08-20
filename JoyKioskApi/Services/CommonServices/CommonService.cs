using System.Net.Http.Headers;
using System.Text;

namespace JoyKioskApi.Services.CommonServices;

public class CommonService : ICommonService
{
    private readonly IConfiguration _configuration;

    public CommonService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<HttpResponseMessage> CrmGetAsync(string? crmToken, string endpoint, object? data = default)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CrmApi")!.ToString());
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        if (string.IsNullOrEmpty(crmToken))
        {
            var byteArray = Encoding.ASCII.GetBytes("admin:1234");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", crmToken);
        }
        
        return await client.GetAsync(endpoint);
    }

    public async Task<HttpResponseMessage> CrmPostAsync(string? crmToken, string endpoint, object data)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CrmApi")!.ToString());
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (string.IsNullOrEmpty(crmToken))
        {
            var byteArray = Encoding.ASCII.GetBytes("admin:1234");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", crmToken);
        }

        return await client.PostAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> CrmPutAsync(string? crmToken, string endpoint, object data)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CrmApi")!.ToString());
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (string.IsNullOrEmpty(crmToken))
        {
            var byteArray = Encoding.ASCII.GetBytes("admin:1234");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", crmToken);
        }

        return await client.PutAsJsonAsync(endpoint, data);
    }

    public async Task<HttpResponseMessage> DeleteProductAsync(string? crmToken, string endpoint, object data)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CrmApi")!.ToString());
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        if (string.IsNullOrEmpty(crmToken))
        {
            var byteArray = Encoding.ASCII.GetBytes("admin:1234");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }
        else
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", crmToken);
        }

        return await client.DeleteAsync(endpoint);
    }

    public async Task<HttpResponseMessage> PostUnAuthenAsync(string endpoint, object data)
    {
        HttpClient client = new();
        client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("CrmApi")!.ToString());
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(_configuration["AuthToken"]!.ToString());
        return await client.PostAsJsonAsync(endpoint, data);
    }

}