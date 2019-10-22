using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyScope
{
    [Serializable]
    class AppData
    {
        public BindingList<Aufgabe> aufgaben { get; set; }
        public BindingList<Projekt> projekte { get; set; }

        public AppData()
        {
            aufgaben = new BindingList<Aufgabe>();
            projekte = new BindingList<Projekt>();
        }

        internal Boolean delTask(String name)
        {
            foreach (Aufgabe item in aufgaben)
            {
                if (item.strName.Equals(name))
                {
                    if (aufgaben.Remove(item))
                    {
                        return true;
                    }
                    else
                        return false;
                }
            }
            return false;
        }

        internal Boolean containsTask(String name)
        {
            foreach (Aufgabe item in aufgaben)
            {
                if (item.strName.Equals(name))
                    return true;
            }
            return false;
        }
    }
}
