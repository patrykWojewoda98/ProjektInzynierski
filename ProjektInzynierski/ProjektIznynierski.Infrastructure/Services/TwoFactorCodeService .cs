using ProjektIznynierski.Domain.Abstractions;
using System.Security.Cryptography;
using System.Text;

namespace ProjektIznynierski.Infrastructure.Services
{
    internal class TwoFactorCodeService : ITwoFactorCodeService
    {
        public string GenerateCode()
            => RandomNumberGenerator.GetInt32(100000, 999999).ToString();

        public string HashCode(string code)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(
                sha256.ComputeHash(Encoding.UTF8.GetBytes(code)));
        }
    }
}
