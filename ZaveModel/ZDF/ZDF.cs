﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;



namespace ZaveModel.ZDF
{
    public sealed class ZDFSingleton : ObservableObject, IZDF
    {

        //public static event PropertyChangedEventHandler ModelPropertyChanged;
        

        

       

        //Needs to be protected virtual with private set
       

        private static ZDFSingleton instance;

        private ZDFSingleton()
        {
            isActive = true;
            
            EntryList = new List<ZDFEntry.IZDFEntry>();
        }

        public static ZDFSingleton Instance
        {
            get
            {
                if(instance == null)
                {
                    instance = new ZDFSingleton();
                }
                return instance;
            }
        }
        public bool isActive { get; set; }
        
        public List<ZDFEntry.IZDFEntry> EntryList { get; set; }

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(IZDFEntry zEntry)
        {
            try {
                EntryList.Add(zEntry);                
                OnPropertyChanged("EntryList");
            }
            catch(ArgumentException ae)
            {
                throw ae;
            }
        }
    }
}
