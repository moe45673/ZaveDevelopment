///////////////////////////////////////////////////////////
//  IZDFEntry.cs
//  Implementation of the Interface IZDFEntry
//  Generated by Enterprise Architect
//  Created on:      28-Mar-2016 9:30:09 AM
//  Original author: Moshe
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.ComponentModel;
using ZaveGlobalSettings.Data_Structures;
using Prism.Mvvm;
using ZaveGlobalSettings.Data_Structures.ZaveObservableCollection;
using ZaveModel.ZDFColors;


namespace ZaveModel.ZDFEntry
{
    /// <summary>
    /// Interface that all entries must implement
    /// </summary>
	public interface IZDFEntry
    {

        //event EventHandler<ZaveGlobalSettings.Data_Structures.ModelEventArgs> PropertyChanged;

        /// <summary>
        /// All Comments associated with this entry
        /// </summary>
        ObservableImmutableList<IEntryComment> Comments { get; set; }

        /// <summary>
        /// The ColorCategory associated with this entry
        /// </summary>
        ColorCategory HColor
        {
            get;
            set;
        }

        //ZaveGlobalSettings.Data_Structures.SelectionState Source
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// The page that this entry was lifted from
        /// </summary>
        /// <remarks>This should be replaced to make entries less Document-centric and more flexible regarding
        /// alternative sources (EG Emails)</remarks>        
        string Page
        {
            get;
            set;
        }

        /// <summary>
        /// The name of the source document
        /// </summary>
        /// /// <remarks>This should be replaced to make entries less Document-centric and more flexible regarding
        /// alternative sources (EG Emails)</remarks>   
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// The text that was highlighted
        /// </summary>
        string Text
        {
            get;
            set;
        }

        /// <summary>
        /// The last time this entry was modified
        /// </summary>        
        DateTime DateModified
        {
            get;
            set;
        }

        /// <summary>
        /// The type of source this entry was taken from (EG spreadsheet, document, etc)
        /// </summary>
        SrcType Format
        {
            get;
            set;
        }

        /// <summary>
        /// Identifier of this entry
        /// </summary>
        int ID
        {
            get;
        }

        SelectionState toSelectionState();



    }//end IZDFEntry

}//end namespace ZDFEntry