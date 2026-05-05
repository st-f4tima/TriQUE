using BCrypt.Net;

namespace TriQue.Helpers
{
    public static class PasswordHelper
    {
        public static string Hash(string password)
            => BCrypt.Net.BCrypt.HashPassword(password);

        public static bool Verify(string password, string hash)
        {
            try
            {
                // if hash is not a valid BCrypt hash, return false instead of crashing
                if (string.IsNullOrWhiteSpace(hash) || !hash.StartsWith("$2"))
                    return false;

                return BCrypt.Net.BCrypt.Verify(password, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }

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