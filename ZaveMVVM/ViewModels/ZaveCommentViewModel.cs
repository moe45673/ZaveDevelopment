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
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Prism.Mvvm;
using Prism.Events;
using Prism.Commands;
using Prism.Regions;
using Prism.Unity;
using ZaveGlobalSettings.Data_Structures;
using ZaveViewModel.Data_Structures;
using System.Collections;
using ZaveModel.ZDF;
using ZaveService.ZDFEntry;
using Prism.Interactivity.InteractionRequest;

namespace ZaveViewModel.ViewModels
{

    public class ZaveCommentViewModel : ZaveCommentItem
    {


        public ZaveCommentViewModel(IUnityContainer cont, IEventAggregator agg, IRegionManager rm, ref IEntryComment modelComment, string text = default(string), string author = default(string)) : base(modelComment, text, author)
        {

        }

        private ZaveCommentViewModel(IEntryComment modelComment) : base(modelComment)
        {

        }


        public static ZaveCommentViewModel ItemFactory(IUnityContainer cont, IEventAggregator agg, IRegionManager rm, ref IEntryComment modelComment, string text = default(string), string author = default(string))
        {


            var item = new ZaveCommentViewModel(cont, agg, rm, ref modelComment, text, author);

            return item;
        }

        
        

        //public static ZDFCommentItem fromObject(Object<Object, String, String> obj = default(Object<Object, String, String>))
        //{

        //    return new ZDFCommentItem(obj.FirstProp as ModelComment.EntryComment, obj.SecondProp, obj.ThirdProp);
        //}

        public static explicit operator ZaveCommentViewModel(EntryComment comment)
        {
            return new ZaveCommentViewModel(comment);
        }

        public static explicit operator EntryComment(ZaveCommentViewModel commItem)
        {
            return new EntryComment(commItem.CommentText, commItem.CommentAuthor);
        }



    }
}
