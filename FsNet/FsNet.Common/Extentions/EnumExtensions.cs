using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace FsNet.Common.Extentions
{
    public static class EnumExtensions
    {
        public static string GetLocalizedDescription(this Enum @enum)
        {
            if (@enum == null) return null;

            var description = @enum.ToString();

            var fieldInfo = @enum.GetType().GetField(description);
            var attributes = (DescriptionAttribute[])fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attributes.Any() ? attributes[0].Description : description;
        }

        public static string GetDescription<T>(this T enumerationValue)
            where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException(@"EnumerationValue must be of Enum type", $"enumerationValue");
            }

            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumerationValue.ToString();
        }
   
    }

    public class LocalizedDescriptionAttribute : DescriptionAttribute
    {
        private PropertyInfo _nameProperty;
        private Type _resourceType;

        public LocalizedDescriptionAttribute(string displayNameKey)
            : base(displayNameKey)
        {

        }

        public Type ResourceType
        {
            get => _resourceType;
            set
            {
                _resourceType = value;
                _nameProperty = _resourceType.GetProperty(this.Description, BindingFlags.Static | BindingFlags.Public);
            }
        }

        public override string Description
        {
            get
            {
                if (_nameProperty == null)
                {
                    return base.Description;
                }

                return (string)_nameProperty.GetValue(_nameProperty.DeclaringType, null);
            }
        }
    }
}
