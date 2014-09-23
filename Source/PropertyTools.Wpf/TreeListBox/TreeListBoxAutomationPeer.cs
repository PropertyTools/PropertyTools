// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TreeListBoxAutomationPeer.cs" company="PropertyTools">
//   Copyright (c) 2014 PropertyTools contributors
// </copyright>
// <summary>
//   Exposes T:TreeListBox types to UI Automation.
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