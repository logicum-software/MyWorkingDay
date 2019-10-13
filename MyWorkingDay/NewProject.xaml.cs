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
        public NewProject()
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
                listBox.Items.Add(dlgSelection.listBoxSelectTask.SelectedIndex)
            }
        }
    }
}
