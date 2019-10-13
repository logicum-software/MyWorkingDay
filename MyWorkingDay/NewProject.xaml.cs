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
        private AppData appData;
        private List<Aufgabe> tmpAufgaben;
        public NewProject()
        {
            InitializeComponent();

            appData = new AppData();
            tmpAufgaben = new List<Aufgabe>();

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
            listBox.ItemsSource = tmpAufgaben;
            textBoxName.SelectAll();
            textBoxName.Focus();
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
                tmpAufgaben.Add(appData.Aufgaben[dlgSelection.listBoxSelectTask.SelectedIndex]);
                listBox.Items.Refresh();
            }
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            foreach (Projekt item in appData.Projekte)
            {
                if (String.Compare(item.strName, textBoxName.Text, true) > -1 &&
                    String.Compare(item.strName, textBoxName.Text, true) < 1)
                {
                    MessageBox.Show("Ein Projekt mit dem Namen " + textBoxName.Text +
                        " existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Projekt vorhanden", MessageBoxButton.OK);
                    return;
                }
            }
            appData.Projekte.Add(new Projekt(textBoxName.Text, textBoxDescription.Text,
                datePickerStart.DisplayDate, datePickerEnd.DisplayDate, (Boolean)checkBox.IsChecked));

            foreach (Aufgabe item in tmpAufgaben)
                appData.Projekte[appData.Projekte.Count - 1].Aufgaben.Add(item);

            saveData();
            MessageBox.Show("Das Projekt wurde gespeichert.", "Aufgabe gespeichert", MessageBoxButton.OK);
        }

        private void textBoxName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            if (IsInitialized)
                buttonSave.IsEnabled = true;
        }
    }
}
