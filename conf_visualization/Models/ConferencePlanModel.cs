using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace conf_visualization.Models
{

    internal class ConferencePlanModel : INotifyPropertyChanged
    {
        private int _conferenceId;
        private int _conferencePlanId;
        private DateTime _conferenceBeginPeriod = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 00, 00, 00);
        private DateTime _conferenceEndPeriod = new DateTime(DateTime.Now.Year + 1, 1, 10, 00, 01, 00);
        private string _periodicType = "Еженедельно";
        private string _periodicValue;
        private DateTime _conferenceStartTime = new DateTime(2022, 12, 30, 00, 00, 00);
        private DateTime _conferenceStopTime = new DateTime(2022, 12, 31, 00, 00, 00);
        private bool _changedValue = false;
        private bool _newValue = false;
        private bool _hasError = false;
        private string _errorToolTip;

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        // private PeriodicWeeklyValuesClass[] _arrCheckedDayOfWeek =
        //{
        //     new PeriodicWeeklyValuesClass("Ежедневно", "Ежедневно"),
        //   new PeriodicWeeklyValuesClass("Понедельник", "Пн"),
        //     new PeriodicWeeklyValuesClass("Вторник", "Вт"),
        //     new PeriodicWeeklyValuesClass("Среда", "Ср"),
        //     new PeriodicWeeklyValuesClass("Четверг", "Чт"),
        //     new PeriodicWeeklyValuesClass("Пятница", "Пт"),
        //     new PeriodicWeeklyValuesClass("Суббота", "Сб"),
        //     new PeriodicWeeklyValuesClass("Воскресение", "Вс"),
        // };


        private List<PeriodicWeeklyValuesClass> _arrCheckedDayOfWeek = new List<PeriodicWeeklyValuesClass>()
        {
            new PeriodicWeeklyValuesClass("Ежедневно", "Ежедневно"),
            new PeriodicWeeklyValuesClass("Понедельник", "Пн"),
            new PeriodicWeeklyValuesClass("Вторник", "Вт"),
            new PeriodicWeeklyValuesClass("Среда", "Ср"),
            new PeriodicWeeklyValuesClass("Четверг", "Чт"),
            new PeriodicWeeklyValuesClass("Пятница", "Пт"),
            new PeriodicWeeklyValuesClass("Суббота", "Сб"),
            new PeriodicWeeklyValuesClass("Воскресение", "Вс")
        };

        public IList<PeriodicWeeklyValuesClass> PeriodicTypesWeeklyListInModel
        {
            get { return _arrCheckedDayOfWeek; }
            set { }
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
                if (_newValue)
                {
                    ChangedValue = true;
                }
                OnPropertyChanged();
            }
        }

        public string ErrorToolTip
        {
            get { return _errorToolTip; }
            set
            {
                _errorToolTip = value;
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
        public int ConferencePlanId
        {
            get { return _conferencePlanId; }
            set 
            {
                if (_conferencePlanId != value)
                {
                    _conferencePlanId = value;
                    _changedValue = true;
                    OnPropertyChanged();
                }
            }
        }
        public string ConferenceBeginPeriod 
        {
            get { return _conferenceBeginPeriod.ToShortDateString(); }
            set
            {

                //if (value.Contains("/"))
                //{
                //    CultureInfo culture = new CultureInfo("en-US");
                //    _conferenceBeginPeriod = Convert.ToDateTime(value, culture);
                //}
                //else
                    _conferenceBeginPeriod = Convert.ToDateTime(value);
                _changedValue = true;
                OnPropertyChanged();
            }
        }
        public string ConferenceEndPeriod 
        {
            get { return _conferenceEndPeriod.ToShortDateString(); }
            set
            {
                DateTime vlbegin = _conferenceBeginPeriod;
                DateTime vlend = _conferenceEndPeriod;
                //if (value.Contains("/"))
                //{
                //    CultureInfo culture = new CultureInfo("en-US");
                //    vlend = Convert.ToDateTime(value, culture);
                //}
                //else

                    vlend = Convert.ToDateTime(value);
                _changedValue = true;

                if (vlend > vlbegin)
                {
                    _conferenceEndPeriod = vlend;
                    OnPropertyChanged();
                }
                else
                {
                    ErrorToolTip = "Конец периода должен быть больше начала периода";
                    hasError = true;
                }


            }
        }
        public string PeriodicType
        {
            get { return _periodicType; }
            set 
            {
                if (_periodicType != value)
                {
                    _periodicType = value;
                    _changedValue = true;
                    OnPropertyChanged();
                }
            }
        }
        
        public string PeriodicValue 
        { 
            get {
                _periodicValue = "";
                for (int i = 0; i < _arrCheckedDayOfWeek.Count; i++)
                {
                    if (_arrCheckedDayOfWeek[i].IsChecked == true)
                    { 
                        _periodicValue += _arrCheckedDayOfWeek[i].ShortdayOfWeek + ", ";
                    }
                }
                if (_periodicValue.Length > 2)
                    _periodicValue = _periodicValue.Remove(_periodicValue.Length - 2, 2);
                return _periodicValue;
            }
            set
            {
                string out_value = "";
                if (value.Contains("Пн"))
                {
                    _arrCheckedDayOfWeek[1].IsChecked = true;
                    out_value += "Пн, ";
                }
                if (value.Contains("Вт"))
                {
                    _arrCheckedDayOfWeek[2].IsChecked = true;
                    out_value += "Вт, ";
                }
                if (value.Contains("Ср"))
                {
                    _arrCheckedDayOfWeek[3].IsChecked = true;
                    out_value += "Ср, ";
                }
                if (value.Contains("Чт"))
                {
                    _arrCheckedDayOfWeek[4].IsChecked = true;
                    out_value += "Чт, ";
                }
                if (value.Contains("Пт"))
                {
                    _arrCheckedDayOfWeek[5].IsChecked = true;
                    out_value += "Пт, ";
                }
                if (value.Contains("Сб"))
                {
                    _arrCheckedDayOfWeek[6].IsChecked = true;
                    out_value += "Сб, ";
                }
                if (value.Contains("Вс"))
                {
                    _arrCheckedDayOfWeek[7].IsChecked = true;
                    out_value += "Вс, ";
                }
                if (out_value.Length > 2)
                    out_value = out_value.Remove(out_value.Length - 2, 2);
                _periodicValue = out_value;
                _changedValue = true;
                OnPropertyChanged();
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
                    _changedValue = true;
                    OnPropertyChanged();
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
                    _changedValue = true;
                    OnPropertyChanged();
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
