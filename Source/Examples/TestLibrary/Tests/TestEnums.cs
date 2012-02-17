namespace TestLibrary
{
    using System;
    using System.ComponentModel;

    public enum Fruits1 { Apple, Pears, Bananas }
    
    public enum Fruits2 { }
    
    public enum Fruits3 { [Browsable(false)] Apple, Pears, Bananas }
    
    public enum Fruits4 { [Browsable(false)] Apple, [Browsable(false)] Pears, [Browsable(false)] Bananas }
    
    public enum Fruits5 { [Description("Epler")] Apple, [Description("Pærer")] Pears, [Description("Bananer")] Bananas }

    [Flags]
    public enum Fruits6 { Apple=1, Pears=2, Bananas=4 }

    public class TestEnums : TestBase
    {
        [Description("Normal enum.")]
        public Fruits1 Fruits1 { get; set; }
        
        [Description("Empty enum.")]
        public Fruits2 Fruits2 { get; set; }
        
        [Description("First item is not browsable.")]
        public Fruits3 Fruits3 { get; set; }
        
        [Description("No items are browsable.")]
        public Fruits4 Fruits4 { get; set; }

        [Description("With descriptions.")]
        public Fruits5 Fruits5 { get; set; }

        [Description("Flags.")]
        public Fruits6 Fruits6 { get; set; }

        public override string ToString()
        {
            return "Enumerations";
        }
    }
}