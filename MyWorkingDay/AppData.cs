using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyWorkingDay
{
    [Serializable]
    class AppData
    {
        public List<Aufgabe> Aufgaben { get; set; }
        public List<Projekt> Projekte { get; set; }

        public AppData()
        {
            Aufgaben = new List<Aufgabe>();
            Projekte = new List<Projekt>();
        }

        public Boolean delTask(String name)
        {
            foreach (Aufgabe item in Aufgaben)
            {
                if (item.strName.Equals(name))
                {
                    if (Aufgaben.Remove(item))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }
    }
}
