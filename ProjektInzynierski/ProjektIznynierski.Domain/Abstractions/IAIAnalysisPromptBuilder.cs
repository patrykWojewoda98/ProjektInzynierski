namespace ProjektIznynierski.Domain.Abstractions
{
    public interface IAIAnalysisPromptBuilder
    {
        Task<string> BuildPromptAsync(int clientId,int investInstrumentId,CancellationToken cancellationToken);
    }
}
