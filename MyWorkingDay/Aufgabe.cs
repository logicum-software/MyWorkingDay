using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkingDay
{
    [Serializable]
    class Aufgabe
    {
        private String strName { get; set; }
        private DateTime dtPlannedStart { get; set; }
        private DateTime dtPlannedEnd { get; set; }
        private DateTime dtStart { get; set; }
        private DateTime dtEnd { get; set; }

        public Aufgabe()
        {
            strName = "";
            dtPlannedStart = DateTime.Now;
            dtPlannedEnd = DateTime.Now.AddDays(7);
        }

        public Aufgabe(String name, DateTime plannedStart, DateTime plannedEnd)
        {
            strName = name;
            dtPlannedStart = plannedStart;
            dtPlannedEnd = plannedEnd;
        }
    }
}
