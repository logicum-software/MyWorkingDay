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
    /// Interaktionslogik für NewTask.xaml
    /// </summary>
    public partial class NewTask : Window
    {
        private List<Aufgabe> taskList;
        public NewTask()
        {
            InitializeComponent();

            if (taskList == null)
                taskList = new List<Aufgabe>();

            textBoxName.SelectAll();
            textBoxName.Focus();
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



        internal void SetTaskList(List<Aufgabe> tasks)
        {
            taskList = new List<Aufgabe>(tasks);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void TextBoxDescription_GotFocus(object sender, RoutedEventArgs e)
        {
            textBoxDescription.SelectAll();
        }

        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            foreach (Aufgabe item in taskList)
            {
                if (String.Compare(item.strName, textBoxName.Text, true) > -1 &&
                    String.Compare(item.strName, textBoxName.Text, true) < 1)
                {
                    MessageBox.Show("Eine Aufgabe mit dem Namen " + textBoxName.Text +
                        " existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Aufgabe vorhanden", MessageBoxButton.OK);
                    textBoxName.SelectAll();
                    textBoxName.Focus();
                    return;
                }
            }
            DialogResult = true;
            Close();
        }

        private void textBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
