using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für NewProject.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        internal List<Aufgabe> ProjectTasksList, AllTasksList;
        internal List<Projekt> ProjectsList;
        public NewProject()
        {
            InitializeComponent();

            if (ProjectTasksList == null)
                ProjectTasksList = new List<Aufgabe>();

            if (AllTasksList == null)
                AllTasksList = new List<Aufgabe>();

            if (ProjectsList == null)
                ProjectsList = new List<Projekt>();

            listBoxTasks.ItemsSource = ProjectTasksList;
            textBoxName.SelectAll();
            textBoxName.Focus();
        }

        /*internal Projekt GetProjekt()
        {
            return new Projekt(textBoxName.Text, textBoxDescription.Text, datePickerStart.DisplayDate,
                datePickerEnd.DisplayDate, ProjectTasksList, false); //false muss ersetzt werden durch IsChecked
        }*/

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            TaskSelection dlgSelection = new TaskSelection();
            
            dlgSelection.ShowDialog();
            if (dlgSelection.DialogResult == true)
            {
                // muss noch geändert werden für neu erstellte Aufgaben
                foreach (object item in dlgSelection.listBoxSelectTask.SelectedItems)
                {
                    ProjectTasksList.Add(AllTasksList[dlgSelection.listBoxSelectTask.Items.IndexOf(item)]);
                }
                
                //ProjectTasksList.Add(AllTasksList[dlgSelection.listBoxSelectTask.SelectedIndex]);
                listBoxTasks.Items.Refresh();
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Projekt item in ProjectsList)
            {
                if (String.Compare(item.strName, textBoxName.Text, true) > -1 &&
                    String.Compare(item.strName, textBoxName.Text, true) < 1)
                {
                    MessageBox.Show("Ein Projekt mit dem Namen " + textBoxName.Text +
                        " existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Projekt vorhanden", MessageBoxButton.OK);
                    textBoxName.SelectAll();
                    textBoxName.Focus();
                    return;
                }
            }
            DialogResult = true;
            Close();
        }

        private void buttonNewMilestone_Click(object sender, RoutedEventArgs e)
        {
            NewMilestone dlgNewMilestone = new NewMilestone();

            dlgNewMilestone.ShowDialog();
        }

        private void textBoxName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (IsInitialized)
                buttonSave.IsEnabled = true;
        }
    }
}