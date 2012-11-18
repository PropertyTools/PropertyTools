namespace TestLibrary
{
    using System.Collections.Generic;
    using System.ComponentModel;

    using PropertyTools.DataAnnotations;

    public class TestItemsSourcePropertyAttribute : TestBase
    {
        [Browsable(false)]
        public List<string> Items { get; set; }

        [ItemsSourceProperty("Items")]
        public string SelectedItem { get; set; }

        [ItemsSourceProperty("Items")]
        [IsEditable]
        public string EditableItem { get; set; }

        public TestItemsSourcePropertyAttribute()
        {
            Items = new List<string> { "Oslo", "Stockholm", "København" };
        }

        public override string ToString()
        {
            return "ItemsSourceProperty";
        }
    }
}