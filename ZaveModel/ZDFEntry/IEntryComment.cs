///////////////////////////////////////////////////////////
//  IEntryComment.cs
//  Implementation of the Interface IEntryComment
//  Generated by Enterprise Architect
//  Created on:      28-Mar-2016 9:30:09 AM
//  Original author: Moshe
///////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;



namespace ZaveModel.ZDFEntry.Comment {
	public interface IEntryComment  {

		string CommentText{
			get;
			set;
		}

		/// 
		/// <param name="newComm"></param>
		int Edit(string newComm);
	}//end IEntryComment

    public class EntryComment : IEntryComment
    {

        public EntryComment(string commText = "") : base()
        {
            CommentText = commText;
        }
        public string CommentText
        {
            get;
            set;                    
        }

        public int Edit(string newComm)
        {
            CommentText = newComm;
            return 1;
        }
    }

}//end namespace ZDFEntry

