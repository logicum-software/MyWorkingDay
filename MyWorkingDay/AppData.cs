using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWorkingDay
{
    [Serializable]
    class AppData
    {
        public List<Aufgabe> Aufgaben { get; set; }

        public AppData()
        {
            Aufgaben = new List<Aufgabe>();
        }
    }
}
