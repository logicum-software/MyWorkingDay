﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            listBoxTasks.ItemsSource = appData.Aufgaben;
            listBoxProjects.ItemsSource = appData.Projekte;
        }

        private void saveData()
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

        private void ButtonTaskNew_Click(object sender, RoutedEventArgs e)
        {
            NewTask dlgNewTask = new NewTask();

            dlgNewTask.ShowDialog();

            if (dlgNewTask.DialogResult.HasValue && dlgNewTask.DialogResult.Value == true)
            {
                foreach (Aufgabe item in appData.Aufgaben)
                {
                    if (String.Compare(item.strName, dlgNewTask.textBoxName.Text, true) > -1 &&
                        String.Compare(item.strName, dlgNewTask.textBoxName.Text, true) < 1)
                    {
                        MessageBox.Show("Eine Aufgabe mit dem Namen " + dlgNewTask.textBoxName.Text + 
                            " existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Aufgabe vorhanden", MessageBoxButton.OK);
                        return;
                    }
                }
                appData.Aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                    dlgNewTask.datePickerStart.DisplayDate, dlgNewTask.datePickerEnd.DisplayDate, (Boolean)dlgNewTask.checkBox.IsChecked));
                saveData();
                listBoxTasks.Items.Refresh();
                //saveData();
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
                        saveData();
                        listBoxTasks.Items.Refresh();
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
            }
            else
            {
                buttonDelTask.IsEnabled = true;
                textBoxName.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].strName;
                textBoxDescription.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].strDescription;
                dateStart.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedStart.ToString();
                dateEnd.Text = appData.Aufgaben[listBoxTasks.SelectedIndex].dtPlannedEnd.ToString();
            }
        }

        private void ButtonProjectNew_Click(object sender, RoutedEventArgs e)
        {
            NewProject dlgNewProject = new NewProject();

            dlgNewProject.ShowDialog();
        }

        private void ButtonDelProject_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
