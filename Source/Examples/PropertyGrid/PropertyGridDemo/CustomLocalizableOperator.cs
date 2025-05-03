// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyItemFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

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

            if (declaringType != null)
            {
                if (declaringType.IsEnumOrNullableEnum()) 
                {
                    var enumMemberResourceKey = declaringType.Name + "." + (key ?? "-"); // in case it is NULL in Nullable<EnumType>

                    var enumMemberResourceValue = Translations.ResourceManager.GetString(enumMemberResourceKey);
                    if (enumMemberResourceValue != null)
                    {
                        value = enumMemberResourceValue;
                    }
                }
            }

            return value;
        }
    }
}