using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace conf_visualization.Models
{
    public enum PeriodicTypeEnum{ weekly, monthly };


    internal class ConferencePlanModel
    {
        private int _conferenceId;
        private int _conferencePlanId;
        private DateTime _conferenceBeginPeriod = DateTime.Now;
        private DateTime _conferenceEndPeriod = DateTime.Now;
        private string _periodicType = "Еженедельно";
        private string _periodicValue;
        private DateTime _conferenceStartTime = new DateTime(2022, 12, 30, 00, 00, 00);
        private DateTime _conferenceStopTime = new DateTime(2022, 12, 31, 00, 00, 00);

        private PeriodicTypeEnum _periodicTypeEnum = PeriodicTypeEnum.weekly;


        public string PeriodicType2
        {
            get 
            {
                switch (_periodicTypeEnum)
                {
                    case PeriodicTypeEnum.weekly:
                        return "Еженедельно";
                    case PeriodicTypeEnum.monthly:
                        return "Ежемесячно";
                    default:
                        return "Еженедельно";
                }
          //      return _periodicType; 
            }
            set {
                switch (value)
                {
                    case "Еженедельно":
                        _periodicTypeEnum = PeriodicTypeEnum.weekly; 
                        break;
                    case "Ежемесячно":
                        _periodicTypeEnum = PeriodicTypeEnum.monthly;
                        break;
                    default:
                        break;
                }
            //    _periodicType = value; 
            }
        }

        public int ConferenceId 
        {
            get { return _conferenceId; }
            set { _conferenceId = value; }
        }
        public int ConferencePlanId
        {
            get { return _conferencePlanId; }
            set { _conferencePlanId = value; }
        }
        public string ConferenceBeginPeriod 
        {
            get { return _conferenceBeginPeriod.ToShortDateString(); }
            set
            {

                if (value.Contains("/"))
                {
                    CultureInfo culture = new CultureInfo("en-US");
                    _conferenceBeginPeriod = Convert.ToDateTime(value, culture);
                }
                else
                    _conferenceBeginPeriod = Convert.ToDateTime(value);
            }
        }
        public string ConferenceEndPeriod 
        {
            get { return _conferenceEndPeriod.ToShortDateString(); }
            set
            {
                _conferenceEndPeriod = Convert.ToDateTime(value);
            }
        }
        public string PeriodicType
        {
            get { return _periodicType; }
            set
            {
                if (value == "" || value == null)
                {
                    _periodicType = "Еженедельно";
                }
                else
                    _periodicType = value.Trim();
            }
        }
        public string PeriodicValue 
        { 
            get { return _periodicValue; }
            set
            {
                _periodicValue = value.Trim();
            }
        }
        public string ConferenceStartTime 
        { 
            get {

                CultureInfo culture = new CultureInfo("ru-RU");
                return _conferenceStartTime.ToString("HH:mm",culture);
            }
            set
            {
                // проверка формата времени при вводе, время вида ХХ:ХХ
                Regex regex = new Regex("(\\d:[0-5]\\d)|([1]\\d:[0-5]\\d)|([2][0-3]:[0-5]\\d)");
                Match match= regex.Match(value);
                if (match.Success)
                {
                    _conferenceStartTime = Convert.ToDateTime(value);
                }
                else
                {
                    MessageBox.Show("Время введено неверно. Формат для ввода ХХ:ХХ", "Ошибка");
                }

            }
        }
        public string ConferenceStopTime
        {
            get
            {
                CultureInfo culture = new CultureInfo("ru-RU");
                return _conferenceStopTime.ToString("HH:mm", culture);
            }
            set
            {
                Regex regex = new Regex("(\\d:[0-5]\\d)|([1]\\d:[0-5]\\d)|([2][0-3]:[0-5]\\d)");
                Match match = regex.Match(value);
                if (match.Success)
                {
                    _conferenceStopTime = Convert.ToDateTime(value);
                }
                else
                {
                    MessageBox.Show("Время введено неверно. Формат для ввода ХХ:ХХ", "Ошибка");
                }
            }
        }
        public bool IsAcive
        {
            get; set;
        }






    }
}
