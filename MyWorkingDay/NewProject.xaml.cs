using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
        internal List<Aufgabe> ProjectTasksList;
        internal List<Meilenstein> MilestonesList;
        private AppData appData;

        public NewProject()
        {
            InitializeComponent();

            LoadData();

            if (ProjectTasksList == null)
                ProjectTasksList = new List<Aufgabe>();

            if (MilestonesList == null)
                MilestonesList = new List<Meilenstein>();

            listBoxTasks.ItemsSource = ProjectTasksList;
            textBoxName.SelectAll();
            textBoxName.Focus();
        }

        private void LoadData()
        {
            //Daten einlesen aus Datei "udata.dat"
            IFormatter formatter = new BinaryFormatter();
            try
            {
                Stream stream = new FileStream("udata.dat", FileMode.Open, FileAccess.Read, FileShare.Read);
                appData = (AppData)formatter.Deserialize(stream);
                stream.Close();
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(e.Message, "Dateifehler", MessageBoxButton.OK);
                //throw;
            }
        }

        private void SaveData()
        {
            FileStream fs = new FileStream("udata.dat", FileMode.Create);

            // Construct a BinaryFormatter and use it to serialize the data to the stream.
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, appData);
            }
            catch (SerializationException ec)
            {
                MessageBox.Show(ec.Message, "Speicherfehler", MessageBoxButton.OK);
                //Console.WriteLine("Failed to serialize. Reason: " + ec.Message);
                throw;
            }
            finally
            {
                fs.Close();
            }
        }

        internal void setName(String name)
        {
            textBoxName.Text = name;
        }

        internal void setDescription(String description)
        {
            textBoxDescription.Text = description;
        }

        internal void setPlannedStart(DateTime plannedStart)
        {
            datePickerStart.SelectedDate = plannedStart;
        }

        internal void setPlannedEnd(DateTime plannedEnd)
        {
            datePickerEnd.SelectedDate = plannedEnd;
        }

        internal void setIsStarted(Boolean isStarted)
        {
            checkBox.IsChecked = isStarted;
        }

        internal List<Aufgabe> getProjectTaskList()
        {
            return ProjectTasksList;
        }

        internal void addProjectTask(Aufgabe aufgabe)
        {
            ProjectTasksList.Add(aufgabe);
        }

        internal Projekt GetProjekt()
        {
            return new Projekt(textBoxName.Text, textBoxDescription.Text, datePickerStart.DisplayDate,
                datePickerEnd.DisplayDate, (bool) checkBox.IsChecked);
        }

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
                LoadData();
                // muss noch geändert werden für neu erstellte Aufgaben
                foreach (object item in dlgSelection.listBoxSelectTask.SelectedItems)
                    ProjectTasksList.Add(appData.Aufgaben[dlgSelection.listBoxSelectTask.Items.IndexOf(item)]);

                listBoxTasks.Items.Refresh();
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Projekt item in appData.Projekte)
            {
                if (String.Compare(item.strName, textBoxName.Text, true) > -1 &&
                    String.Compare(item.strName, textBoxName.Text, true) < 1)
                {
                    if (MessageBox.Show("Möchten sie das Projekt mit dem Namen " + textBoxName.Text +
                        " wirklich überschreiben?", "Projekt überschreiben", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    {
                        textBoxName.SelectAll();
                        textBoxName.Focus();
                        return;
                    }
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