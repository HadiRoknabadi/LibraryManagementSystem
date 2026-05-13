namespace Application.Generators
{
    public class Generator
    {
        public static string GenerateUniqCode()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }

        public static string CreateBookCopyCode(string lastCode)
        {
            int nextNumber = 1;

            if (!string.IsNullOrWhiteSpace(lastCode))
            {
                var numberPart = lastCode.Split('-')[1];
                nextNumber = int.Parse(numberPart) + 1;
            }

            return $"BC-{nextNumber:D4}";
        }
    }
}