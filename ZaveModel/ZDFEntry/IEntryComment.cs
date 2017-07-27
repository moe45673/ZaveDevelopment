///////////////////////////////////////////////////////////
//  IEntryComment.cs
//  Implementation of the Interface IEntryComment
//  Generated by Enterprise Architect
//  Created on:      28-Mar-2016 9:30:09 AM
//  Original author: Moshe
///////////////////////////////////////////////////////////

using System;
using Prism.Mvvm;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

//using System.Security.Principal;



namespace ZaveModel.ZDFEntry {

    /// <summary>
    /// Placeholder for early development. Eventually should be replaced by the user defined by the login system
    /// </summary>
    [JsonObject]
    public class User
    {

        /// <summary>
        /// The Name of the user
        /// </summary>
        [JsonProperty]
        public string Name { get; set; }


        public static explicit operator User(string s)  // explicit string to User conversion operator
        {
            User u = new User();
            u.Name = s;
            return u;
        }

        public static explicit operator string(User u = null)  // explicit User to string conversion operator
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

    /// <summary>
    /// EqualityComparer implementation for comparing Comments
    /// </summary>
    public class CommentEqualityComparer : EqualityComparer<IEntryComment>
    {
        public override bool Equals(IEntryComment x, IEntryComment y)
        {
            return x.CommentID == y.CommentID;
        }

        public override int GetHashCode(IEntryComment obj)
        {
            
            return obj.CommentID.GetHashCode() * obj.CommentText.GetHashCode() * obj.Author.GetHashCode();
        }
    }

    /// <summary>
    /// Interface that all Zave Comment classes implement
    /// </summary>
    public interface IEntryComment {

        

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


    /// <summary>
    /// Comments made by Users on various ZDF attributes
    /// </summary>
    [JsonObject]
    public class EntryComment : BindableBase, IEntryComment
    {

        //private static int idCounter = 0;

            /// <summary>
            /// Create a new EntryComment
            /// </summary>
            /// <param name="commText"></param>
            /// <param name="author"></param>
        public EntryComment(string commText = "", string author = default(string) ) : base()
        {
            _commentText = commText;
            _author = (User)author;
            IDTracker.setCommentID(this, out _commentID);
            

            

        }

        public EntryComment(IEntryComment newComm, bool SameID = false) : this(newComm.CommentText, newComm.Author.Name)
        {
            if (SameID)
            {
                CommentID = newComm.CommentID;
            }
        }

        //public override bool Equals(object obj)
        //{
        //    var commEq = new CommentEqualityComparer();
        //    return commEq.Equals(this as IEntryComment, obj as IEntryComment);
        //}

        //public override int GetHashCode()
        //{
        //    return new CommentEqualityComparer().GetHashCode(this as IEntryComment);
        //}


        [JsonIgnore]
        private int _commentID;
        [JsonProperty]
        public int CommentID
        {
            get { return this._commentID; }
            private set { SetProperty(ref _commentID, value); }
        }

        [JsonIgnore]
        private string _commentText;
        [JsonProperty]
        public string CommentText
        {
            get { return this._commentText; }
            set { SetProperty(ref _commentText, value); }
        }

        [JsonIgnore]
        private User _author;
        [JsonProperty]
        public User Author
        {
            get { return this._author; }
            set { SetProperty(ref _author, value); }
        }

        

        
    }

}//end namespace ZDFEntry

