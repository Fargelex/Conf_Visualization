using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization.Models
{
    internal class ConferencePlanModel
    {
        public int ConferenceId { get; set; }
        public int ConferencePlanId { get; set; }
        public DateTime ConferenceBeginPeriod { get; set; }
        public DateTime ConferenceEndPeriod { get; set; }
        public string PeriodicType { get; set; }
        public string PeriodicValue { get; set; }
        public DateTime ConferenceStartTime { get; set; }
        public DateTime ConferenceStopTime { get; set; }
        public bool IsAcive { get; set; }
    }
}
