using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace conf_visualization.Models
{
    internal class PeriodicWeeklyValuesClass : CheckBox
    {
        private string _shortdayOfWeek;
        public PeriodicWeeklyValuesClass(string dayOfWeek, string shortdayOfWeek)
        {
            this.Content = dayOfWeek;
            this._shortdayOfWeek= shortdayOfWeek;
        }

        public string ShortdayOfWeek 
        {
            get { return this._shortdayOfWeek; }
        }
    }
}
