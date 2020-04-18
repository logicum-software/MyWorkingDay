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
        public String strName { get; set; }
        public String strDescription { get; set; }
        public DateTime dtPlannedStart { get; set; }
        public DateTime dtPlannedEnd { get; set; }
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public String strColor { get; set; }
        
        //Status der Augabe: 0 = steht aus, 1 = gestartet, 2 = angehalten, 3 = abgeschlossen, 4 = abgebrochen
        internal int iStatus { get; set; }

        public Aufgabe()
        {
            // Constructs a task and starts it immediately
            strName = "";
            strDescription = "";
            dtPlannedStart = DateTime.Now;
            dtPlannedEnd = DateTime.Now.AddDays(7);
            dtStart = DateTime.Now;
            dtEnd = new DateTime(1970, 1, 1);
            iStatus = 0;

            if (dtPlannedEnd <= DateTime.Now)
                strColor = "Red";
            else
                strColor = "Black";
        }

        public Aufgabe(String name, String description, DateTime plannedStart, DateTime plannedEnd, Boolean bStarten)
        {
            strName = name;
            strDescription = description;
            dtPlannedEnd = plannedEnd;
            dtStart = new DateTime(1970, 1, 1);
            dtEnd = new DateTime(1970, 1, 1);

            if (bStarten)
            {
                dtPlannedStart = DateTime.Now;
                iStatus = 1;
            }
            else
            {
                dtPlannedStart = plannedStart;
                iStatus = 0;
            }
            
            if (dtPlannedEnd <= DateTime.Now)
                strColor = "Red";
            else
                strColor = "Black";
        }

        internal bool startTask()
        {
            if (iStatus == 0 | iStatus == 2)
            {
                iStatus = 1;
                dtStart = DateTime.Now;
                return true;
            }
            else
                return false;
        }

        internal bool completeTask()
        {
            if (iStatus == 1)
            {
                iStatus = 3;
                dtEnd = DateTime.Now;
                return true;
            }
            else
                return false;
        }

        internal bool cancelTask()
        {
            if (iStatus == 0 | iStatus == 1 | iStatus == 2)
            {
                iStatus = 4;
                dtEnd = DateTime.Now;
                return true;
            }
            else
                return false;
        }

        internal bool stopTask()
        {
            if (iStatus == 1)
            {
                iStatus = 2;
                return true;
            }
            else
                return false;
        }
    }
}
