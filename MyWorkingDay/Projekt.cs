using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkingDay
{
    [Serializable]
    class Projekt
    {
        public String strName { get; set; }
        public String strDescription { get; set; }
        public DateTime dtPlannedStart { get; set; }
        public DateTime dtPlannedEnd { get; set; }
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public List<Aufgabe> Aufgaben { get; set; }
        
        //Status des Projekts: 0 = steht aus, 1 = gestartet, 2 = angehalten, 3 = abgeschlossen, 4 = abgebrochen
        private int iStatus { get; set; }

        public Projekt()
        {
            strName = "";
            strDescription = "";
            dtPlannedStart = DateTime.Now;
            dtPlannedEnd = DateTime.Now.AddDays(7);
            dtStart = new DateTime(1970, 1, 1);
            dtEnd = new DateTime(1970, 1, 1);
            iStatus = 0;
            Aufgaben = new List<Aufgabe>();
        }

        public Projekt(String name, String description, DateTime plannedStart, DateTime plannedEnd,
            List<Aufgabe> aufgaben, Boolean bStarten)
        {
            strName = name;
            strDescription = description;
            dtPlannedStart = plannedStart;
            dtPlannedEnd = plannedEnd;
            dtStart = new DateTime(1970, 1, 1);
            dtEnd = new DateTime(1970, 1, 1);
            Aufgaben = new List<Aufgabe>(aufgaben);

            if (bStarten)
                iStatus = 1;
            else
                iStatus = 0;
        }
    }
}
