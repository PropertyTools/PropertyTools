namespace ExampleLibrary
{
    [PropertyGridExample]
    public class TestEditableAttribute : TestBase
    {
        [System.ComponentModel.DataAnnotations.Editable(true)]
        public string Editable { get; set; }

        [System.ComponentModel.DataAnnotations.Editable(false)]
        public string NotEditable { get; set; }

        public override string ToString()
        {
            return "EditableAttribute";
        }
    }
}