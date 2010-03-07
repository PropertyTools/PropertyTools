using System.ComponentModel;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyAttributeProvider : IPropertyAttributeProvider
    {

        #region IPropertyAttributeProvider Members

        public string GetOptionalProperty(PropertyDescriptor descriptor)
        {
            var oa = PropertyHelper.GetAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
                return oa.PropertyName;
            return null;
        }
        public string GetFormatString(PropertyDescriptor descriptor)
        {
            var fsa = PropertyHelper.GetAttribute<FormatStringAttribute>(descriptor);
            if (fsa != null)
                return fsa.FormatString;
            return null;
        }


        public int GetSortOrder(PropertyDescriptor descriptor)
        {
            var soa = PropertyHelper.GetAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                return soa.SortOrder;
            return 0;
        }
        public double GetHeight(PropertyDescriptor descriptor)
        {
            var ha = PropertyHelper.GetAttribute<HeightAttribute>(descriptor);
            if (ha != null)
                return ha.Height;

            return double.NaN;
        }
        public bool IsWide(PropertyDescriptor descriptor, out bool noHeader)
        {
            var wa = PropertyHelper.GetAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
            {
                noHeader = wa.NoHeader;
                return true;
            }
            noHeader = false;
            return false;
        }

        public bool IsSlidable(PropertyDescriptor descriptor, out double minimum, out double maximum, out double largeChange, out double smallChange)
        {
            var sa = PropertyHelper.GetAttribute<SlidableAttribute>(descriptor);
            if (sa != null)
            {
                maximum = sa.Maximum;
                minimum = sa.Minimum;
                largeChange = sa.LargeChange;
                smallChange = sa.SmallChange;
                return true;
            }
            maximum = double.NaN;
            minimum = double.NaN;
            largeChange = double.NaN;
            smallChange = double.NaN;
            return false;
        }

        #endregion
    }
}