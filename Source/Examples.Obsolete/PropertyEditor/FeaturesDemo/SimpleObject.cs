using System.ComponentModel;
namespace FeaturesDemo
{
	public class SimpleObject 
	{
		public double Double { get; set; }
		public int Int { get; set; }
		public string String { get; set; }
		public bool Bool { get; set; }
		public Enum1 Enum { get; set; }
		public AllHidden1 AllHiddenEnum { get; set; }

		public enum Enum1 { 
			[Browsable(false)]
			NotShownValue, Value1, Value2,
			[Browsable( false )]
			[Description( "Description of AnotherNotShownValue" )]
			AnotherNotShownValue,
			[Description("Description of Value3")]
			Value3 }

		public enum AllHidden1
		{
			[Browsable( false )]
			NotShownValue,
			[Browsable( false )]
			AnotherNotShownValue
		}
	}
}