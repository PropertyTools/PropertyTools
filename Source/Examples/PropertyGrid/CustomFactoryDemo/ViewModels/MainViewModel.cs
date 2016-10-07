// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MainViewModel.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace CustomFactoryDemo
{
    public class MainViewModel
    {
        public TestObject TestObject { get; set; }

        public CustomPropertyGridControlFactory CustomPropertyGridControlFactory { get; set; }

        public CustomPropertyGridOperator CustomPropertyGridOperator { get; set; }

        public MainViewModel()
        {
            this.CustomPropertyGridControlFactory = new CustomPropertyGridControlFactory();
            this.CustomPropertyGridOperator = new CustomPropertyGridOperator();
            this.TestObject = new TestObject { Title = "Title", Range1 = new Range { Minimum = 0, Maximum = 100 } };
        }
    }
}