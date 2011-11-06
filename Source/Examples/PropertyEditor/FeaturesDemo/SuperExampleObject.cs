using System.ComponentModel;

namespace FeaturesDemo
{
    using PropertyTools.DataAnnotations;

    public class SuperExampleObject : ExampleObject
    {
        [Category("Subclass|Subclass properties"), SortIndex(999)]
        public string String2 { get; set; }

        public float Float2 { get; set; }
    }
}