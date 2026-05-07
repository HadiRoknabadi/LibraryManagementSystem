namespace Application.Generators
{
    public class Generator
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}