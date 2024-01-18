namespace YBS2.Service.Utils
{
    public static class TextUtils
    {
        public static string Capitalize(string text)
        {
            text = text.ToLower().Trim();
            string[] stringArray = text.Split(' ');
            for (int i = 0; i < stringArray.Length; i++)
            {
                char c = char.ToUpper(stringArray[i][0]);
                stringArray[i] = c.ToString() + stringArray[i].Remove(0, 1);
            }
            return string.Join(' ', stringArray);
        }

        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source?.IndexOf(source, StringComparison.OrdinalIgnoreCase) > -1;
        }
        public static string GenerateRandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            // Use StringBuilder for better performance when manipulating strings in a loop
            var randomString = new System.Text.StringBuilder(length);

            for (int i = 0; i < length; i++)
            {
                randomString.Append(chars[random.Next(chars.Length)]);
            }

            return randomString.ToString();
        }
    }
}
