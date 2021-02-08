using System;
using System.Collections.Generic;
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
        
        internal Boolean delTask(int ID)
        {
            foreach (Aufgabe item in Aufgaben)
            {
                if (item.getID() == ID)
                {
                    if (Aufgaben.Remove(item))
                        return true;
                    else
                        return false;
                }
            }
            return false;
        }

        // Obsolete - wird über die Klasse Aufgabe gemacht
        /*internal bool completeTask(Aufgabe task)
        {
            if (task.iStatus != 3)
            {
                task.iStatus = 3;
                task.dtEnd = DateTime.Now;
                return true;
            }
            else
            {
                if (MessageBox.Show("Die Aufgabe wurde bereits abgeschlossen. Möchten Sie das Abschlußdatum ändern?", "Bereits abgeschlossen", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    task.dtEnd = DateTime.Now;
                    return true;
                }
                else
                    return false;
            }
        }*/

        internal bool cancelTask(Aufgabe task)
        {
            if (task.iStatus != 4)
            {
                task.iStatus = 4;
                return true;
            }
            else
                return false;
        }

        internal bool startTask(Aufgabe task)
        {
            if (task.iStatus == 0 || task.iStatus == 2)
            {
                task.iStatus = 1;
                return true;
            }
            else
                return false;
        }

        internal Boolean containsTask(String name)
        {
            foreach (Aufgabe item in Aufgaben)
            {
                if (item.strName.Equals(name))
                    return true;
            }
            return false;
        }

        internal Boolean delProject(String name)
        {
            foreach (Projekt item in Projekte)
            {
                if (item.strName.Equals(name))
                {
                    if (Projekte.Remove(item))
                        return true;
                    else
                        return false;

                }
            }
            return false;
        }

        internal Boolean containsProject(String name)
        {
            foreach (Projekt item in Projekte)
            {
                if (item.strName.Equals(name))
                    return true;
            }
            return false;
        }
    }
}
