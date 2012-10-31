// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OptionsViewModel.xaml.cs" company="PropertyTools">
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
// --------------------------------------------------------------------------------------------------------------------
using System.ComponentModel;
using DialogDemos.Properties;

namespace DialogDemos
{
    public class OptionsViewModel : Observable
    {
        private Settings settings
        {
            get { return Settings.Default; }
        }

        [Category("Environment|General")]
        public string ProjectPath
        {
            get { return settings.ProjectPath; }
            set
            {
                settings.ProjectPath = value;
                OnPropertyChanged("ReportPath");
            }
        }

        [Category("Environment|General")]
        public int UndoLevels
        {
            get { return settings.UndoLevels; }
            set
            {
                settings.UndoLevels = value;
                OnPropertyChanged("UndoLevels");
            }
        }

        [Category("Environment|Menus")]
        public int WindowMenuItems
        {
            get { return settings.WindowMenuItems; }
            set
            {
                settings.WindowMenuItems = value;
                OnPropertyChanged("WindowMenuItems");
            }
        }

        [Category("Environment|Menus")]
        public int MRUItems
        {
            get { return settings.MRUItems; }
            set
            {
                settings.MRUItems = value;
                OnPropertyChanged("MRUItems");
            }
        }

        [Category("Startup|Actions")]
        public bool ShowStartPage
        {
            get { return settings.ShowStartPage; }
            set
            {
                settings.ShowStartPage = value;
                OnPropertyChanged("NewsChannel");
            }
        }


        [Category("Startup|News channel")]
        public string NewsChannel
        {
            get { return settings.NewsChannel; }
            set
            {
                settings.NewsChannel = value;
                OnPropertyChanged("NewsChannel");
            }
        }


        [Category("Startup|Actions")]
        public StartupAction StartupAction
        {
            get { return settings.StartupAction; }
            set
            {
                settings.StartupAction = value;
                OnPropertyChanged("StartupAction");
            }
        }

        public void Save()
        {
            settings.Save();
        }
    }
}