using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace conf_visualization.Models
{
    internal class ConferenceModel : INotifyPropertyChanged
     {
        private int _id;
        private int _conferenceId;
        private string _conferenceName = "новый";
        private int _participantsCount = 3;
        private int _conferenceDuration = 15;
        private bool _isAcive;
        private bool _changedValue = false;
        private bool _newValue = false;
        private bool _hasError = false;
        private SolidColorBrush _conferenceIdColorBrush = Brushes.LightGray;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public SolidColorBrush ConferenceIdColorBrush
        {
            get { return _conferenceIdColorBrush; }
            set { 
                _conferenceIdColorBrush = value;
                OnPropertyChanged();
            }
        }
        public int ConferenceId
        {
            get { return _conferenceId; }
            set
            {
                if (_conferenceId != value)
                { 
                    _changedValue = true;
                    _conferenceId = value;
                    OnPropertyChanged();
                }               
            }
        }

        public int ID
        {
            get { return _id; }
            set { 
                _id = value;
                OnPropertyChanged();
            }
        }
        public string ConferenceName
        {
            get { return _conferenceName; }
            set
            {
                if (_conferenceName != value) { _changedValue = true; }
                _conferenceName = value;
                OnPropertyChanged();
            }
        }
        public int ParticipantsCount
        {
            get { return _participantsCount; }
            set
            {
            //    _hasError = false;
                if (_participantsCount != value)
                {
                    _changedValue = true;
                    _participantsCount = value;
                    OnPropertyChanged();
                }

            }
        }
        public int ConferenceDuration
        {
            get { return _conferenceDuration; }
            set
            {
            //    _hasError = false;
                if (_conferenceDuration != value)
                {
                    _changedValue = true;
                    _conferenceDuration = value;
                    OnPropertyChanged();
                }
            }

        }
        public bool IsAcive
        {
            get { return _isAcive; }
            set
            {
                if (_isAcive != value) 
                { 
                    _changedValue = true;
                    _isAcive = value;
                    OnPropertyChanged();
                }
                
            }
        }
        public bool ChangedValue
        {
            get { return _changedValue; }
            set 
            { 
                _changedValue = value;
                OnPropertyChanged();
            }
        }
        public bool NewValue
        {
            get { return _newValue; }
            set 
            { 
                _newValue = value;
                OnPropertyChanged();
            }
        }
        public bool hasError
        {
            get { return _hasError; }
            set 
            { 
                _hasError = value; 
                OnPropertyChanged();            
            }
        }




    }
}
