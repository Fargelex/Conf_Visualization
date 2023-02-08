using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization.Models
{
    internal class ConferenceModel
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

        public int ConferenceId
        {
            get { return _conferenceId; }
            set
            {
                if (_conferenceId != value) { _changedValue = true; }
                _conferenceId = value;
            }
        }

        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        public string ConferenceName
        {
            get { return _conferenceName; }
            set
            {
                if (_conferenceName != value) { _changedValue = true; }
                _conferenceName = value;
            }
        }
        public int ParticipantsCount
        {
            get { return _participantsCount; }
            set
            {
                _hasError = false;
                if (_participantsCount != value)
                {
                    _changedValue = true;
                    _participantsCount = value;
                }

            }
        }
        public int ConferenceDuration
        {
            get { return _conferenceDuration; }
            set
            {
                _hasError = false;
                if (_conferenceDuration != value)
                {
                    _changedValue = true;
                    _conferenceDuration = value;
                }
            }

        }
        public bool IsAcive
        {
            get { return _isAcive; }
            set
            {
                if (_isAcive != value) { _changedValue = true; }
                _isAcive = value;
            }
        }
        public bool ChangedValue
        {
            get { return _changedValue; }
            set { _changedValue = value; }
        }
        public bool NewValue
        {
            get { return _newValue; }
            set { _newValue = value; }
        }
        public bool hasError
        {
            get { return _hasError; }
        }




    }
}
