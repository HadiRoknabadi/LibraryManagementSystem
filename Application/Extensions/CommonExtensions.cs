using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Application.Extensions
{
    public static class CommonExtensions
    {
        public static string GetEnumName(this System.Enum myEnum)
        {
            var enumMember = myEnum.GetType().GetMember(myEnum.ToString()).FirstOrDefault();

            return enumMember?
                .GetCustomAttribute<DisplayAttribute>()?
                .GetName()
                ?? myEnum.ToString();
        }
    }
}