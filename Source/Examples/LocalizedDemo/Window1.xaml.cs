using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PropertyEditorLibrary;

namespace LocalizedDemo
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }
    }

    public class MainViewModel
    {
        public Person CurrentPerson { get; set; }

        public ILocalizationService MyLocalizationService { get; set; }

        public MainViewModel()
        {
            CurrentPerson = new Person { Name = "Joe", Address = "Hong Kong", Occupation = "Student" };
            MyLocalizationService = new MyLocalizationService { CurrentLanguage = Languages.UbbiDubbi };
        }
    }
    public class Person
    {
        [Category("Personal|Information")]
        public string Name { get; set; }
        public string Address { get; set; }
        public string Occupation { get; set; }
    }

    public enum Languages
    {
        RoevarSpraaket,
        PigLatin,
        UbbiDubbi
    } ;

    // The localization is provided by the ILocalizationService
    public class MyLocalizationService : ILocalizationService
    {
        public Languages CurrentLanguage { get; set; }

        #region ILocalizationService Members

        public string GetString(Type instanceType, string key)
        {
            switch (CurrentLanguage)
            {
                case Languages.RoevarSpraaket:
                    return Translate(key);
                case Languages.PigLatin:
                    return TranslateToPigLatin(key);
                case Languages.UbbiDubbi:
                    return TranslateToUbbiDubbi(key);
                default:
                    return key;
            }
        }

        public object GetTooltip(Type instanceType, string key)
        {
            var tooltip = String.Format("More information about {0}.", key);

            // could return the string, or create a UIElement:           
            return new TextBlock
            {
                Text = GetString(instanceType, tooltip),
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(4)
            };
        }

        #endregion

        /// <summary>
        /// Translate a string to rövarspråket
        /// http://en.wikipedia.org/wiki/R%C3%B6varspr%C3%A5ket
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string Translate(string s)
        {
            foreach (char c in "bdfghjklmnpqrstvwxz")
            {
                s = s.Replace(c.ToString(), c + "o" + c);
            }
            return s;
        }

        /// <summary>
        /// Translate a string to pig latin
        /// http://en.wikipedia.org/wiki/Pig_Latin
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string TranslateToPigLatin(string s)
        {
            return s.Substring(1, s.Length - 1) + s.Substring(0, 1) + "ay";
        }

        /// <summary>
        /// http://en.wikipedia.org/wiki/Ubbi_Dubbi
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public string TranslateToUbbiDubbi(string s)
        {
            foreach (char vowel in "aeiou")
            {
                s = s.Replace(vowel.ToString(), "\tb" + vowel);
            }
            s = s.Replace('\t', 'u');
            return s;
        }
    }

}
