// Test Case for TreeListBox TabControl Crash Fix
// This file demonstrates the issue and can be used to verify the fix

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using PropertyTools.Wpf;

namespace TreeListBoxTabControlTest
{
    // Test Node class
    public class TestNode : INotifyPropertyChanged
    {
        private bool _isExpanded;
        private bool _isSelected;
        private ObservableCollection<TestNode> _children;

        public string Name { get; set; }
        
        public ObservableCollection<TestNode> Children
        {
            get => _children ?? (_children = new ObservableCollection<TestNode>());
            set
            {
                _children = value;
                OnPropertyChanged(nameof(Children));
            }
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged(nameof(IsExpanded));
            }
        }

        public bool IsSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged(nameof(IsSelected));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString() => Name;
    }

    // Test ViewModel
    public class TestViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<TestNode> _hierarchySource;

        public ObservableCollection<TestNode> HierarchySource
        {
            get => _hierarchySource;
            set
            {
                _hierarchySource = value;
                OnPropertyChanged(nameof(HierarchySource));
            }
        }

        public TestViewModel()
        {
            // Populate BEFORE window is shown (common scenario)
            HierarchySource = new ObservableCollection<TestNode>
            {
                new TestNode 
                { 
                    Name = "Root 1",
                    Children = new ObservableCollection<TestNode>
                    {
                        new TestNode { Name = "Child 1.1" },
                        new TestNode { Name = "Child 1.2" }
                    }
                },
                new TestNode 
                { 
                    Name = "Root 2",
                    Children = new ObservableCollection<TestNode>
                    {
                        new TestNode { Name = "Child 2.1" },
                        new TestNode { Name = "Child 2.2" }
                    }
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    /// <summary>
    /// Test cases for TreeListBox TabControl crash
    /// </summary>
    public static class TreeListBoxTabControlTests
    {
        /// <summary>
        /// Test 1: Basic TabControl scenario (most common crash)
        /// </summary>
        public static Window CreateBasicTabControlTest()
        {
            var window = new Window
            {
                Title = "TreeListBox TabControl Test - Basic",
                Width = 600,
                Height = 400
            };

            var tabControl = new TabControl();

            // Tab 1: Some other content
            var tab1 = new TabItem { Header = "First Tab" };
            tab1.Content = new TextBlock 
            { 
                Text = "Click the 'TreeListBox Tab' to trigger the bug",
                Margin = new Thickness(10)
            };
            tabControl.Items.Add(tab1);

            // Tab 2: TreeListBox (THIS WILL CRASH on tab switch)
            var tab2 = new TabItem { Header = "TreeListBox Tab" };
            var treeListBox = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            
            // Bind to pre-populated data
            var viewModel = new TestViewModel();
            treeListBox.DataContext = viewModel;
            treeListBox.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            
            tab2.Content = treeListBox;
            tabControl.Items.Add(tab2);

            window.Content = tabControl;
            return window;
        }

        /// <summary>
        /// Test 2: Rapid tab switching
        /// </summary>
        public static Window CreateRapidSwitchingTest()
        {
            var window = new Window
            {
                Title = "TreeListBox TabControl Test - Rapid Switching",
                Width = 600,
                Height = 400
            };

            var stack = new StackPanel();
            
            var tabControl = new TabControl();
            
            var tab1 = new TabItem { Header = "Tab 1" };
            tab1.Content = new TextBlock { Text = "Tab 1 Content", Margin = new Thickness(10) };
            tabControl.Items.Add(tab1);

            var tab2 = new TabItem { Header = "TreeListBox Tab" };
            var treeListBox = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            var viewModel = new TestViewModel();
            treeListBox.DataContext = viewModel;
            treeListBox.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            tab2.Content = treeListBox;
            tabControl.Items.Add(tab2);

            var button = new Button
            {
                Content = "Rapid Switch Test (100 times)",
                Margin = new Thickness(10),
                Padding = new Thickness(5)
            };

            button.Click += async (s, e) =>
            {
                button.IsEnabled = false;
                for (int i = 0; i < 100; i++)
                {
                    tabControl.SelectedIndex = 0;
                    await Task.Delay(10);
                    tabControl.SelectedIndex = 1;
                    await Task.Delay(10);
                }
                button.IsEnabled = true;
                MessageBox.Show("Test completed successfully!", "Success");
            };

            stack.Children.Add(button);
            stack.Children.Add(tabControl);
            
            window.Content = stack;
            return window;
        }

        /// <summary>
        /// Test 3: Dynamic collection updates during tab switch
        /// </summary>
        public static Window CreateDynamicCollectionTest()
        {
            var window = new Window
            {
                Title = "TreeListBox TabControl Test - Dynamic Collection",
                Width = 600,
                Height = 400
            };

            var stack = new StackPanel();
            var tabControl = new TabControl();
            var viewModel = new TestViewModel();

            var tab1 = new TabItem { Header = "Tab 1" };
            var buttonStack = new StackPanel { Margin = new Thickness(10) };
            
            var addButton = new Button 
            { 
                Content = "Add 100 Items While on This Tab",
                Margin = new Thickness(5),
                Padding = new Thickness(5)
            };
            addButton.Click += (s, e) =>
            {
                for (int i = 0; i < 100; i++)
                {
                    viewModel.HierarchySource.Add(new TestNode 
                    { 
                        Name = $"Dynamic Item {i}",
                        Children = new ObservableCollection<TestNode>
                        {
                            new TestNode { Name = $"Child {i}.1" }
                        }
                    });
                }
                MessageBox.Show("Added 100 items. Now switch to TreeListBox tab.", "Info");
            };
            
            buttonStack.Children.Add(addButton);
            buttonStack.Children.Add(new TextBlock 
            { 
                Text = "1. Click 'Add Items'\n2. Switch to TreeListBox tab\n3. Bug may occur during switch",
                Margin = new Thickness(5)
            });
            
            tab1.Content = buttonStack;
            tabControl.Items.Add(tab1);

            var tab2 = new TabItem { Header = "TreeListBox Tab" };
            var treeListBox = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            treeListBox.DataContext = viewModel;
            treeListBox.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            tab2.Content = treeListBox;
            tabControl.Items.Add(tab2);

            stack.Children.Add(tabControl);
            window.Content = stack;
            return window;
        }

        /// <summary>
        /// Test 4: TreeListBox starts in non-visible tab (deferred loading)
        /// </summary>
        public static Window CreateDeferredLoadingTest()
        {
            var window = new Window
            {
                Title = "TreeListBox TabControl Test - Deferred Loading",
                Width = 600,
                Height = 400
            };

            var tabControl = new TabControl();
            
            // Start with tab 1 selected
            tabControl.SelectedIndex = 0;

            var tab1 = new TabItem { Header = "First Tab (Default)" };
            tab1.Content = new TextBlock 
            { 
                Text = "TreeListBox is in Tab 2 but not loaded yet.\nSwitch to Tab 2 to trigger loading.",
                Margin = new Thickness(10)
            };
            tabControl.Items.Add(tab1);

            // TreeListBox in tab 2 - will be loaded when tab is selected
            var tab2 = new TabItem { Header = "TreeListBox Tab (Deferred)" };
            var treeListBox = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            
            // ViewModel with substantial data
            var viewModel = new TestViewModel();
            for (int i = 0; i < 50; i++)
            {
                viewModel.HierarchySource.Add(new TestNode 
                { 
                    Name = $"Node {i}",
                    Children = new ObservableCollection<TestNode>
                    {
                        new TestNode { Name = $"Child {i}.1" },
                        new TestNode { Name = $"Child {i}.2" },
                        new TestNode { Name = $"Child {i}.3" }
                    }
                });
            }
            
            treeListBox.DataContext = viewModel;
            treeListBox.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            
            tab2.Content = treeListBox;
            tabControl.Items.Add(tab2);

            window.Content = tabControl;
            return window;
        }

        /// <summary>
        /// Test 5: Multiple TreeListBox instances in different tabs
        /// </summary>
        public static Window CreateMultipleInstancesTest()
        {
            var window = new Window
            {
                Title = "TreeListBox TabControl Test - Multiple Instances",
                Width = 600,
                Height = 400
            };

            var tabControl = new TabControl();
            var viewModel = new TestViewModel();

            // Tab 1: First TreeListBox
            var tab1 = new TabItem { Header = "TreeListBox 1" };
            var tree1 = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            tree1.DataContext = viewModel;
            tree1.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            tab1.Content = tree1;
            tabControl.Items.Add(tab1);

            // Tab 2: Second TreeListBox (same data)
            var tab2 = new TabItem { Header = "TreeListBox 2" };
            var tree2 = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            tree2.DataContext = viewModel;
            tree2.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            tab2.Content = tree2;
            tabControl.Items.Add(tab2);

            // Tab 3: Third TreeListBox (same data)
            var tab3 = new TabItem { Header = "TreeListBox 3" };
            var tree3 = new TreeListBox
            {
                ChildrenPath = "Children",
                IsExpandedPath = "IsExpanded",
                IsSelectedPath = "IsSelected"
            };
            tree3.DataContext = viewModel;
            tree3.SetBinding(TreeListBox.HierarchySourceProperty, 
                new System.Windows.Data.Binding("HierarchySource"));
            tab3.Content = tree3;
            tabControl.Items.Add(tab3);

            window.Content = tabControl;
            return window;
        }
    }
}

// XAML Alternative (for reference)
/*
<Window x:Class="TreeListBoxTabControlTest.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:pt="http://propertytools.org/wpf"
        Title="TreeListBox TabControl Crash Test" Height="400" Width="600">
    <TabControl>
        <TabItem Header="First Tab">
            <TextBlock Text="Click 'TreeListBox Tab' to trigger the bug" Margin="10"/>
        </TabItem>
        <TabItem Header="TreeListBox Tab">
            <pt:TreeListBox HierarchySource="{Binding HierarchySource}" 
                           ChildrenPath="Children"
                           IsExpandedPath="IsExpanded"
                           IsSelectedPath="IsSelected"/>
        </TabItem>
    </TabControl>
</Window>
*/

// Main entry point for testing
/*
public class Program
{
    [STAThread]
    public static void Main()
    {
        var app = new Application();
        
        // Choose which test to run:
        var testWindow = TreeListBoxTabControlTests.CreateBasicTabControlTest();
        // var testWindow = TreeListBoxTabControlTests.CreateRapidSwitchingTest();
        // var testWindow = TreeListBoxTabControlTests.CreateDynamicCollectionTest();
        // var testWindow = TreeListBoxTabControlTests.CreateDeferredLoadingTest();
        // var testWindow = TreeListBoxTabControlTests.CreateMultipleInstancesTest();
        
        app.Run(testWindow);
    }
}
*/
