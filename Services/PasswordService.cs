namespace MySchool_System.Services
{
    public class PasswordService
    {
        public static string GetPassword()
        {
            Random random = new Random();
            string pass = "";

            char[] capitals = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray();
            char[] small = "abcdefghijklmnopqrstuvwxyz".ToCharArray();
            char[] symbols = "@!$#".ToCharArray();

            // Capital letter
            pass += capitals[random.Next(0, capitals.Length)];

            // Small letter
            pass += small[random.Next(0, small.Length)];

            // Symbol
            pass += symbols[random.Next(0, symbols.Length)];

            // Random number (5 digits)
            pass += random.Next(11111, 99999);

            return pass;
        }
    }
}
