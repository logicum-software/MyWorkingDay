using System;
using System.Collections.Generic;
using System.Windows;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für TaskSelection.xaml
    /// </summary>
    public partial class TaskSelection : Window
    {
        internal List<Aufgabe> tmpAufgaben;

        public TaskSelection()
        {
            InitializeComponent();

            //listBoxSelectTask.ItemsSource = tmpAufgaben;
        }

        internal void setAufgaben(List<Aufgabe> tmpList)
        {
            foreach (Aufgabe item in tmpList)
                tmpAufgaben.Add(item);

            //tmpAufgaben = new List<Aufgabe>(tmpList);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ButtonTaskNew_Click(object sender, RoutedEventArgs e)
        {
            NewTask dlgNewTask = new NewTask();

            dlgNewTask.ShowDialog();

            if (dlgNewTask.DialogResult.HasValue && dlgNewTask.DialogResult.Value == true)
            {
                foreach (Aufgabe item in tmpAufgaben)
                {
                    if (String.Compare(item.strName, dlgNewTask.textBoxName.Text, true) > -1 &&
                        String.Compare(item.strName, dlgNewTask.textBoxName.Text, true) < 1)
                    {
                        MessageBox.Show("Eine Aufgabe mit dem Namen " + dlgNewTask.textBoxName.Text +
                            " existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Aufgabe vorhanden", MessageBoxButton.OK);
                        return;
                    }
                }
                /*appData.Aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                    dlgNewTask.datePickerStart.DisplayDate, dlgNewTask.datePickerEnd.DisplayDate, (Boolean)dlgNewTask.checkBox.IsChecked));
                saveData();
                listBoxTasks.Items.Refresh();*/
                //saveData();
                MessageBox.Show("Die Aufgabe wurde gespeichert", "Aufgabe gespeichert", MessageBoxButton.OK);
            }
        }
    }
}
