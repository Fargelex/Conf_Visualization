using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace conf_visualization.Models
{
    internal class ConferenceModel
    {
        public int ConferenceId { get; set; }
        public string ConferenceName { get; set; }
        public int ParticipantsCount { get; set; }
        public int ConferenceDuration { get; set; }
        public bool IsAcive { get; set; }
    }
}
