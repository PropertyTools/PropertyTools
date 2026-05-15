using ExampleLibrary;
using PropertyTools.DataAnnotations;
using System;

namespace PropertyGridDemo.Examples
{
    [PropertyGridExample]
    public class EnumsWithFlagsExample : Example
    {
        [Flags]
        public enum Permissions
        {
            None = 0,      // 0000
            Read = 1 << 0, // 0001 (1)
            Write = 1 << 1, // 0010 (2)
            Execute = 1 << 2, // 0100 (4)
            Delete = 1 << 3  // 1000 (8)
        }

        [Flags]
        [System.ComponentModel.DefaultValue(2)]      
        public enum PermissionsWithoutZero1
        {            
            Read = 1 << 0, // 0001 (1)            
            Write = 1 << 1, // 0010 (2)
            Execute = 1 << 2, // 0100 (4)
            Delete = 1 << 3  // 1000 (8)
        }

        [Flags]
        [System.ComponentModel.DefaultValue(2)]
        public enum PermissionsWithoutZero2
        {
            Read = 1 << 0, // 0001 (1)
            [Description("Write (default, is set when all other have been unset)")]
            Write = 1 << 1, // 0010 (2)
            Execute = 1 << 2, // 0100 (4)
            Delete = 1 << 3  // 1000 (8)
        }

        private Permissions flags1;
        private Permissions flags2;
        private Permissions? flags3;
        private Permissions? flags4;

        private PermissionsWithoutZero1 flags5;
        private PermissionsWithoutZero2 flags6;

        private PermissionsWithoutZero1? flags7;

        public EnumsWithFlagsExample()
        {
            flags5 = 0; // but default is 2
            flags6 = 0; // but default is 2
        }

        
        // CheckBoxList
        [Category("CheckBoxList|")]
        [Description("All flags")]
        public Permissions Flags1C
        {
            get => this.flags1;
            set
            {
                this.flags1 = value;
                this.RaisePropertyChanged(nameof(Flags1C));
                this.RaisePropertyChanged(nameof(Flags1L));
            }
        }



        [Category("CheckBoxList|")]
        [Description("Without 'None' ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Permissions.None)]
        public Permissions Flags2C
        {
            get => this.flags2;
            set
            {
                this.flags2 = value;
                this.RaisePropertyChanged(nameof(Flags2C));
                this.RaisePropertyChanged(nameof(Flags2L));
            }
        }


        [Category("CheckBoxList|")]
        [DisplayName(nameof(Flags3C) + " (Nullable)")]
        [Description("Without 'None' (Nullable)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Permissions.None)]
        public Permissions? Flags3C
        {
            get => this.flags3;
            set
            {
                this.flags3 = value;
                this.RaisePropertyChanged(nameof(Flags3C));
                this.RaisePropertyChanged(nameof(Flags3L));
            }
        }

        [Category("CheckBoxList|")]
        [DisplayName(nameof(Flags4C) + " (Nullable)")]
        [Description("only 'Read' (Nullable)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Permissions.Read)]
        public Permissions? Flags4C
        {
            get => this.flags4;
            set
            {
                this.flags4 = value;
                this.RaisePropertyChanged(nameof(Flags4C));
                this.RaisePropertyChanged(nameof(Flags4L));
            }
        }
        

        [Category("CheckBoxList|")]
        [Description("All flags. 'Write' is default. Set by System.ComponentModel.DefaultValue(2)")]
        public PermissionsWithoutZero1 Flags5C
        {
            get => this.flags5;
            set
            {
                this.flags5 = value;
                this.RaisePropertyChanged(nameof(Flags5C));
                this.RaisePropertyChanged(nameof(Flags5L));
            }
        }

        [Category("CheckBoxList|")]        
        [Description("Initialized by default. 'Write' is default. Set by System.ComponentModel.DefaultValue(2)")]
        [EnumMissingZeroBehaviorAttribute(initializeWithDefault: true, resetToDefault: true)]
        public PermissionsWithoutZero2 Flags6C
        {
            get => this.flags6;
            set
            {
                this.flags6 = value;
                this.RaisePropertyChanged(nameof(Flags6C));
                this.RaisePropertyChanged(nameof(Flags6L));
            }
        }


        [Category("CheckBoxList|")]
        [DisplayName(nameof(Flags7C) + " (Nullable)")]
        [Description("All flags (Nullable)")]
        public PermissionsWithoutZero1? Flags7C
        {
            get => this.flags7;
            set
            {
                this.flags7 = value;
                this.RaisePropertyChanged(nameof(Flags7C));
                this.RaisePropertyChanged(nameof(Flags7L));
            }
        }

        // MultiselectListBox        
        
        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [Description("All flags")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Permissions.None)]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public Permissions Flags1L
        {
            get => this.flags1;
            set
            {
                this.flags1 = value;
                this.RaisePropertyChanged(nameof(Flags1L));
                this.RaisePropertyChanged(nameof(Flags1C));
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [Description("Without 'None' ")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Permissions.None)]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public Permissions Flags2L
        {
            get => this.flags2;
            set
            {
                this.flags2 = value;
                this.RaisePropertyChanged(nameof(Flags2L));
                this.RaisePropertyChanged(nameof(Flags2C));
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [DisplayName(nameof(Flags3L) + " (Nullable)")]
        [Description("Without 'None' (Nullable)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Exclude, Permissions.None)]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public Permissions? Flags3L
        {
            get => this.flags3;
            set
            {
                this.flags3 = value;
                this.RaisePropertyChanged(nameof(Flags3L));
                this.RaisePropertyChanged(nameof(Flags3C));
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [DisplayName(nameof(Flags4L) + " (Nullable)")]
        [Description("only 'Read' (Nullable)")]
        [EnumFilter(EnumFilterAttribute.FilteringMode.Include, Permissions.Read)]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public Permissions? Flags4L
        {
            get => this.flags4;
            set
            {
                this.flags4 = value;
                this.RaisePropertyChanged(nameof(Flags4L));
                this.RaisePropertyChanged(nameof(Flags4C));
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [Description("All flags")]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public PermissionsWithoutZero1 Flags5L
        {
            get => this.flags5;
            set
            {
                this.flags5 = value;
                this.RaisePropertyChanged(nameof(Flags5L));
                this.RaisePropertyChanged(nameof(Flags5C));                
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [DisplayName(nameof(Flags6L) + " (Nullable)")]
        [Description("Initialized by default. 'Write' is default. Set by System.ComponentModel.DefaultValue(2)")]
        [EnumMissingZeroBehaviorAttribute(initializeWithDefault: true, resetToDefault: true)]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public PermissionsWithoutZero2 Flags6L
        {
            get => this.flags6;
            set
            {
                this.flags6 = value;
                this.RaisePropertyChanged(nameof(Flags6L));
                this.RaisePropertyChanged(nameof(Flags6C));                
            }
        }

        [Browsable(false)] // hidden for now
        [Category("MultipleSelectListBox|")]
        [DisplayName(nameof(Flags7L) + " (Nullable)")]
        [Description("All flags (Nullable)")]
        [SelectorStyle(SelectorStyle.ListBox)]
        [SelectorMode(SelectorMode.Multiple)]
        public PermissionsWithoutZero1? Flags7L
        {
            get => this.flags7;
            set
            {
                this.flags7 = value;
                this.RaisePropertyChanged(nameof(Flags7L));
                this.RaisePropertyChanged(nameof(Flags7C));                
            }
        }
    }
}