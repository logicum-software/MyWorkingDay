using System.Collections.Generic;
using System.Windows;

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
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            TaskSelection dlgSelection = new TaskSelection();
            
            dlgSelection.tmpAufgaben = new List<Aufgabe>(tmpAufgaben);
            dlgSelection.listBoxSelectTask.ItemsSource = dlgSelection.tmpAufgaben;
            dlgSelection.ShowDialog();

            if (dlgSelection.DialogResult == true)
            {
                tmpAufgaben.Add(dlgSelection.tmpAufgaben[dlgSelection.listBoxSelectTask.SelectedIndex]);
            }
        }
    }
}
