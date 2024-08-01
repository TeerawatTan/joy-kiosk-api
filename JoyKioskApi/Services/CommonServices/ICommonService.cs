namespace JoyKioskApi.Services.CommonServices;

public interface ICommonService
{
    Task<HttpResponseMessage> CrmGetAsync(string username, string password, string endpoint, object? data = default);
    Task<HttpResponseMessage> CrmPostAsync(string username, string password, string endpoint, object data);
    Task<HttpResponseMessage> CrmPutAsync(string username, string password, string endpoint, object data);
    Task<HttpResponseMessage> DeleteProductAsync(string username, string password, string endpoint, object data);
    Task<HttpResponseMessage> PostUnAuthenAsync(string endpoint, object data);
}
