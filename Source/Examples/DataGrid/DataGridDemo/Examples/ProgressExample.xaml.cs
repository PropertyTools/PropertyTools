// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemplateExample.xaml.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Interaction logic for ProgressExample.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

using PropertyTools;
using PropertyTools.DataAnnotations;
using PropertyTools.Wpf;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace DataGridDemo
{
    /// <summary>
    /// Interaction logic for ProgressExample.
    /// </summary>
    public partial class ProgressExample
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressExample" /> class.
        /// </summary>
        public ProgressExample()
        {
            this.InitializeComponent();
        }
    }

    public class ProgressExampleViewModel
    {
        private DispatcherTimer timer;

        public ICommand StartCommand { get; }
        public ICommand StopCommand { get; }
        public ICommand AddCommand { get; }

        public ObservableCollection<ProgressItem> Items { get; set; } = new ObservableCollection<ProgressItem> {
            new ProgressItem() { Progress = 0} ,
            new ProgressItem() { Progress = 0.1} ,
            new ProgressItem() { Progress = 0.5} ,
            new ProgressItem() { Progress = 0.9} ,
            new ProgressItem() { Progress = 1 } ,
        };

        public MyProgressControlFactory ControlFactory { get; } = new MyProgressControlFactory();
        public MyProgressCellDefinitionFactory CellDefinitionFactory { get; } = new MyProgressCellDefinitionFactory();

        public ProgressExampleViewModel()
        {
            this.StartCommand = new DelegateCommand(this.Start, () => this.timer == null);
            this.StopCommand = new DelegateCommand(this.Stop, () => this.timer != null);
            this.AddCommand = new DelegateCommand(() => this.Add(10));
        }

        public void Add(int n)
        {
            var r = new Random();
            for (int i = 0; i < n; i++)
            {
                this.Items.Add(new ProgressItem() { Progress = r.NextDouble() });
            }
        }

        public void Start()
        {
            if (this.timer != null)
            {
                return;
            }

            this.timer = new System.Windows.Threading.DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(0.3);
            var r = new Random();
            timer.Tick += (s, e) =>
            {
                foreach (var item in this.Items)
                {
                    item.Progress = Math.Min(1, item.Progress + r.NextDouble() * 0.02);
                }
            };

            timer.Start();
        }

        public void Stop()
        {
            if (this.timer == null)
            {
                return;
            }

            this.timer.Stop();
            this.timer = null;
        }

        public class MyProgressCellDefinitionFactory : CellDefinitionFactory
        {
            public MyProgressCellDefinitionFactory()
            {
            }

            protected override CellDefinition CreateCellDefinitionOverride(CellDescriptor d)
            {
                if (d.Attributes.OfType<MyProgressAttribute>().Any())
                {
                    return new MyProgressCellDefinition();
                }

                return base.CreateCellDefinitionOverride(d);
            }
        }

        public class MyProgressControlFactory : DataGridControlFactory
        {
            protected override FrameworkElement CreateDisplayControlOverride(CellDefinition d)
            {
                if (d is MyProgressCellDefinition)
                {
                    return CreateProgressControl(d);
                }

                return base.CreateDisplayControlOverride(d);
            }

            protected virtual FrameworkElement CreateProgressControl(CellDefinition d)
            {
                var c = new ProgressBar
                {
                    Minimum = 0,
                    Maximum = 1,
                    Margin = new Thickness(4),
                    Foreground = System.Windows.Media.Brushes.OrangeRed
                };
                c.SetBinding(ProgressBar.ValueProperty, this.CreateBinding(d));
                return c;
            }
        }

        public class MyProgressCellDefinition : CellDefinition
        {
        }

        [AttributeUsage(AttributeTargets.Property)]
        public class MyProgressAttribute : Attribute
        {
        }

        public class ProgressItem : Observable
        {
            private double progress;
            private string status;

            [Progress] // PropertyTools data annotation attribute
            public double Progress
            {
                get => this.progress;
                internal set
                {
                    if (this.SetValue(ref this.progress, value))
                    {
                        this.RaisePropertyChanged(nameof(ProgressValue));
                        this.RaisePropertyChanged(nameof(MyProgress));
                        this.Status = GetStatusText(this.progress);
                    }
                }
            }

            [MyProgress] // custom attribute
            public double MyProgress
            {
                get => this.Progress;
                set => this.Progress = value;
            }

            public double ProgressValue { get => this.progress; set => this.Progress = value; }

            public string Status
            {
                get => this.status;
                set => this.SetValue(ref this.status, value);
            }

            private string GetStatusText(double progress)
            {
                if (progress >= 1) return "Done";
                if (progress >= 0.9) return "Almost done";
                if (progress >= 0.1) return "In progress";
                if (progress > 0) return "Just started";
                return "Not started";
            }
        }
    }
}