using System.ComponentModel.DataAnnotations;

namespace Notebook.Server.Config
{
    public static class RoleNameExtention
    {
        public static string GetDisplayName(this System.Enum enumValue)
        {
            var type = enumValue.GetType();
            var memInfo = type.GetMember(enumValue.ToString());
            var attributes = memInfo[0].GetCustomAttributes(typeof(DisplayAttribute), false);
            return attributes.Length > 0 ? ((DisplayAttribute)attributes[0]).Name : enumValue.ToString();
        }
    }
}
