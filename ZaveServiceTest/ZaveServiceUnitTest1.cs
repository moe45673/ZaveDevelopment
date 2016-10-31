using System;
using System.Drawing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ZaveGlobalSettings.Data_Structures;
using ZaveModel.ZDFEntry;
using ZaveService;
using ZaveService.ZDFEntry;
using Prism.Events;

namespace ZaveServiceTest
{
    [TestClass]
    public class ZaveServiceUnitTest1
    {
        private ZaveModel.ZDFEntry.ZDFEntry entry;
        private static IZDFEntryService service;
        private static ZaveModel.ZDF.ZDFSingleton activeZDF;

        [ClassInitialize]
        public static void Initialize(TestContext tc)
        {
            activeZDF = ZaveModel.ZDF.ZDFSingleton.GetInstance(new EventAggregator());
            service = new ZDFEntryService();
        }


        [TestInitialize]
        public void Initialize()
        {
            
            entry = ZaveModel.ZDFEntry.ZDFEntry.CreateZDFEntry(new SelectionState(3, "EntryName", "24", "lorem ipsum", DateTime.Now, Color.Orange, SrcType.WORD));
            activeZDF.Add(entry);

            
        }

        [TestMethod]
        public void CreateDefaultService()
        {
            var newservice = new ZDFEntryService();
            Assert.IsNotNull(newservice);
            Assert.AreEqual<int>(ZaveModel.ZDF.ZDFSingleton.IDTracker, newservice.ActiveZDFEntry.ID);            
        }

        //[TestMethod]
        //public void CreateSpecificServiceInstance()
        //{
            

        //    Assert.IsNotNull(service);
        //    Assert.AreEqual<string>(((ZDFEntryService)service).ActiveZDFEntry.Name, entry.Name);
        //}

        [TestMethod]
        public void GetSpecificEntryFromService()
        {
            var tempEntry = service.getZDFEntry("3");

            Assert.AreEqual<string>("EntryName", tempEntry.Name);
        }

    }
}
