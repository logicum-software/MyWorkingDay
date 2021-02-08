using System;

namespace MyWorkingDay
{
    [Serializable]
    class Aufgabe
    {
        private int ID { get; set; }
        public String strName { get; set; }
        public String strDescription { get; set; }
        public DateTime dtPlannedStart { get; set; }
        public DateTime dtPlannedEnd { get; set; }
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        public String strColor { get; set; }
        public int ProjectID { get; set; }
        public String strProject { get; set; }
        public String strDisplayPlannedEnd { get; set; }
        
        //Status der Augabe: 0 = ausstehend, 1 = gestartet, 2 = angehalten, 3 = abgeschlossen, 4 = abgebrochen
        public int iStatus { get; set; }

        public String strStatusComment { get; set; }

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
            strStatusComment = "ausstehend";

            if (dtPlannedEnd <= DateTime.Now)
                strColor = "Red";
            else
                strColor = "Black";

            ProjectID = 0;
            strProject = "ohne";
            strDisplayPlannedEnd = dtPlannedEnd.ToString("dd.MM.yyyy");
            ID = this.GetHashCode();
        }

        public Aufgabe(String name, String description, DateTime plannedStart, 
            DateTime plannedEnd, Boolean bStarten, Projekt project)
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
                strStatusComment = "gestartet";
            }
            else
            {
                dtPlannedStart = plannedStart;
                iStatus = 0;
                strStatusComment = "ausstehend";
            }
            
            if (dtPlannedEnd <= DateTime.Now)
                strColor = "Red";
            else
                strColor = "Black";

            if (project == null)
            {
                ProjectID = 0;
                strProject = "ohne";
            }
            else
            {
                ProjectID = project.getID();
                strProject = project.strName;
            }

            strDisplayPlannedEnd = dtPlannedEnd.ToString("dd.MM.yyyy");
            ID = this.GetHashCode();
        }

        internal int getID()
        {
            return ID;
        }

        internal bool startTask()
        {
            if (iStatus == 0 | iStatus == 2)
            {
                iStatus = 1;
                strStatusComment = "gestartet";
                dtStart = DateTime.Now;
                return true;
            }
            else
                return false;
        }

        internal bool completeTask()
        {
            if (iStatus != 3)
            {
                iStatus = 3;
                strStatusComment = "abgeschlossen";
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
                strStatusComment = "abgebrochen";
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
                strStatusComment = "angehalten";
                return true;
            }
            else
                return false;
        }
    }
}
