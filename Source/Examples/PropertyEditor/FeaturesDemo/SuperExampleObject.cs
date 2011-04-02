using System.ComponentModel;
using PropertyTools.Wpf;

namespace FeaturesDemo
{
    public class SuperExampleObject : ExampleObject
    {
        [Category("Subclass|Subclass properties"), SortOrder(999)]
        public string String2 { get; set; }

        public float Float2 { get; set; }
    }
}