using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkingDay
{
    [Serializable]
    class Meilenstein
    {
        String strName { get; set; }
        String strDescription { get; set; }
        DateTime dtPlanned { get; set; }
        DateTime dtAchieved { get; set; }
        Boolean bAchieved { get; set; }
        List<Aufgabe> Aufgaben;

        public Meilenstein()
        {
            strName = "";
            strDescription = "";
            dtPlanned = DateTime.Now;
            dtPlanned.AddMonths(1);
            bAchieved = false;
            Aufgaben = new List<Aufgabe>();
        }

        public Meilenstein(String name, String description, DateTime planned,
            Boolean achieved, List<Aufgabe> aufgaben)
        {
            strName = name;
            strDescription = description;
            dtPlanned = planned;
            if (achieved)
            {
                dtAchieved = planned;
                bAchieved = true;
            }
            else
                bAchieved = false;

            Aufgaben = new List<Aufgabe>(aufgaben);
        }

        internal void setAchieved()
        {
            dtAchieved = DateTime.Now;
            bAchieved = true;
        }
    }
}
