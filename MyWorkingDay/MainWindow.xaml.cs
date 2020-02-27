using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppData appData;
        public MainWindow()
        {
            InitializeComponent();

            appData = new AppData();

            LoadData();

            listBoxTasks.ItemsSource = appData.Aufgaben;
            listBoxProjects.ItemsSource = appData.Projekte;
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

        private void RefreshListBoxes()
        {
            listBoxTasks.Items.Refresh();
            listBoxProjects.Items.Refresh();
        }

        private void ButtonTaskNew_Click(object sender, RoutedEventArgs e)
        {
            NewTask dlgNewTask = new NewTask();

            dlgNewTask.SetTaskList(appData.Aufgaben);

            dlgNewTask.ShowDialog();

            if (dlgNewTask.DialogResult.HasValue && dlgNewTask.DialogResult.Value == true)
            {
                appData.Aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                    dlgNewTask.datePickerStart.DisplayDate, dlgNewTask.datePickerEnd.DisplayDate, (Boolean)dlgNewTask.checkBox.IsChecked));
                SaveData();
                RefreshListBoxes();
                MessageBox.Show("Die Aufgabe wurde gespeichert", "Aufgabe gespeichert", MessageBoxButton.OK);
            }
        }

        private void ButtonDelTask_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxTasks.SelectedItem != null)
            {
                if (MessageBox.Show("Möchten Sie die Aufgabe wirklich löschen?", "Aufgabe löschen", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
                else
                {
                    if (appData.delTask(appData.Aufgaben[listBoxTasks.SelectedIndex].strName))
                    {
                        SaveData();
                        RefreshListBoxes();
                        MessageBox.Show("Die Aufgabe wurde gelöscht.", "Aufgabe gelöscht", MessageBoxButton.OK);
                    }
                    else
                        MessageBox.Show("Die Aufgabe konnte nicht gelöscht werden.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                }
            }
        }

        private void ListBoxTasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxTasks.SelectedItem == null)
                buttonDelTask.IsEnabled = false;
            else
                buttonDelTask.IsEnabled = true;
        }

        private void ButtonProjectNew_Click(object sender, RoutedEventArgs e)
        {
            NewProject dlgNewProject = new NewProject();

            dlgNewProject.ShowDialog();
            
            if (dlgNewProject.DialogResult.HasValue && dlgNewProject.DialogResult.Value == true)
            {
                appData.Projekte.Add(dlgNewProject.GetProjekt());
                SaveData();
                LoadData();
                RefreshListBoxes();
                MessageBox.Show("Das Projekt wurde gespeichert", "Projekt gespeichert", MessageBoxButton.OK);
            }
        }

        private void ButtonDelProject_Click(object sender, RoutedEventArgs e)
        {
            if (listBoxProjects.SelectedItem != null)
            {
                if (MessageBox.Show("Möchten Sie das Projekt wirklich löschen?", "Projekt löschen", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
                else
                {
                    if (appData.delProject(appData.Projekte[listBoxProjects.SelectedIndex].strName))
                    {
                        SaveData();
                        RefreshListBoxes();
                        MessageBox.Show("Das Projekt wurde gelöscht.", "Projekt gelöscht", MessageBoxButton.OK);
                    }
                    else
                        MessageBox.Show("Das Projekt konnte nicht gelöscht werden.", "Löschen fehlgeschlagen", MessageBoxButton.OK);
                }
            }
        }

        private void listBoxProjects_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listBoxProjects.SelectedItem == null)
                buttonDelProject.IsEnabled = false;
            else
                buttonDelProject.IsEnabled = true;
        }

        private void listBoxTasks_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NewTask dlgEditTask = new NewTask();

            dlgEditTask.setName(appData.Aufgaben[listBoxTasks.SelectedIndex].strName);
            dlgEditTask.setDescription(appData.Aufgaben[listBoxTasks.SelectedIndex].strDescription);
            dlgEditTask.setPlannedStart(appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedStart);
            dlgEditTask.setPlannedEnd(appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedEnd);
            dlgEditTask.setIsStarted(false);
            
            
            dlgEditTask.SetTaskList(appData.Aufgaben);
            dlgEditTask.Title = "Aufgabe bearbeiten";

            dlgEditTask.ShowDialog();
            //MessageBox.Show(appData.Aufgaben[listBoxTasks.SelectedIndex].strName, "Doppelklick auf...", MessageBoxButton.OK);
        }

        private void listBoxProjects_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            MessageBox.Show(appData.Projekte[listBoxProjects.SelectedIndex].strName, "Doppelklick auf...", MessageBoxButton.OK);
        }
    }
}
