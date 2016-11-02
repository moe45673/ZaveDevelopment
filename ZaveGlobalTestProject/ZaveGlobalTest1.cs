using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ZaveGlobalTestProject
{
    [TestClass]
    public class ZaveGlobalTest1
    {
        [TestMethod]
        public void CheckDefaultSelectionState()
        {
            var selState = new ZaveGlobalSettings.Data_Structures.SelectionState();
            Assert.AreEqual<int>(-1, selState.ID);
        }
    }
}
