using System.ComponentModel;

namespace PropertyEditorLibrary
{
    public class DefaultPropertyAttributeProvider : IPropertyAttributeProvider
    {

        #region IPropertyAttributeProvider Members

        public PropertyAttributes GetAttributes(PropertyDescriptor descriptor)
        {
            var pa = new PropertyAttributes();
            var oa = PropertyHelper.GetAttribute<OptionalAttribute>(descriptor);
            if (oa != null)
                pa.OptionalProperty = oa.PropertyName;

            var fsa = PropertyHelper.GetAttribute<FormatStringAttribute>(descriptor);
            if (fsa != null)
                pa.FormatString = fsa.FormatString;

            var soa = PropertyHelper.GetAttribute<SortOrderAttribute>(descriptor);
            if (soa != null)
                pa.SortOrder = soa.SortOrder;

            var ha = PropertyHelper.GetAttribute<HeightAttribute>(descriptor);
            if (ha != null)
                pa.Height = ha.Height;

            var wa = PropertyHelper.GetAttribute<WidePropertyAttribute>(descriptor);
            if (wa != null)
            {
                pa.IsWide = true;
                pa.NoHeader = wa.NoHeader;
            }

            var sa = PropertyHelper.GetAttribute<SlidableAttribute>(descriptor);
            if (sa != null)
            {
                pa.IsSlidable = true;
                pa.SliderMaximum = sa.Maximum;
                pa.SliderMinimum = sa.Minimum;
                pa.SliderLargeChange = sa.LargeChange;
                pa.SliderSmallChange = sa.SmallChange;
            }
            return pa;
        }

        #endregion
    }
}