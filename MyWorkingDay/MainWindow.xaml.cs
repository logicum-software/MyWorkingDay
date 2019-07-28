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

            listBoxTasks.ItemsSource = appData.Aufgaben;
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
                        MessageBox.Show("Eine Aufgabe mit dem Name" + "\n\n" + dlgNewTask.textBoxName.Text + "\n\n" +
                            "existiert bereits.\n\nBitte wählen Sie einen anderen Namen.", "Aufgabe vorhanden", MessageBoxButton.OK);
                        return;
                    }
                }
                appData.Aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                    dlgNewTask.datePickerStart.DisplayDate, dlgNewTask.datePickerEnd.DisplayDate, (Boolean)dlgNewTask.checkBox.IsChecked));
                listBoxTasks.Items.Refresh();
                //saveData();
                MessageBox.Show("Die Aufgabe wurde gespeichert", "Aufgabe gespeichert", MessageBoxButton.OK);
            }
        }
    }
}
