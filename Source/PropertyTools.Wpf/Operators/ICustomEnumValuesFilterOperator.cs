// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ICustomEnumValuesFilterOperator.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf.Operators
{
    public interface ICustomEnumValuesFilterOperator
    {
        void UseEnumValuesFilterOperator(IEnumValuesFilterOperator customEnumValuesFilterOperator);
    }
}