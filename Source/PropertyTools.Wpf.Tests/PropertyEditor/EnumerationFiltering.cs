using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.ComponentModel;

namespace PropertyTools.Wpf.Tests.PropertyEditor
{
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
