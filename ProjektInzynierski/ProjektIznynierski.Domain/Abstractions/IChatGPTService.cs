namespace ProjektInzynierski.Application.Interfaces
{
    public interface IChatGPTService
    {
        Task<string> GetResponseAsync(string message);
    }
}