using System;
using System.Windows.Markup;

namespace PropertyTools.Wpf
{
    [Obsolete]
    public abstract class SelfProvider : MarkupExtension
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}