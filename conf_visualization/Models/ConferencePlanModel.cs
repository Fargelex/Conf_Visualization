using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization.Models
{
    internal class ConferencePlanModel
    {
        private int _conferenceId;
        private int _conferencePlanId;
        private DateTime _conferenceBeginPeriod;
        private DateTime _conferenceEndPeriod;
        private string _periodicType = "Еженедельно";
        private string _periodicValue;
        private DateTime _conferenceStartTime;
        private DateTime _conferenceStopTime;


        public int ConferenceId 
        {
            get { return _conferenceId; }
            set { _conferenceId = value; }
        }
        public int ConferencePlanId 
        {
            get { return _conferencePlanId; }
            set { _conferencePlanId = value;}
        }
        public DateTime ConferenceBeginPeriod 
        {
            get { return _conferenceBeginPeriod; }
            set
            {
                _conferenceBeginPeriod = value;
            }
        }
        public DateTime ConferenceEndPeriod 
        {
            get { return _conferenceEndPeriod; }
            set
            {
                _conferenceEndPeriod = value;
            }
        }
        public string PeriodicType 
        {
            get { return _periodicType; }
            set
            {
                _periodicType = value;
            }
        }
        public string PeriodicValue 
        { 
            get { return _periodicValue; }
            set
            {
                _periodicValue = value;
            }
        }
        public DateTime ConferenceStartTime 
        { 
            get { return _conferenceStartTime; }
            set
            {
                _conferenceStartTime = value;
            }
        }
        public DateTime ConferenceStopTime
        {
            get { return _conferenceStopTime; }
            set
            {
                _conferenceStopTime = value;
            }
        }
        public bool IsAcive
        {
            get; set;
        }






    }
}
