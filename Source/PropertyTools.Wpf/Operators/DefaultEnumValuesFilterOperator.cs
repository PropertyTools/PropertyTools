// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DefaultEnumValuesFilterOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Operators
{
    using PropertyTools.DataAnnotations;
    using PropertyTools.Wpf.Common;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class DefaultEnumValuesFilterOperator : IEnumValuesFilterOperator
    {
        /// <inheritdoc/>        
        /// <exception cref="InvalidOperationException"></exception>
        public IEnumerable<Enum> GetEnumValues(IPropertyItem pi, object instance, bool browsableOnly = true)
        {
            if (!pi.PropertyType.IsEnumOrNullableEnum())
            {
                throw new InvalidOperationException($"The PropertyType ({pi.PropertyType.FullName}) must be enum / nullable enum type.");
            }

            var enumType = pi.PropertyType;
            var ult = Nullable.GetUnderlyingType(enumType);
            var isNullable = ult != null;
            if (isNullable)
            {
                enumType = ult;
            }

            var enumValues = Enum.GetValues(enumType).Cast<Enum>();

            var filtersMetadata = GetEnumFilterMetadata(pi, instance);
            var mode = filtersMetadata.Item1;
            var items = filtersMetadata.Item2;
            if (mode != null && items != null)
            {
                switch (mode.Value)
                {
                    case EnumFilterAttribute.FilteringMode.Include:
                        enumValues = enumValues.Intersect(items);
                        break;

                    case EnumFilterAttribute.FilteringMode.Exclude:
                        enumValues = enumValues.Except(items);
                        break;
                }
            }
            
            if (browsableOnly)
            {
                enumValues = enumValues.FilterOnBrowsableAttribute();
            }

            return enumValues;
        }

        /// <inheritdoc/>
        public IEnumerable<object> GetEnumValuesWithNullEntry(IPropertyItem pi, object instance, bool nullAtStart, bool browsableOnly = true)
        {
            if (!pi.PropertyType.IsEnumOrNullableEnum())
            {
                throw new InvalidOperationException($"The PropertyType ({pi.PropertyType.FullName}) must be enum / nullable enum type.");
            }
            
            var enumValues = GetEnumValues(pi, instance, browsableOnly);
            
            var result = enumValues.Cast<object>().ToList();
            
            var isNullable = Nullable.GetUnderlyingType(pi.PropertyType) != null;
            if (isNullable)
            {
                if (nullAtStart)
                {
                    result.Insert(0, null);
                }
                else
                {
                    result.Add(null);
                }
            }

            return result;
        }

        protected virtual Tuple<EnumFilterAttribute.FilteringMode?, Enum[]> GetEnumFilterMetadata(IPropertyItem pi, object instance)
        {
            Enum[] items = Array.Empty<Enum>();
            EnumFilterAttribute.FilteringMode? mode = null;

            var enumMembersAttribute = pi.Descriptor.Attributes.OfType<EnumFilterAttribute>().FirstOrDefault();
            if (enumMembersAttribute != null)
            {
                mode = enumMembersAttribute.Mode;
                items = enumMembersAttribute.Items;
            }

            return new Tuple<EnumFilterAttribute.FilteringMode?, Enum[]>(mode, items);
        }
    }
}