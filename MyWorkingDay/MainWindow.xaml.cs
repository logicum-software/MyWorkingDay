﻿using System;
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

            dlgNewTask.taskList = appData.Aufgaben;

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
            {
                buttonDelTask.IsEnabled = false;
                textBoxName.IsEnabled = false;
                textBoxDescription.IsEnabled = false;
                dateStart.IsEnabled = false;
                dateEnd.IsEnabled = false;
            }
            else
            {
                buttonDelTask.IsEnabled = true;
                textBoxName.IsEnabled = true;
                textBoxDescription.IsEnabled = true;
                dateStart.IsEnabled = true;
                dateEnd.IsEnabled = true;
                textBoxName.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].strName;
                textBoxDescription.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].strDescription;
                dateStart.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedStart.ToString();
                dateEnd.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedEnd.ToString();
            }
            buttonSave.IsEnabled = false;
        }

        private void ButtonProjectNew_Click(object sender, RoutedEventArgs e)
        {
            NewProject dlgNewProject = new NewProject();

            dlgNewProject.AllTasksList = new List<Aufgabe>(appData.Aufgaben);
            dlgNewProject.ProjectsList = new List<Projekt>(appData.Projekte);

            dlgNewProject.ShowDialog();
            
            if (dlgNewProject.DialogResult.HasValue && dlgNewProject.DialogResult.Value == true)
            {
                appData.Projekte.Add(dlgNewProject.GetProjekt());
                SaveData();
                MessageBox.Show("Das Projekt wurde gespeichert", "Projekt gespeichert", MessageBoxButton.OK);
            }
            RefreshListBoxes();
        }

        private void ButtonDelProject_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsInitialized && textBoxName.IsFocused)
                buttonSave.IsEnabled = true;
        }

        private void TextBoxDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (this.IsInitialized && textBoxDescription.IsFocused)
                buttonSave.IsEnabled = true;
        }

        private void DateStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
                buttonSave.IsEnabled = true;
        }

        private void DateEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.IsInitialized)
                buttonSave.IsEnabled = true;
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            if (appData.containsTask(textBoxName.Text))
            {
                if (MessageBox.Show("Eine Aufgabe mit diesem Namen existiert bereits. Soll sie überschrieben werden?",
                    "Aufgabe vorhanden", MessageBoxButton.YesNo) == MessageBoxResult.No)
                    return;
                else
                {
                    appData.Aufgaben[listBoxTasks.SelectedIndex].strName = textBoxName.Text;
                    appData.Aufgaben[listBoxTasks.SelectedIndex].strDescription = textBoxDescription.Text;
                    appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedStart = dateStart.DisplayDate;
                    appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedEnd = dateEnd.DisplayDate;
                    SaveData();
                    buttonSave.IsEnabled = false;
                    MessageBox.Show("Die Änderungen wurden gespeichert.", "Änderungen gespeichert",
                        MessageBoxButton.OK);
                }
            }
        }
    }
}
