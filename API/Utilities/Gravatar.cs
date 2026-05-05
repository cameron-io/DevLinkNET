namespace API.Utilities
{
    public static class Gravatar
    {
        public static string GetGravatarUrl(string email, int size = 200)
        {
            var hash = ComputeHash(email.Trim().ToLower());
            return $"https://www.gravatar.com/avatar/{hash}?s={size}&d=identicon";
        }

        private static string ComputeHash(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            var hashBytes = System.Security.Cryptography.SHA256.HashData(inputBytes);
            return Convert.ToHexStringLower(hashBytes);
        }
    }
}