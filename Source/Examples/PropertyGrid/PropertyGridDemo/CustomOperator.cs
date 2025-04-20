// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LocalPropertyItemFactory.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemo
{
    using PropertyGridDemo.Resources;
    using PropertyTools.Wpf;
    using System;

    public class CustomOperator : PropertyGridOperator
    {
        protected override string GetLocalizedString(string key, Type declaringType)
        {
            var value = key;

            if (declaringType != null)
            {
                if (declaringType.IsEnum == true || IsNullableEnum(declaringType)) 
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