namespace FeaturesDemo
{
    public class SimpleObject 
    {
        public double Double { get; set; }
        public int Int { get; set; }
        public string String { get; set; }
        public bool Bool { get; set; }
        public Enum1 Enum { get; set; }

        public enum Enum1 { Value1, Value2, Value3 }
    }
}