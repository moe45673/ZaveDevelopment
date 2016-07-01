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
using Prism.Events;
using Prism.Mvvm;
//using System.Security.Principal;



namespace ZaveModel.ZDFEntry.Comment {

    public class User
    {
        public string Name { get; set; }

        public static explicit operator User(string s)  // explicit string to User conversion operator
        {
            User u = new User();
            u.Name = s;
            return u;
        }

        public static explicit operator string(User u = default(User))  // explicit string to User conversion operator
        {
            try
            {
                return u.Name;
            }
            catch(NullReferenceException nre)
            {
                throw nre;
            }
        }

    }

	public interface IEntryComment  {

        int CommentID
        {
            get;
            
        }

		string CommentText{
			get;
			set;
		}

        User Author
        {
            get;
            set;
        }

		
	}//end IEntryComment

    internal static class IDTracker
    {
        private static int idCounter = 0;

        public static void setCommentID(IEntryComment comment, out int id)
        {
            if (comment == null)
                throw new InvalidOperationException();

            id = idCounter++;


        }
    }

    public class EntryComment : BindableBase, IEntryComment
    {

        //private static int idCounter = 0;

        public EntryComment(string commText = "", string author = default(string) ) : base()
        {
            _commentText = commText;
            _author = (User)author;
            IDTracker.setCommentID(this, out _commentID);

        }


        private int _commentID;

        public int CommentID
        {
            get { return this._commentID; }
            private set { SetProperty(ref _commentID, value); }
        }


        private string _commentText;

        public string CommentText
        {
            get { return this._commentText; }
            set { SetProperty(ref _commentText, value); }
        }


        private User _author;

        public User Author
        {
            get { return this._author; }
            set { SetProperty(ref _author, value); }
        }


    }

}//end namespace ZDFEntry

