﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using ZaveModel.ZDFEntry;
using System.Collections.ObjectModel;
using ZaveGlobalSettings.Data_Structures;
using Prism.Mvvm;



namespace ZaveModel.ZDF
{


    public sealed class ZDFSingleton : BindableBase, IZDF
    {

        //Needs to be protected virtual with private set
       

        private static ZDFSingleton instance;
        private static readonly object syncRoot = new Object();
        private static int _iDTracker;
        //private string _date = DateTime.Now.ToShortTimeString();
        //FileSystemWatcher watcher;

        public static int setID()
        {          
            
            return ++_iDTracker;
        }



        private ZDFSingleton()
        {
            

           
            
            _entryList = new ObservableCollection<IZDFEntry>();
            
            
            if (_entryList.Count.Equals(0))
                _iDTracker = 0;
            else
            {
                //get highest existing id and set _iDtracker to it
            }
            //CreateFileWatcher(Path.GetTempPath());
        }



        public static ZDFSingleton Instance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new ZDFSingleton();
                    }
                }
                return instance;
            }
        }

        public static int IDTracker { get { return _iDTracker; } }

        

        private ObservableCollection<ZDFEntry.IZDFEntry> _entryList;
        
        public ObservableCollection<ZDFEntry.IZDFEntry> EntryList
        {
            get { return _entryList; }
            set { SetProperty(ref _entryList, value); }
        }


        public SelectionStateList toSelectionStateList()
        {
            SelectionStateList selStateList = SelectionStateList.Instance;

            selStateList.Clear();

            foreach(var item in EntryList)
            {
                selStateList.Add(item.toSelectionState());
            }

            return selStateList;


        }

        public IEnumerable<IZDFEntry> ListEntries()
        {
            return EntryList.ToList<ZDFEntry.IZDFEntry>();
        }

        public void Add(IZDFEntry zEntry)
        {
            try {
                _entryList.Add(zEntry);
                OnPropertyChanged("EntryList");

            }
            catch(ArgumentException ae)
            {
                throw ae;
            }
        }

        
    }
}
