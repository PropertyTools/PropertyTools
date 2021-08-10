// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HexConverter.cs" company="PropertyTools">
//   Copyright (c) 2021 PropertyTools contributors
// </copyright>
// <summary>
//   Converts Complex instances to string instances.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace ExampleLibrary
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Windows.Data;

    /// <summary>
    /// Converts numeric and byte array instances to <see cref="string" /> hex instances.
    /// </summary>
    [ValueConversion(typeof(ulong), typeof(string))]
    [ValueConversion(typeof(long), typeof(string))]
    [ValueConversion(typeof(uint), typeof(string))]
    [ValueConversion(typeof(int), typeof(string))]
    [ValueConversion(typeof(ushort), typeof(string))]
    [ValueConversion(typeof(short), typeof(string))]
    [ValueConversion(typeof(byte), typeof(string))]
    [ValueConversion(typeof(byte[]), typeof(string))]
    public class HexConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ulong u64) return $"0x{u64:X16}";
            if (value is long i64) return $"0x{i64:X16}";
            if (value is uint u32) return $"0x{u32:X8}";
            if (value is int i32) return $"0x{i32:X8}";
            if (value is ushort u16) return $"0x{u16:X4}";
            if (value is short i16) return $"0x{i16:X4}";
            if (value is byte b) return $"0x{b:X2}";
            if (value is byte[] bArray)
            {
                return BitConverter.ToString(bArray).Replace("-", " ");
            }
            throw new InvalidDataException("Value cannot be converted to hex");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string s)
            {
                if (targetType == typeof(ulong))
                    return ulong.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(long))
                    return long.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(uint))
                    return uint.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(int))
                    return int.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(ushort))
                    return ushort.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(short))
                    return short.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(byte))
                    return byte.Parse(s.Replace("0x", "", StringComparison.InvariantCultureIgnoreCase), NumberStyles.HexNumber);
                if (targetType == typeof(byte[]))
                {
                    string[] hexValues = Regex.Split(s.Replace("0x",""), "[^0-9A-Fa-f]+");
                    List<byte> byteList = new List<byte>();
                    foreach (string hexValue in hexValues)
                    {
                        if (byte.TryParse(hexValue, out byte b))
                        {
                            byteList.Add(b);
                        }
                    }

                    return byteList.ToArray();
                }
            }

            throw new InvalidDataException("Value cannot be converted from hex");
        }
    }
}
