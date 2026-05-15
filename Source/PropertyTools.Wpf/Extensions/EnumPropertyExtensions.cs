// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnumPropertyExtensions.cs" company="PropertyTools">
//   Copyright (c) 2026 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
using PropertyTools.DataAnnotations;
using PropertyTools.Wpf.Common;
using PropertyTools.Wpf.Operators;
using System;
using System.Linq;
using System.Reflection;

namespace PropertyTools.Wpf.Extensions
{
    public static class EnumPropertyExtensions
    {
        /// <summary>
        /// Tries to initialize the <see cref="IPropertyItem.EnumMetadata"/>.
        /// </summary>
        /// <param name="pi">The property item</param>
        /// <param name="localizedPropertyOperator">The localizable operator</param>
        /// <param name="enumValuesFilterOperator">The enum values filter operator</param>
        /// <param name="instance">The instance being edited (in PropertyGrid, DataGrid row, etc.)</param>
        public static void TrySetEnumMetadata(this IPropertyItem pi, 
            ILocalizableOperator localizedPropertyOperator, 
            IEnumValuesFilterOperator enumValuesFilterOperator,
            object instance)
        {
            var propertyType = pi.PropertyType;
            if (propertyType.IsEnumOrNullableEnum())
            {
                var enumType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;
                var enumValues = enumValuesFilterOperator.GetEnumValues(pi, instance, browsableOnly: true);

                var enumMissingZeroBehaviorAttribute = pi.Descriptor.GetFirstAttributeOrDefault<EnumMissingZeroBehaviorAttribute>();
                pi.EnumMetadata.InitializeWithDefault = enumMissingZeroBehaviorAttribute?.InitializeWithDefault;
                pi.EnumMetadata.ResetToDefault = enumMissingZeroBehaviorAttribute?.ResetToDefault;

                pi.EnumMetadata.EnumType = enumType;

                pi.EnumMetadata.EnumDisplayNames = enumValues
                   .ToDictionary(x => x,
                    x =>
                    {
                        var fieldInfo = enumType.GetFields(BindingFlags.Public | BindingFlags.Static)
                            .FirstOrDefault(f => f.GetValue(null).Equals(x));

                        // System.ComponentModel.DisplayNameAttribute is not supported for fields (enum members)                           
                        var displayNameAttribute = fieldInfo.GetCustomAttribute<PropertyTools.DataAnnotations.DisplayNameAttribute>();

                        var descriptionAttribute1 = fieldInfo.GetCustomAttribute<System.ComponentModel.DescriptionAttribute>();
                        var descriptionAttribute2 = fieldInfo.GetCustomAttribute<PropertyTools.DataAnnotations.DescriptionAttribute>();

                        var enumMemberDisplayName = displayNameAttribute?.DisplayName
                           ?? descriptionAttribute1?.Description
                           ?? descriptionAttribute2?.Description
                           ?? x.ToString();

                        return localizedPropertyOperator.GetLocalizedString(enumMemberDisplayName, enumType);
                    });

                if (propertyType.IsNullableEnum())
                {
                    pi.EnumMetadata.IsNullableEnum = true;
                    pi.EnumMetadata.EnumDisplayNull = localizedPropertyOperator.GetLocalizedString(null, enumType);
                }
            }
        }
    }
}
