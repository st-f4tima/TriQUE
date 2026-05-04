using BCrypt.Net;

namespace TriQue.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool Verify(string password, string hash)
            => BCrypt.Net.BCrypt.Verify(password, hash);

        public static string GenerateTempPassword(int length = 8)
        {
            const string chars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
            var rng = new Random();
            var result = new char[length];
            for (int i = 0; i < length; i++)
                result[i] = chars[rng.Next(chars.Length)];
            return new string(result);
        }
    }
}