// --------------------------------------------------------------------------------------------------------------------
// <copyright company="" file="BrowseForFolderDialog.cs">
//   
// </copyright>
// <summary>
//   Represents a common dialog box (Win32::SHBrowseForFolder()) that allows a user to select a folder.
// </summary>
// 
// --------------------------------------------------------------------------------------------------------------------
using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Interop;

namespace PropertyTools.Wpf.Shell32
{
    /// <summary>
    /// Represents a common dialog box (Win32::SHBrowseForFolder()) that allows a user to select a folder.
    /// </summary>
    public class BrowseForFolderDialog
    {
        #region Public Properties

        /// <summary>
        /// The browse info.
        /// </summary>
        private BROWSEINFOW browseInfo;

        /// <summary>
        /// Gets the current and or final selected folder path.
        /// </summary>
        public string SelectedFolder { get; protected set; }

        /// <summary>
        /// Gets or sets the string that is displayed above the tree view control in the dialog box (must set BEFORE calling ShowDialog()). 
        /// </summary>
        public string Title
        {
            get { return BrowseInfo.lpszTitle; }
            set { BrowseInfo.lpszTitle = value; }
        }

        /// <summary>
        /// Gets or sets the initially selected folder path.
        /// </summary>
        public string InitialFolder { get; set; }

        /// <summary>
        /// Gets or sets the initially selected and expanded folder path.  Overrides SelectedFolder.
        /// </summary>
        public string InitialExpandedFolder { get; set; }

        /// <summary>
        /// Gets or sets the text for the dialog's OK button.
        /// </summary>
        public string OKButtonText { get; set; }

        /// <summary>
        /// Provides direct access to the Win32::SHBrowseForFolder() BROWSEINFO structure used to create the dialog in ShowDialog().
        /// </summary>
        public BROWSEINFOW BrowseInfo
        {
            get { return browseInfo; }
            protected set { browseInfo = value; }
        }

        /// <summary>
        /// Provides direct access to the ulFlags field of the Win32::SHBrowseForFolder() structure used to create the dialog in ShowDialog().
        /// </summary>
        public BrowseInfoFlags BrowserDialogFlags
        {
            get { return BrowseInfo.ulFlags; }
            set { BrowseInfo.ulFlags = value; }
        }

        #endregion

        #region Public Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowseForFolderDialog"/> class. 
        /// Constructs a BrowseForFolderDialog with default BrowseInfoFlags set to BIF_NEWDIALOGSTYLE.
        /// </summary>
        public BrowseForFolderDialog()
        {
            BrowseInfo = new BROWSEINFOW();
            BrowseInfo.hwndOwner = IntPtr.Zero;
            BrowseInfo.pidlRoot = IntPtr.Zero;
            BrowseInfo.pszDisplayName = new string(' ', 260);
            BrowseInfo.lpszTitle = "Select a folder:";
            BrowseInfo.ulFlags = BrowseInfoFlags.BIF_NEWDIALOGSTYLE;
            BrowseInfo.lpfn = new BrowseCallbackProc(BrowseEventHandler);
            BrowseInfo.lParam = IntPtr.Zero;
            BrowseInfo.iImage = -1;
        }

        #endregion

        #region Public ShowDialog() Overloads

        /// <summary>
        /// Shows the dialog (Win32::SHBrowseForFolder()).
        /// </summary>
        public bool? ShowDialog()
        {
            return PInvokeSHBrowseForFolder(null);
        }

        /// <summary>
        /// Shows the dialog (Win32::SHBrowseForFolder()) with its hwndOwner set to the handle of 'owner'.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        public bool? ShowDialog(Window owner)
        {
            return PInvokeSHBrowseForFolder(owner);
        }

        #endregion

        #region PInvoke Stuff

        #region Delegates

        /// <summary>
        /// The browse callback proc.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="uMsg">
        /// The u msg.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <param name="lpData">
        /// The lp data.
        /// </param>
        public delegate int BrowseCallbackProc(IntPtr hwnd, MessageFromBrowser uMsg, IntPtr lParam, IntPtr lpData);

        #endregion

        #region BrowseInfoFlags enum

        /// <summary>
        /// The browse info flags.
        /// </summary>
        [Flags]
        public enum BrowseInfoFlags : uint
        {
            /// <summary>
            /// No specified BIF_xxx flags.
            /// </summary>
            BIF_None = 0x0000, 

            /// <summary>
            /// Only return file system directories. If the user selects folders that are not part of the file system, the OK button is grayed.
            /// </summary>
            BIF_RETURNONLYFSDIRS = 0x0001, // For finding a folder to start document searching
            /// <summary>
            /// Do not include network folders below the domain level in the dialog box's tree view control.
            /// </summary>
            BIF_DONTGOBELOWDOMAIN = 0x0002, // For starting the Find Computer
            /// <summary>
            /// Include a status area in the dialog box. 
            /// </summary>
            BIF_STATUSTEXT = 0x0004, // Top of the dialog has 2 lines of text for BROWSEINFO.lpszTitle and one line if
            // this flag is set.  Passing the message BFFM_SETSTATUSTEXTA to the hwnd can set the
            // rest of the text.  This is not used with BIF_USENEWUI and BROWSEINFO.lpszTitle gets
            // all three lines of text.
            /// <summary>
            /// Only return file system ancestors. An ancestor is a subfolder that is beneath the root folder in the namespace hierarchy.
            /// </summary>
            BIF_RETURNFSANCESTORS = 0x0008, 

            /// <summary>
            /// Include an edit control in the browse dialog box that allows the user to type the name of an item.
            /// </summary>
            BIF_EDITBOX = 0x0010, // Add an editbox to the dialog
            /// <summary>
            /// If the user types an invalid name into the edit box, the browse dialog box will call the application's BrowseCallbackProc with the BFFM_VALIDATEFAILED message. 
            /// </summary>
            BIF_VALIDATE = 0x0020, // insist on valid result (or CANCEL)
            /// <summary>
            /// Use the new user interface. Setting this flag provides the user with a larger dialog box that can be resized.
            /// </summary>
            BIF_NEWDIALOGSTYLE = 0x0040, // Use the new dialog layout with the ability to resize
            // Caller needs to call OleInitialize() before using this API
            /// <summary>
            /// Use the new user interface, including an edit box. This flag is equivalent to BIF_EDITBOX | BIF_NEWDIALOGSTYLE. 
            /// </summary>
            BIF_USENEWUI = BIF_NEWDIALOGSTYLE | BIF_EDITBOX, 

            /// <summary>
            /// The browse dialog box can display URLs. The BIF_USENEWUI and BIF_BROWSEINCLUDEFILES flags must also be set. 
            /// </summary>
            BIF_BROWSEINCLUDEURLS = 0x0080, // Allow URLs to be displayed or entered. (Requires BIF_USENEWUI)
            /// <summary>
            /// When combined with BIF_NEWDIALOGSTYLE, adds a usage hint to the dialog box in place of the edit box.
            /// </summary>
            BIF_UAHINT = 0x0100, 

// Add a UA hint to the dialog, in place of the edit box. May not be combined with BIF_EDITBOX
            /// <summary>
            /// Do not include the New Folder button in the browse dialog box.
            /// </summary>
            BIF_NONEWFOLDERBUTTON = 0x0200, 

// Do not add the "New Folder" button to the dialog.  Only applicable with BIF_NEWDIALOGSTYLE.
            /// <summary>
            /// When the selected item is a shortcut, return the PIDL of the shortcut itself rather than its target.
            /// </summary>
            BIF_NOTRANSLATETARGETS = 0x0400, // don't traverse target as shortcut
            /// <summary>
            /// Only return computers. If the user selects anything other than a computer, the OK button is grayed.
            /// </summary>
            BIF_BROWSEFORCOMPUTER = 0x1000, // Browsing for Computers.
            /// <summary>
            /// Only allow the selection of printers. If the user selects anything other than a printer, the OK button is grayed. 
            /// </summary>
            BIF_BROWSEFORPRINTER = 0x2000, // Browsing for Printers
            /// <summary>
            /// The browse dialog box will display files as well as folders.
            /// </summary>
            BIF_BROWSEINCLUDEFILES = 0x4000, // Browsing for Everything
            /// <summary>
            /// The browse dialog box can display shareable resources on remote systems. 
            /// </summary>
            BIF_SHAREABLE = 0x8000 // sharable resources displayed (remote shares, requires BIF_USENEWUI)
        }

        #endregion

        // message from browser
        #region MessageFromBrowser enum

        /// <summary>
        /// The message from browser.
        /// </summary>
        public enum MessageFromBrowser : uint
        {
            /// <summary>
            /// The dialog box has finished initializing.
            /// </summary>
            BFFM_INITIALIZED = 1, 

            /// <summary>
            /// The selection has changed in the dialog box.
            /// </summary>
            BFFM_SELCHANGED = 2, 

            /// <summary>
            /// (ANSI) The user typed an invalid name into the dialog's edit box. A nonexistent folder is considered an invalid name.
            /// </summary>
            BFFM_VALIDATEFAILEDA = 3, 

            /// <summary>
            /// (Unicode) The user typed an invalid name into the dialog's edit box. A nonexistent folder is considered an invalid name.
            /// </summary>
            BFFM_VALIDATEFAILEDW = 4, 

            /// <summary>
            /// An IUnknown interface is available to the dialog box.
            /// </summary>
            BFFM_IUNKNOWN = 5
        }

        #endregion

        // messages to browser
        #region MessageToBrowser enum

        /// <summary>
        /// The message to browser.
        /// </summary>
        public enum MessageToBrowser : uint
        {
            /// <summary>
            /// Win32 API macro - start of user defined window message range.
            /// </summary>
            WM_USER = 0x0400, 

            /// <summary>
            /// (ANSI) Sets the status text. Set lpData to point to a null-terminated string with the desired text. 
            /// </summary>
            BFFM_SETSTATUSTEXTA = WM_USER + 100, 

            /// <summary>
            /// Enables or disables the dialog box's OK button.  lParam - To enable, set to a nonzero value. To disable, set to zero.
            /// </summary>
            BFFM_ENABLEOK = WM_USER + 101, 

            /// <summary>
            /// (ANSI) Specifies the path of a folder to select. 
            /// </summary>
            BFFM_SETSELECTIONA = WM_USER + 102, 

            /// <summary>
            /// (Unicode) Specifies the path of a folder to select. 
            /// </summary>
            BFFM_SETSELECTIONW = WM_USER + 103, 

            /// <summary>
            /// (Unicode) Sets the status text. Set lpData to point to a null-terminated string with the desired text. 
            /// </summary>
            BFFM_SETSTATUSTEXTW = WM_USER + 104, 

            /// <summary>
            /// Sets the text that is displayed on the dialog box's OK button.
            /// </summary>
            BFFM_SETOKTEXT = WM_USER + 105, // Unicode only
            /// <summary>
            /// Specifies the path of a folder to expand in the Browse dialog box. 
            /// </summary>
            BFFM_SETEXPANDED = WM_USER + 106 // Unicode only
        }

        #endregion

        /// <summary>
        /// The p invoke sh browse for folder.
        /// </summary>
        /// <param name="owner">
        /// The owner.
        /// </param>
        /// <returns>
        /// </returns>
        private bool? PInvokeSHBrowseForFolder(Window owner)
        {
            WindowInteropHelper windowhelper;
            if (null != owner)
            {
                windowhelper = new WindowInteropHelper(owner);
                BrowseInfo.hwndOwner = windowhelper.Handle;
            }

            IntPtr pidl = SHBrowseForFolderW(browseInfo);

            if (IntPtr.Zero != pidl)
            {
                var pathsb = new StringBuilder(260);
                if (SHGetPathFromIDList(pidl, pathsb))
                {
                    SelectedFolder = pathsb.ToString();
                    Marshal.FreeCoTaskMem(pidl);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// The browse event handler.
        /// </summary>
        /// <param name="hwnd">
        /// The hwnd.
        /// </param>
        /// <param name="uMsg">
        /// The u msg.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <param name="lpData">
        /// The lp data.
        /// </param>
        /// <returns>
        /// The browse event handler.
        /// </returns>
        private int BrowseEventHandler(IntPtr hwnd, MessageFromBrowser uMsg, IntPtr lParam, IntPtr lpData)
        {
            switch (uMsg)
            {
                case MessageFromBrowser.BFFM_INITIALIZED:
                    {
                        // The dialog box has finished initializing.
                        // lParam   Not used, value is NULL.
                        if (!string.IsNullOrEmpty(InitialExpandedFolder))
                        {
                            SendMessageW(hwnd, MessageToBrowser.BFFM_SETEXPANDED, new IntPtr(1), InitialExpandedFolder);
                        }
                        else if (!string.IsNullOrEmpty(InitialFolder))
                        {
                            SendMessageW(hwnd, MessageToBrowser.BFFM_SETSELECTIONW, new IntPtr(1), InitialFolder);
                        }

                        if (!string.IsNullOrEmpty(OKButtonText))
                        {
                            SendMessageW(hwnd, MessageToBrowser.BFFM_SETOKTEXT, new IntPtr(1), OKButtonText);
                        }

                        break;
                    }

                case MessageFromBrowser.BFFM_SELCHANGED:
                    {
                        // The selection has changed in the dialog box.
                        // lParam   A pointer to an item identifier list (PIDL) identifying the newly selected item.
                        var pathsb = new StringBuilder(260);
                        if (SHGetPathFromIDList(lParam, pathsb))
                        {
                            SelectedFolder = pathsb.ToString();
                        }

                        break;
                    }

                case MessageFromBrowser.BFFM_VALIDATEFAILEDA:
                    {
// ANSI
                        // The user typed an invalid name into the dialog's edit box. A nonexistent folder is considered an invalid name.
                        // lParam   A pointer to a string containing the invalid name. An application can use this data in an error dialog informing the user that the name was not valid.
                        // Return zero to dismiss the dialog or nonzero to keep the dialog displayed
                        break;
                    }

                case MessageFromBrowser.BFFM_VALIDATEFAILEDW:
                    {
// Unicode
                        // The user typed an invalid name into the dialog's edit box. A nonexistent folder is considered an invalid name.
                        // lParam   A pointer to a string containing the invalid name. An application can use this data in an error dialog informing the user that the name was not valid.
                        // Return zero to dismiss the dialog or nonzero to keep the dialog displayed
                        break;
                    }

                case MessageFromBrowser.BFFM_IUNKNOWN:
                    {
                        // An IUnknown interface is available to the dialog box.
                        // lParam   A pointer to an IUnknown interface.
                        break;
                    }
            }

            return 0;
        }

        /// <summary>
        /// The sh browse for folder w.
        /// </summary>
        /// <param name="bi">
        /// The bi.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("shell32.dll")]
        private static extern IntPtr SHBrowseForFolderW([MarshalAs(UnmanagedType.LPStruct), In, Out] BROWSEINFOW bi);

        /// <summary>
        /// The sh get path from id list.
        /// </summary>
        /// <param name="pidl">
        /// The pidl.
        /// </param>
        /// <param name="path">
        /// The path.
        /// </param>
        /// <returns>
        /// The sh get path from id list.
        /// </returns>
        [DllImport("shell32.dll")]
        private static extern bool SHGetPathFromIDList(IntPtr pidl, StringBuilder path);

        /// <summary>
        /// The send message w.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="Msg">
        /// The msg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="lParam">
        /// The l param.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        /// <summary>
        /// The send message w.
        /// </summary>
        /// <param name="hWnd">
        /// The h wnd.
        /// </param>
        /// <param name="msg">
        /// The msg.
        /// </param>
        /// <param name="wParam">
        /// The w param.
        /// </param>
        /// <param name="str">
        /// The str.
        /// </param>
        /// <returns>
        /// </returns>
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, MessageToBrowser msg, IntPtr wParam, 
                                                 [MarshalAs(UnmanagedType.LPWStr)] string str);

        /// <summary>
        /// The browseinfow.
        /// </summary>
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        public class BROWSEINFOW
        {
            /// <summary>
            /// A handle to the owner window for the dialog box.
            /// </summary>
            public IntPtr hwndOwner;

            /// <summary>
            /// A pointer to an item identifier list (PIDL) specifying the location of the root folder from which to start browsing. 
            /// </summary>
            public IntPtr pidlRoot; // PCIDLIST_ABSOLUTE

            /// <summary>
            /// The address of a buffer to receive the display name of the folder selected by the user. The size of this buffer is assumed to be MAX_PATH characters.
            /// </summary>
            public string pszDisplayName; // Output parameter! (length must be >= MAX_PATH)

            /// <summary>
            /// The address of a null-terminated string that is displayed above the tree view control in the dialog box. 
            /// </summary>
            public string lpszTitle;

            /// <summary>
            /// Flags specifying the options for the dialog box. 
            /// </summary>
            public BrowseInfoFlags ulFlags;

            /// <summary>
            /// A BrowseCallbackProc delegate that the dialog box calls when an event occurs.
            /// </summary>
            public BrowseCallbackProc lpfn;

            /// <summary>
            /// An application-defined value that the dialog box passes to the BrowseCallbackProc delegate, if one is specified.
            /// </summary>
            public IntPtr lParam;

            /// <summary>
            /// A variable to receive the image associated with the selected folder. The image is specified as an index to the system image list.
            /// </summary>
            public int iImage; // Output parameter!
        }

        #endregion
    }
}