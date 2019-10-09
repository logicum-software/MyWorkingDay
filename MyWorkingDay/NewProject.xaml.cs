using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für NewProject.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        internal List<Aufgabe> tmpAufgaben;

        public NewProject()
        {
            InitializeComponent();

            textBoxName.SelectAll();
            textBoxName.Focus();
        }

        internal void setAufgaben(List<Aufgabe> tmpList)
        {
            foreach (Aufgabe item in tmpList)
                tmpAufgaben.Add(item);

            //tmpAufgaben = new List<Aufgabe>(tmpList);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            TaskSelection dlgSelection = new TaskSelection();
            
            dlgSelection.tmpAufgaben = new List<Aufgabe>(tmpAufgaben);
            dlgSelection.listBoxSelectTask.ItemsSource = dlgSelection.tmpAufgaben;
            dlgSelection.ShowDialog();
        }
    }
}
