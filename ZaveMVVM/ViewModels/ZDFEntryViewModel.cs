﻿using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ComponentModel;
using ZaveModel;
using ZaveModel.ZDFEntry;
using System.Windows.Media;
using ZaveViewModel.Commands;
using Prism.Mvvm;
using Prism.Events;
using ZaveGlobalSettings.Events;

//using ZaveModel.Factories.ZDFEntry;
using ZaveGlobalSettings.Data_Structures;
//using Zave


namespace ZaveViewModel.ViewModels
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;
    





    public class ZDFEntryViewModel : BindableBase
    {

        private IZDFEntry _zdfEntry;
        //private IEventAggregator _eventAggregator;
        //private string _txtDocId;

        private void setProperties(int id, string name, string page, string txt, DateTime dateModded)
        {
            if (_zdfEntry == null) { 
                throw new NullReferenceException("No ZDFEntryViewModel referenced!");
            }

            TxtDocID = id.ToString();
            TxtDocName = name;
            TxtDocPage = page;
            TxtDocText = txt;
            TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
            
        }

        public SelectionState toSelectionState()
        {
            return _zdfEntry.toSelectionState();
        }

        private void setProperties(SelectionState selState)
        {
            try
            {
                setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified);
            }
            catch(NullReferenceException nre)
            {
                throw nre;
            }
        }

        public ZDFEntryViewModel(IZDFEntry zdfEntry)
        {
            _zdfEntry = zdfEntry;
            


            try
            {
                setProperties(zdfEntry.ID, zdfEntry.Name, zdfEntry.Page, zdfEntry.Text, zdfEntry.DateModified);
            }
            catch(NullReferenceException nre)
            {
                throw nre;
            }
            
        }
        #region Properties

        private String _txtDocName;
        public String TxtDocName
        {
            get { return _zdfEntry.Name; }
            set
            {


                SetProperty(ref _txtDocName, value);
                _zdfEntry.Name = _txtDocName;
                //System.Windows.Forms.MessageBox.Show(value.ToString());


            }

        }

        private String _txtDocID;
        public string TxtDocID
        {
            get { return _zdfEntry.ID.ToString(); }
            private set
            {

                SetProperty(ref _txtDocID, value);
                //_zdfEntry.ID = int.Parse(_txtDocID);
            }
            
        }

        private String _txtDocPage;
        public String TxtDocPage
        {
            get { return _zdfEntry.Page; }
            set
            {
                SetProperty(ref _txtDocPage, value);
                _zdfEntry.Page = _txtDocPage;
               
            }

        }

        private String _txtDocText;
        public String TxtDocText
        {
            get { return _zdfEntry.Text; }
            set
            {
                SetProperty(ref _txtDocText, value);
                _zdfEntry.Text = _txtDocText;
                
            }
        }

        private String _txtDocLastModified;
        public String TxtDocLastModified
        {
            get
            {
                string date;
                if (!_zdfEntry.DateModified.Equals(default(DateTime)))
                    date = _zdfEntry.DateModified.ToShortDateString() + " " + _zdfEntry.DateModified.ToShortTimeString();
                else
                    date = "";
                return date;

            }
            set
            {
                SetProperty(ref _txtDocLastModified, value);
                _zdfEntry.DateModified = DateTime.Parse(_txtDocLastModified);
                
            }
        }

        private String _txtDocColor;
        public String TxtDocColor
        {
            get
            {
                return _zdfEntry.HColor.Name;
            }
            set
            {
                SetProperty(ref _txtDocColor, value);
                _zdfEntry.HColor.ParseFromString(_txtDocColor);
                
            }
        }

        public ZDFEntry ZDFEntry
        {
            get;
        }

        #endregion 
    }
}
