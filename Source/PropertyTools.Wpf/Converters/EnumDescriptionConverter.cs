using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Data;
using System.Collections;

namespace PropertyTools.Wpf
{
	/// <summary>
	/// The EnumDescriptionConverter gets the Description attribute for Enum values.
	/// </summary>
	[ValueConversion(typeof(object), typeof(string))]
	public class EnumDescriptionConverter : IValueConverter
	{
		#region IValueConverter Members

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			// Default, non-converted result.
			string result = value.ToString();

			var field = value.GetType().GetFields(BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public).FirstOrDefault(f => f.GetValue(null).Equals(value));

			if (field != null)
			{
				var descriptionAttribute = field.GetCustomAttributes<DescriptionAttribute>(true).FirstOrDefault();
				if (descriptionAttribute != null)
				{
					// Found the attribute, assign description
					result = descriptionAttribute.Description;
				}
			}

			return result;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotSupportedException();
		}

		#endregion
	}

	public static class ReflectionExtensions
	{
		public static IEnumerable<T> GetCustomAttributes<T>(this FieldInfo fieldInfo, bool inherit)
		{
			return fieldInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>();
		}

		/// <summary>
		/// Filters on the browsable attribute.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arr">The arr.</param>
		/// <returns></returns>
		public static List<object> FilterOnBrowsableAttribute<T>( this T arr ) where T : IEnumerable
		{
			// Default empty list
			List<object> res = new List<object>();

			// Loop each item in the enumerable
			foreach( var o in arr ) {
				// Get the field information for the current field
				FieldInfo field = o.GetType().GetField( o.ToString(), BindingFlags.Static | BindingFlags.GetField | BindingFlags.Public );
				if( field != null ) {
					// Get the Browsable attribute, if it is declared for this field
					var browsable = field.GetCustomAttributes<BrowsableAttribute>( true ).FirstOrDefault();
					if( browsable != null ) {
						// It is declared, is it true or false?
						if( browsable.Browsable ) {
							// Is true - field is not hidden so add it to the result.
							res.Add( o );
						}
					}
					else {
						// Not declared so add it to the result
						res.Add( o );
					}
				}
				else {
					// Can't evaluate, include it.
					res.Add( o );
				}
			}

			return res;
		}
	}
}