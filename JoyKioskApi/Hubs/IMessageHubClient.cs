namespace JoyKioskApi.Hubs
{
    public interface IMessageHubClient
    {
        Task ResultPayment(string message);
    }
}
