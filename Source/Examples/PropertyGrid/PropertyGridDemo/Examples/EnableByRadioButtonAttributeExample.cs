namespace ExampleLibrary
{
    using PropertyTools.DataAnnotations;

    [PropertyGridExample]
    public class EnableByRadioButtonAttributeExample : Example
    {
        private Fruit fruitSelector;
        private int apples;
        private int bananas;
        private Country countrySelector;
        private string argentina;
        private string bolivia;
        private string chile;

        public enum Fruit { Apples, Bananas, Carrots }

        [Browsable(false)]
        public Fruit FruitSelector { get => this.fruitSelector; set { this.fruitSelector = value; this.RaisePropertyChanged(nameof(FruitSelector)); } }

        [Category("Fruits")]
        [EnableByRadioButton("FruitSelector", Fruit.Apples)]
        public int Apples { get => this.apples; set { this.apples = value; this.RaisePropertyChanged(nameof(Apples)); } }

        [EnableByRadioButton("FruitSelector", Fruit.Bananas)]
        public int Bananas { get => this.bananas; set { this.bananas = value; this.RaisePropertyChanged(nameof(Bananas)); } }

        public enum Country { None, Argentina, Bolivia, Chile }

        [Browsable(false)]
        public Country CountrySelector { get => this.countrySelector; set { this.countrySelector = value; this.RaisePropertyChanged(nameof(CountrySelector)); } }

        [Category("Countries")]
        [EnableByRadioButton("CountrySelector", Country.None)]
        [Comment]
        [DisplayName("None")]
        public string NoneDummy => string.Empty;

        [EnableByRadioButton("CountrySelector", Country.Argentina)]
        public string Argentina { get => this.argentina; set { this.argentina = value; this.RaisePropertyChanged(nameof(Argentina)); } }

        [EnableByRadioButton("CountrySelector", Country.Bolivia)]
        public string Bolivia { get => this.bolivia; set { this.bolivia = value; this.RaisePropertyChanged(nameof(Bolivia)); } }

        [EnableByRadioButton("CountrySelector", Country.Chile)]
        public string Chile { get => this.chile; set { this.chile = value; this.RaisePropertyChanged(nameof(Chile)); } }
    }
}