
namespace PropertyGridDemo
{
    using PropertyGridDemo.Resources;
    using PropertyTools.Wpf;
    using PropertyTools.Wpf.Operators;
    using System;

    public class CustomLocalizableOperator : DefaultLocalizableOperator
    {
        public override string GetLocalizedString(string key, Type declaringType)
        {
            var value = key;

            String resourceKey = null;

            if (declaringType?.IsEnumOrNullableEnum() == true)
            {
                // resource strings for enum values are retrieved by composite key  "{enumType.FullName}.{enum member}"
                resourceKey = declaringType.FullName + "." + (key ?? "-"); // in case it is NULL in Nullable<EnumType>
            }
            else if (key != null)
            {
                resourceKey = key;
                if (declaringType != null)
                {
                    resourceKey = declaringType.FullName + "." + resourceKey;
                }
            }

            if (resourceKey != null)
            {
                var resourceValue = Translations.ResourceManager.GetString(resourceKey);
                if (resourceValue != null)
                {
                    value = resourceValue;
                }
            }

            return value;
        }
    }
}