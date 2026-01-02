// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TranslatedExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for TranslatedExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyGridDemos
{
    using PropertyTools;

    using System.ComponentModel;

    /// <summary>
    /// Interaction logic for TranslatedExample.
    /// </summary>
    public partial class TranslatedExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TranslatedExample" /> class.
        /// </summary>
        public TranslatedExample()
        {
            this.InitializeComponent();
        }
    }

    public class TranslatedExampleViewModel : Observable
    {
        static object StaticInstance = new TranslatedExampleModel { Name = "John Doe", Age = 30 };

        public TranslatedExampleViewModel()
        {
            this.SelectedObject = StaticInstance;
        }

        private object selectedObject;

        public object SelectedObject
        {
            get => this.selectedObject;
            internal set => this.SetValue(ref this.selectedObject, value);
        }

    }

    public class TranslatedExampleModel : Observable
    {
        private string name;
        private int age;

        public string Name
        {
            get => this.name;
            set => this.SetValue(ref this.name, value);
        }

        public int Age
        {
            get => this.age;
            set => this.SetValue(ref this.age, value);
        }
    }
}