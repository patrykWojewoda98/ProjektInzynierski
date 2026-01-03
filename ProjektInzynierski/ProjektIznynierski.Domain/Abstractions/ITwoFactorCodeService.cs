namespace ProjektIznynierski.Domain.Abstractions
{
    public interface ITwoFactorCodeService
    {
        string GenerateCode();
        string HashCode(string code);
    }
}