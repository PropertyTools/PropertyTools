using System;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Media;
using PropertyEditorLibrary;

namespace SimpleDemo
{
    #region Enums
    public enum Genders
    {
        Male,
        Female
    } ;

    public enum YesOrNo
    {
        No,
        Yes
    }
    #endregion

    #region Mass
    [TypeConverter(typeof(MassConverter))]
    public class Mass
    {
        public double Value { get; set; }

        public static Mass Parse(string s)
        {
            s = s.Replace(',', '.').Trim();
            var r = new Regex(@"[-+]?[0-9]*\.?[0-9]+([eE][-+]?[0-9]+)?");
            Match m = r.Match(s);
            if (!m.Success) return null;
            double value = double.Parse(m.Groups[0].Value, CultureInfo.InvariantCulture);
            // string unit = m.Groups[1].Value;
            return new Mass { Value = value };
        }

        public override string ToString()
        {
            return String.Format("{0:N0} kg", Value, CultureInfo.InvariantCulture);
        }
    }

    public class MassConverter : TypeConverter
    {
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return Mass.Parse((string)value);
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
    #endregion

    public class Person : Observable
    {
        // When the [Category(...)] attribute is used with a "|", the
        // first part is the header of the tab and the second part is
        // the header of the category
        // The order of the properties are not guaranteed to be fixed
        private const string catPersonal = "General|Personal";
        private const string catFavourites = "General|Favourites";
        private const string catHabits = "General|Habits";
        private const string catVehicles = "General|Vehicles";
        private const string catDetails = "More|Details";

        [Category(catPersonal), SortOrder(100)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public Mass Weight { get; set; }
        public Genders Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public Color HairColor { get; set; }

        [Category(catFavourites), SortOrder(200)]
        public Brush FavouriteBrush { get; set; }
        [Category(catFavourites)]
        public FontFamily Font { get; set; }

        [Category(catHabits), SortOrder(300)]
        public bool IsSmoking { get; set; }

        [Category(catVehicles), DisplayName("Owns bicyle"),
        Description("Check if this person owns a bicycle."), SortOrder(401)]
        public bool OwnsBicycle { get; set; }

        // This property is used to control the optional Car property
        // The property should be public, but not browsable
        [Browsable(false)]
        public bool HasCar { get; set; }

        [Category(catVehicles), Optional("HasCar"), SortOrder(402)]
        public string Car { get; set; }

        // Use [WideProperty] to use the full width of the control
        // Use [Height(...)] to set the height of a multiline text control
        [Category(catDetails), WideProperty, Height(100), SortOrder(500)]
        public string History { get; set; }

        public Person()
        {
            Font = new FontFamily("Arial");
        }
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0} {1}, {2}, {3}, {4}, {5}\n", FirstName, LastName, Age, Weight, Gender, BirthDate);
            sb.AppendFormat("{0} {1}\n", HairColor, FavouriteBrush);
            sb.AppendFormat("IsSmoking={0}\n", IsSmoking);
            sb.AppendFormat("OwnsBicycle={0} HasCar={1} Car={2}\n", OwnsBicycle, HasCar, Car);
            sb.AppendFormat("History: {0}", History);
            return sb.ToString();
        }
    }

    public class Observable : INotifyPropertyChanged
    {
        internal virtual void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }

}