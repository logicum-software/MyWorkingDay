using System.Collections.Generic;
using System.Windows;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für NewProject.xaml
    /// </summary>
    public partial class NewProject : Window
    {
        private AppData appData;
        internal NewProject(AppData tmpAppData)
        {
            InitializeComponent();

            textBoxName.SelectAll();
            textBoxName.Focus();
            appData = new AppData(tmpAppData);
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void ButtonNewTask_Click(object sender, RoutedEventArgs e)
        {
            TaskSelection dlgSelection = new TaskSelection();
            
            dlgSelection.tmpAufgaben = new List<Aufgabe>(appData);
            dlgSelection.listBoxSelectTask.ItemsSource = dlgSelection.tmpAufgaben;
            dlgSelection.ShowDialog();

            if (dlgSelection.DialogResult == true)
            {
                tmpAufgaben.Add(dlgSelection.tmpAufgaben[dlgSelection.listBoxSelectTask.SelectedIndex]);
            }
        }
    }
}
