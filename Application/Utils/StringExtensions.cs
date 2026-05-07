using System.Text;
using System.Text.RegularExpressions;

namespace Application.Utils
{
    public static class StringExtensions
    {
        public static bool HasValue(this string value, bool ignoreWhiteSpace = true)
        {
            return ignoreWhiteSpace ? !string.IsNullOrWhiteSpace(value) : !string.IsNullOrEmpty(value);
        }

        public static string ToCurrency(this int value)
        {
            //fa-IR => current culture currency symbol => ریال
            //123456 => "123,123ریال"
            return value.ToString("C0");
        }

        public static string ToCurrency(this decimal value)
        {
            return value.ToString("C0");
        }

        public static string En2Fa(this string str)
        {
            return str.Replace("0", "۰")
                .Replace("1", "۱")
                .Replace("2", "۲")
                .Replace("3", "۳")
                .Replace("4", "۴")
                .Replace("5", "۵")
                .Replace("6", "۶")
                .Replace("7", "۷")
                .Replace("8", "۸")
                .Replace("9", "۹");
        }

        public static string Fa2En(this string str)
        {
            return str.Replace("۰", "0")
                .Replace("۱", "1")
                .Replace("۲", "2")
                .Replace("۳", "3")
                .Replace("۴", "4")
                .Replace("۵", "5")
                .Replace("۶", "6")
                .Replace("۷", "7")
                .Replace("۸", "8")
                .Replace("۹", "9")
                //iphone numeric
                .Replace("٠", "0")
                .Replace("١", "1")
                .Replace("٢", "2")
                .Replace("٣", "3")
                .Replace("٤", "4")
                .Replace("٥", "5")
                .Replace("٦", "6")
                .Replace("٧", "7")
                .Replace("٨", "8")
                .Replace("٩", "9");
        }

        public static string FixPersianChars(this string str)
        {
            return str.Replace("ﮎ", "ک")
                .Replace("ﮏ", "ک")
                .Replace("ﮐ", "ک")
                .Replace("ﮑ", "ک")
                .Replace("ك", "ک")
                .Replace("ي", "ی")
                .Replace(" ", " ")
                .Replace("‌", " ")
                .Replace("ھ", "ه");//.Replace("ئ", "ی");
        }

        public static string CleanString(this string str)
        {
            return str.Trim().FixPersianChars().Fa2En().NullIfEmpty();
        }

        public static string NullIfEmpty(this string str)
        {
            return str?.Length == 0 ? null : str;
        }

        public static string FixText(this string text) => text?.Trim().Replace("  ", " ");
        public static string FixEmail(string email) => email.Trim().ToLower().Replace(" ", "");

        public static string RemoveHtmlTagsExceptBreak(string text) => Regex.Replace(text, @"<(?!br[\x20/>])[^<>]+>", string.Empty);
        public static string ReplaceNewLineTextArea(string text) => text?.Replace(Environment.NewLine, "<br />");
        public static string ReplaceBrToNewLine(string text) => text?.Replace("<br />", Environment.NewLine);

        public static string FixTextForUrl(this string text)
        {
            return text.Replace(" ", "-");
        }

        public static string ConvertBrToNewLine(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace("<br/>", Environment.NewLine);
        }

        public static string ConvertNewLineToBr(this string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return text.Replace(Environment.NewLine, "<br/>");
        }

        public static string FixedEmail(this string email)
        {
            return email.Trim().ToLower();
        }

        public static string[] SplitTags(this string tags)
        {
            return tags.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }


        public static string FixTitleForUrl(this string url)
        {
            return url.Replace(" ", "-").Replace("+", "").Replace("#", "");
        }

        public static string FixUrlToTitle(this string title)
        {
            return title.Replace("-", " ");
        }

        public static string StripHTML(this string input)
        {
            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string GetFirstSectionOfString(this string text, int length = 360)
        {

            if (text.Length >= length)
            {
                var shorted_string = new StringBuilder();

                shorted_string.Append(text.Substring(0, length));
                shorted_string.Append("...");

                return shorted_string.ToString();
            }

            return text;
        }

        public static string PhoneNumberObfuscator(this string phoneNumber)
        {
            var preNumber = phoneNumber.Substring(0, 4);

            var length=phoneNumber.Length;

            var twoLastDigits = phoneNumber.Substring(9, 2);

            var obfuscatedPhonNumber = new StringBuilder();

            obfuscatedPhonNumber.Append(preNumber);

            obfuscatedPhonNumber.Append("*****");

            obfuscatedPhonNumber.Append(twoLastDigits);

            return obfuscatedPhonNumber.ToString();
        }
    }
}
