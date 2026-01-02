// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CustomLocalizableOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace DataGridDemo.Operators
{
    using DataGridDemo.Resources;
    using PropertyTools.Wpf.Operators;
    using System;

    public class CustomLocalizableOperator : DefaultLocalizableOperator
    {
        public override string GetLocalizedString(string key, Type declaringType)
        {
            var value = key;

            var resourceKey = key;
            if (resourceKey != null)
            {
                if (declaringType != null)
                {
                    resourceKey = declaringType.FullName + "." + resourceKey;
                }

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