﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkingDay
{
    [Serializable]
    class Projekt
    {
        private int ID { get; set; }
        public String strName { get; set; }
        public String strDescription { get; set; }
        public DateTime dtPlannedStart { get; set; }
        public DateTime dtPlannedEnd { get; set; }
        public DateTime dtStart { get; set; }
        public DateTime dtEnd { get; set; }
        
        //Status des Projekts: 0 = steht aus, 1 = gestartet, 2 = angehalten, 3 = abgeschlossen, 4 = abgebrochen
        public int iStatus { get; set; }
        public String strStatusComment { get; set; }

        public Projekt()
        {
            strName = "";
            strDescription = "";
            dtPlannedStart = DateTime.Now;
            dtPlannedEnd = DateTime.Now.AddDays(7);
            dtStart = new DateTime(1970, 1, 1);
            dtEnd = new DateTime(1970, 1, 1);
            iStatus = 0;
            strStatusComment = "";
            ID = this.GetHashCode();
        }

        public Projekt(String name, String description, DateTime plannedStart, DateTime plannedEnd, Boolean bStarten)
        {
            strName = name;
            strDescription = description;
            dtPlannedStart = plannedStart;
            dtPlannedEnd = plannedEnd;
            dtStart = new DateTime(1970, 1, 1);
            dtEnd = new DateTime(1970, 1, 1);

            if (bStarten)
                iStatus = 1;
            else
                iStatus = 0;
            
            strStatusComment = "";
            ID = this.GetHashCode();
        }

        internal int getID()
        {
            return ID;
        }

        internal bool startProject()
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

        internal bool completeProject()
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

        internal bool cancelProject()
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

        internal bool stopProject()
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
