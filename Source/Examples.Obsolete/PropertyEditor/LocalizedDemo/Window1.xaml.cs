// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Window1.xaml.cs" company="PropertyTools">
//   The MIT License (MIT)
//
//   Copyright (c) 2012 Oystein Bjorke
//
//   Permission is hereby granted, free of charge, to any person obtaining a
//   copy of this software and associated documentation files (the
//   "Software"), to deal in the Software without restriction, including
//   without limitation the rights to use, copy, modify, merge, publish,
//   distribute, sublicense, and/or sell copies of the Software, and to
//   permit persons to whom the Software is furnished to do so, subject to
//   the following conditions:
//
//   The above copyright notice and this permission notice shall be included
//   in all copies or substantial portions of the Software.
//
//   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
//   OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
//   MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
//   IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
//   CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
//   TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
//   SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary>
//   Interaction logic for Window1.xaml
// </summary>
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using PropertyTools.Wpf;

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