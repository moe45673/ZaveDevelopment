﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ZaveGlobalSettings.ZaveFile
{

    /// <summary>
    /// Unused
    /// </summary>
    public class ZaveFileSystemWatcher : FileSystemWatcher, IDisposable
    {
        #region Private Members
        // This Dictionary keeps the track of when an event occurred 
        // last for a particular file
        private Dictionary<string, DateTime> _lastFileEvent;
        
        // Interval in Millisecond
        private int _interval;
        
        //Timespan created when interval is set
        private TimeSpan _recentTimeSpan;
        
        #endregion

#region Properties
        // This Dictionary keeps the track of when an event occurred 
        // last for a particular file
        public Dictionary<string, DateTime> LastFileEvent { get {return _lastFileEvent;} set{_lastFileEvent = value;}}
        // Interval in Millisecond
        public int Interval {get {return _interval;} set{_interval = value;}}
        public bool FilterRecentEvents;
#endregion

        public ZaveFileSystemWatcher() : base()
        {
            InitializeMembers();
        }

        public ZaveFileSystemWatcher(string Path) : base(Path)
        {
            InitializeMembers();
        }

        public ZaveFileSystemWatcher(string Path, string Filter) : base(Path, Filter)
        {
            InitializeMembers();
        }

        /// <summary>
        /// This Method Initializes the private members.
        /// Interval is set to its default value of 100 millisecond
        /// FilterRecentEvents is set to true, _lastFileEvent dictionary is initialized
        /// We subscribe to the base class events.
        /// </summary>
        private void InitializeMembers()
        {
            Interval = 250;
            FilterRecentEvents = true;
            _lastFileEvent = new Dictionary<string, DateTime>();

            //base.Created += new FileSystemEventHandler(OnCreated);
            base.Changed += new FileSystemEventHandler(OnChanged);
            //base.Deleted += new FileSystemEventHandler(OnDeleted);
            //base.Renamed += new RenamedEventHandler(OnRenamed);
        }

        /// <summary>
        /// This method searches the dictionary to find out when the last event occurred
        /// for a particular file. If that event occurred within the specified timespan
        /// it returns true, else false
        /// </summary>
        /// <param name="FileName">The filename to be checked</param>
        /// <returns>True if an event has occurred within the specified interval, 
        /// False otherwise</returns>
        private bool HasAnotherFileEventOccuredRecently(string FileName)
        {
            bool retVal = false;

            // Check dictionary only if user wants to filter recent events 
            // otherwise return Value stays False
            if (FilterRecentEvents)
            {
                if (_lastFileEvent.ContainsKey(FileName))
                {
                    // If dictionary contains the filename, check how much time has elapsed
                    // since the last event occurred. If the timespan is less that the
                    // specified interval, set return value to true
                    // and store current datetime in dictionary for this file
                    DateTime lastEventTime = _lastFileEvent[FileName];
                    DateTime currentTime = DateTime.Now;
                    TimeSpan timeSinceLastEvent = currentTime - lastEventTime;
                    retVal = timeSinceLastEvent < _recentTimeSpan;
                    _lastFileEvent[FileName] = currentTime;

                    // If dictionary does not contain the filename,
                    // no event has occurred in past for this file, so set return value to false
                    // and filename alongwith current datetime to the dictionary
                    _lastFileEvent.Add(FileName, DateTime.Now);
                    retVal = false;
                }
            }

            return retVal;
        }

        // These events hide the events from the base class.
        // We want to raise these events appropriately and we do not want the
        // users of this class subscribing to these events of the base class accidentally
        public new event FileSystemEventHandler Changed;

        // Base class Event Handlers. Check if an event has occurred recently and call method
        // to raise appropriate event only if no recent event is detected
        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (!HasAnotherFileEventOccuredRecently(e.FullPath))
                this.OnChanged(e);
        }
        // Protected Methods to raise the Events for this class
        protected new virtual void OnChanged(FileSystemEventArgs e)
        {
            if (Changed != null) Changed(this, e);
        }

    }
}
