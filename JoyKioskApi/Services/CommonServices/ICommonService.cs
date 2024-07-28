namespace JoyKioskApi.Services.CommonServices;

public interface ICommonService
{
    Task<HttpResponseMessage> CrmGetAsync(string endpoint);
    Task<HttpResponseMessage> CrmPostAsync(string endpoint, object data);
    Task<HttpResponseMessage> CrmPutAsync(string endpoint, object data);
    Task<HttpResponseMessage> DeleteProductAsync(string endpoint);
}
