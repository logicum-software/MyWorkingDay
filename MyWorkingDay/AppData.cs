using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MyWorkingDay
{
    [Serializable]
    class AppData// : System.ComponentModel.INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;
        public List<Aufgabe> Aufgaben { get; set; }
        public List<Projekt> Projekte { get; set; }
         
        public AppData()
        {
            Aufgaben = new List<Aufgabe>();
            Projekte = new List<Projekt>();
        }

        /*protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }*/

        internal Boolean delTask(String name)
        {
            foreach (Aufgabe item in Aufgaben)
            {
                if (item.strName.Equals(name))
                {
                    if (Aufgaben.Remove(item))
                        return true;
                    else
                        return false;
                }
            }
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
