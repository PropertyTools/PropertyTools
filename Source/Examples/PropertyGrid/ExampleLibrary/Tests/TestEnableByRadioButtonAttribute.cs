namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class TestEnableByRadioButtonAttribute : TestBase
    {
        public enum Fruit { Apples, Bananas, Carrots }

        [Browsable(false)]
        public Fruit FruitSelector { get; set; }

        [Category("Fruits")]
        [EnableByRadioButton("FruitSelector", Fruit.Apples)]
        public int Apples { get; set; }

        [EnableByRadioButton("FruitSelector", Fruit.Bananas)]
        public int Bananas { get; set; }

        public enum Country { None, Argentina, Bolivia, Chile }

        [Browsable(false)]
        public Country CountrySelector { get; set; }

        [Category("Countries")]
        [EnableByRadioButton("CountrySelector", Country.None)]
        [Comment]
        [DisplayName("None")]
        public string NoneDummy { get { return string.Empty; } }

        [EnableByRadioButton("CountrySelector", Country.Argentina)]
        public string Argentina { get; set; }

        [EnableByRadioButton("CountrySelector", Country.Bolivia)]
        public string Bolivia { get; set; }

        [EnableByRadioButton("CountrySelector", Country.Chile)]
        public string Chile { get; set; }

        public override string ToString()
        {
            return "EnableByRadioButtonAttribute";
        }
    }
}