namespace PropertyTools.Wpf.Tests.PropertyEditor
{
    using System;
    using System.ComponentModel;
    using System.Linq;

    using NUnit.Framework;
    
    [TestFixture]
	public class EnumerationFiltering
	{
		public enum Enum1
		{
			[Browsable( false )]
			NotShownValue,
			Value1,
			Value2,
			[Browsable( false )]
			[System.ComponentModel.Description( "Description of AnotherNotShownValue" )]
			AnotherNotShownValue,
			[System.ComponentModel.Description( "Description of Value3" )]
			Value3
		}

		[Test]
		public void TestEnumFilter()
		{
			Assert.AreEqual( 5, Enum.GetValues( typeof( Enum1 )).Length );
			Assert.AreEqual( 3, Enum.GetValues( typeof(Enum1) ).FilterOnBrowsableAttribute().Count() );
		}

	}
}
