using System;
//using GalaSoft.MvvmLight;
//using GalaSoft.MvvmLight.CommandWpf;
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
using ZaveViewModel.Data_Structures;
//using Zave


namespace ZaveViewModel.ViewModels
{
    //using activeZDF = ZaveModel.ZDF.ZDFSingleton;

    public class ZDFEntryViewModel : ZDFEntryItem
    {
       
        private IEventAggregator _eventAggregator;
        public ZDFEntryViewModel(IEventAggregator eventAgg) : base(new ZDFEntry())
        {
            
            if (_eventAggregator == null && eventAgg != null)
            {
                _eventAggregator = eventAgg;
                _eventAggregator.GetEvent<EntryUpdateEvent>().Subscribe(setProperties);
                
            }
            try
            {
                setProperties(_zdfEntry.ID, _zdfEntry.Name, _zdfEntry.Page, _zdfEntry.Text, _zdfEntry.DateModified, _zdfEntry.HColor.Color);
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            
        }

        public static ZDFEntryViewModel entryVMFactory(IEventAggregator eventAgg, IZDFEntry entry)
        {
            var entryVM = new ZDFEntryViewModel(eventAgg);
            entryVM._zdfEntry = entry;            

            try
            {
                entryVM.setProperties(entry.ID, entry.Name, entry.Page, entry.Text, entry.DateModified, entry.HColor.Color);
            }
            catch (NullReferenceException nre)
            {
                throw nre;
            }
            return entryVM;
        }

       

        //private string _txtDocId;

        //protected void setProperties(int id = default(int), string name = default(string), string page = default(string), string txt = default(string), DateTime dateModded = default(DateTime), System.Drawing.Color col = default(System.Drawing.Color))
        //{
        //    if (_zdfEntry == null)
        //    {
        //        throw new NullReferenceException("No ZDFEntryItemViewModel referenced!");
        //    }

        //    _txtDocID = id.ToString();
        //    TxtDocName = name;
        //    TxtDocPage = page;
        //    TxtDocText = txt;
        //    TxtDocLastModified = dateModded.ToShortDateString() + " " + dateModded.ToShortTimeString();
        //    _txtDocColor = new Color();
        //    _txtDocColor.A = col.A;
        //    _txtDocColor.R = col.R;
        //    _txtDocColor.B = col.B;
        //    _txtDocColor.G = col.G;



        //}

        //public SelectionState toSelectionState()
        //{
        //    return _zdfEntry.toSelectionState();
        //}

        //protected void setProperties(SelectionState selState)
        //{
        //    try
        //    {
        //        setProperties(selState.ID, selState.SelectionDocName, selState.SelectionPage, selState.SelectionText, selState.SelectionDateModified, selState.Color);
        //    }
        //    catch (NullReferenceException nre)
        //    {
        //        throw nre;
        //    }
        //}

        //#region Properties

        //protected String _txtDocName;
        //public String TxtDocName
        //{
        //    get { return _zdfEntry.Name; }
        //    set
        //    {
        //        SetProperty(ref _txtDocName, value);
        //        _zdfEntry.Name = _txtDocName;
        //        //System.Windows.Forms.MessageBox.Show(value.ToString());


        //    }

        //}

        //protected String _txtDocID;
        //public string TxtDocID
        //{
        //    get
        //    {
        //        string tempID = "";
        //        if (_txtDocID != _zdfEntry.ID.ToString())
        //        {
        //            _txtDocID = _zdfEntry.ID.ToString();
        //            OnPropertyChanged("TxtDocID");
        //        }

        //        tempID = _txtDocID;

        //        return tempID;
        //    }
        //    protected set
        //    {

        //        SetProperty(ref _txtDocID, value);
        //        //_zdfEntry.ID = int.Parse(_txtDocID);
        //    }

        //}

        //protected String _txtDocPage;
        //public String TxtDocPage
        //{
        //    get { return _zdfEntry.Page; }
        //    set
        //    {
        //        if (SetProperty(ref _txtDocPage, value))
        //            _zdfEntry.Page = _txtDocPage;

        //    }

        //}

        //protected String _txtDocText;
        //public String TxtDocText
        //{
        //    get { return _zdfEntry.Text; }
        //    set
        //    {
        //        if (SetProperty(ref _txtDocText, value))
        //            _zdfEntry.Text = _txtDocText;

        //    }
        //}

        //protected String _txtDocLastModified;
        //public String TxtDocLastModified
        //{
        //    get
        //    {
        //        string date;
        //        if (!_zdfEntry.DateModified.Equals(default(DateTime)))
        //            date = _zdfEntry.DateModified.ToShortDateString() + " " + _zdfEntry.DateModified.ToShortTimeString();
        //        else
        //            date = "";
        //        return date;

        //    }
        //    set
        //    {
        //        SetProperty(ref _txtDocLastModified, value);
        //        _zdfEntry.DateModified = DateTime.Parse(_txtDocLastModified);

        //    }
        //}

        //protected Color _txtDocColor;
        //public Color TxtDocColor
        //{
        //    get
        //    {
        //        return _zdfEntry.HColor.toWPFColor();
        //    }
        //    set
        //    {
        //        SetProperty(ref _txtDocColor, value);
        //        _zdfEntry.HColor = ZaveModel.Colors.ColorCategory.FromWPFColor(value);

        //    }
        //}

        //public ZDFEntry ZDFEntry
        //{
        //    get;
        //}

        //#endregion 



    }
}
