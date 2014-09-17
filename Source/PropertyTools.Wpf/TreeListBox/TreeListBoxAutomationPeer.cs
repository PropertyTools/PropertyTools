// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxAutomationPeer.cs" company="PropertyTools">
//   The MIT License (MIT)
//   
//   Copyright (c) 2014 PropertyTools contributors
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
//   Exposes TreeListBox types to UI Automation.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace PropertyTools.Wpf
{
    using System.Collections.Generic;
    using System.Windows.Automation.Peers;

    /// <summary>
    /// Exposes <see cref="T:TreeListBox"/> types to UI Automation.
    /// </summary>
    public class TreeListBoxAutomationPeer : ListBoxAutomationPeer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TreeListBoxAutomationPeer"/> class.
        /// </summary>
        /// <param name="owner">The owner.</param>
        public TreeListBoxAutomationPeer(TreeListBox owner)
            : base(owner)
        {
        }

        /// <summary>
        /// Gets the name of the <see cref="T:TreeListBox" /> that is associated with this <see cref="T:TreeListBoxAutomationPeer" />. 
        /// This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetClassName" />.
        /// </summary>
        /// <returns>A string that contains "ListBox".</returns>
        protected override string GetClassNameCore()
        {
            return "TreeListBox";
        }

        /// <summary>
        /// Gets the collection of child elements of the <see cref="T:System.Windows.Controls.ItemsControl" /> 
        /// that is associated with this <see cref="T:System.Windows.Automation.Peers.ItemsControlAutomationPeer" />. 
        /// This method is called by <see cref="M:System.Windows.Automation.Peers.AutomationPeer.GetChildren" />.
        /// </summary>
        /// <returns>The collection of child elements.</returns>
        protected override List<AutomationPeer> GetChildrenCore()
        {
            return base.GetChildrenCore();
        }
    }
}