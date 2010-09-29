﻿using System.ComponentModel;
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