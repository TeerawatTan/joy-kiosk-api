namespace JoyKioskApi.Services.CommonServices;

public interface ICommonService
{
    Task<HttpResponseMessage> CrmGetAsync(string? crmToken, string endpoint, object? data = default);
    Task<HttpResponseMessage> CrmPostAsync(string? crmToken, string endpoint, object data);
    Task<HttpResponseMessage> CrmPutAsync(string? crmToken, string endpoint, object data);
    Task<HttpResponseMessage> DeleteProductAsync(string? crmToken, string endpoint, object data);
    Task<HttpResponseMessage> PostUnAuthenAsync(string endpoint, object data);
}
