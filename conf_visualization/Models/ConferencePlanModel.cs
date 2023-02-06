using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace conf_visualization.Models
{

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
        //public IList<CheckBox> PeriodicWeeklyValues
        //{
        //    get
        //    {
        //        _periodicValue = "";
        //        for (int i = 0; i < _arrCheckedDayOfWeek.Length; i++)
        //        {
        //            if (_arrCheckedDayOfWeek[i].IsChecked == true)
        //            {
        //                _periodicValue += _arrCheckedDayOfWeek[i].Content + ", ";
        //            }
        //        }
        //        return _arrCheckedDayOfWeek;
        //    }
        //    set { _arrCheckedDayOfWeek = value; }
        //}
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
            set { _periodicType = value; }
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
