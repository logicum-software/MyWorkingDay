using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace MyWorkingDay
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private AppData appData;
        private List<Aufgabe> dueTasks;
        private DispatcherTimer dispatcherTimer;
        private BitmapImage biGreen;
        private BitmapImage biRed;

        public MainWindow()
        {
            InitializeComponent();

            appData = new AppData();

            LoadData();

            dueTasks = new List<Aufgabe>(appData.Aufgaben);
            
            listBoxTasks.ItemsSource = appData.Aufgaben;
            listBoxProjects.ItemsSource = appData.Projekte;
            listViewDue.ItemsSource = dueTasks;

            // Initialize Status-Images
            biGreen = new BitmapImage();
            biGreen.BeginInit();
            biGreen.UriSource = new Uri("green_check_small.jpg", UriKind.Relative);
            biGreen.EndInit();

            biRed = new BitmapImage();
            biRed.BeginInit();
            biRed.UriSource = new Uri("red_check_small.jpg", UriKind.Relative);
            biRed.EndInit();

            image.Stretch = Stretch.Fill;
            image.Source = biGreen;

            //  DispatcherTimer setup
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 5);
            dispatcherTimer.Start();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            int iDueCount = 0;

            foreach (Aufgabe item in appData.Aufgaben)
            {
                if (DateTime.Now >= item.dtPlannedEnd)
                {
                    item.strColor = "Red";
                    iDueCount++;
                }
                else
                    item.strColor = "Black";
            }

            if (iDueCount > 0)
            {
                labelStatus.Content = "Sie haben " + iDueCount.ToString() + " überfällige Aufgabe(n).";
                image.Source = biRed;
            }
            else
            {
                labelStatus.Content = "Sie haben keine überfälligen Aufgaben.";
                image.Source = biGreen;
            }

            RefreshListBoxes();
            //MessageBox.Show("Timer ausgelöst.", "Timer", MessageBoxButton.OK);
            //throw new NotImplementedException();
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
            dueTasks = new List<Aufgabe>(appData.Aufgaben);
            listViewDue.ItemsSource = dueTasks;
            listBoxTasks.Items.Refresh();
            listBoxProjects.Items.Refresh();
            dueTasks.Sort((x, y) => DateTime.Compare(x.dtPlannedEnd, y.dtPlannedEnd));
            listViewDue.Items.Refresh();
        }

        private void ButtonTaskNew_Click(object sender, RoutedEventArgs e)
        {
            NewTask dlgNewTask = new NewTask();

            dlgNewTask.SetTaskList(appData.Aufgaben);

            dlgNewTask.ShowDialog();

            if (dlgNewTask.DialogResult.HasValue && dlgNewTask.DialogResult.Value == true)
            {
                appData.Aufgaben.Add(new Aufgabe(dlgNewTask.textBoxName.Text, dlgNewTask.textBoxDescription.Text,
                    (DateTime) dlgNewTask.datePickerStart.SelectedDate, (DateTime) dlgNewTask.datePickerEnd.SelectedDate,
                    (Boolean)dlgNewTask.checkBox.IsChecked, null));
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
            dlgEditTask.textBoxName.IsEnabled = false;

            dlgEditTask.ShowDialog();

            if (dlgEditTask.DialogResult.HasValue && dlgEditTask.DialogResult.Value == true)
            {
                if (appData.containsTask(dlgEditTask.textBoxName.Text))
                {
                    foreach (Aufgabe item in appData.Aufgaben)
                    {
                        if (item.strName.Equals(dlgEditTask.textBoxName.Text))
                        {
                            item.strDescription = dlgEditTask.textBoxDescription.Text;
                            item.dtPlannedStart = (DateTime)dlgEditTask.datePickerStart.SelectedDate;
                            item.dtPlannedEnd = (DateTime)dlgEditTask.datePickerEnd.SelectedDate;

                            if ((Boolean)dlgEditTask.checkBox.IsChecked)
                                item.iStatus = 1;
                            else
                                item.iStatus = 0;
                            
                            SaveData();
                            RefreshListBoxes();
                            MessageBox.Show("Die Aufgabe wurde gespeichert", "Aufgabe gespeichert", MessageBoxButton.OK);
                        }
                    }
                }
                else
                    MessageBox.Show("Der Aufgabenname kann nicht nachträglich geändert werden." +
                        "Bitte erstellen Sie eine neue Aufgabe.", "Änderung nich tmöglich", MessageBoxButton.OK);
            }
        }

        private void listBoxProjects_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            NewProject dlgEditProject = new NewProject();

            dlgEditProject.setName(appData.Projekte[listBoxProjects.SelectedIndex].strName);
            dlgEditProject.setDescription(appData.Projekte[listBoxProjects.SelectedIndex].strDescription);
            dlgEditProject.setPlannedStart(appData.Projekte[listBoxProjects.SelectedIndex].dtPlannedStart);
            dlgEditProject.setPlannedEnd(appData.Projekte[listBoxProjects.SelectedIndex].dtPlannedEnd);
            dlgEditProject.setIsStarted(false);

            foreach (Aufgabe item in appData.Aufgaben)
            {
                if (item.strProject.Equals(appData.Projekte[listBoxProjects.SelectedIndex].strName))
                {
                    dlgEditProject.addProjectTask(item);
                }
            }

            dlgEditProject.Title = "Projekt bearbeiten";
            dlgEditProject.textBoxName.IsEnabled = false;

            dlgEditProject.ShowDialog();

            if (dlgEditProject.DialogResult.HasValue && dlgEditProject.DialogResult.Value == true)
            {
                if (appData.containsProject(dlgEditProject.textBoxName.Text))
                {
                    foreach (Projekt item in appData.Projekte)
                    {
                        if (item.strName.Equals(dlgEditProject.textBoxName.Text))
                        {
                            item.strDescription = dlgEditProject.textBoxDescription.Text;
                            item.dtPlannedStart = (DateTime)dlgEditProject.datePickerStart.SelectedDate;
                            item.dtPlannedEnd = (DateTime)dlgEditProject.datePickerEnd.SelectedDate;

                            if ((Boolean)dlgEditProject.checkBox.IsChecked)
                                item.iStatus = 1;
                            else
                                item.iStatus = 0;
                        }
                    }
                }
                else
                    MessageBox.Show("if contains wurde umgangen", "if umgangen", MessageBoxButton.OK);

                SaveData();
                RefreshListBoxes();
                MessageBox.Show("Das Projekt wurde gespeichert", "Projekt gespeichert", MessageBoxButton.OK);
            }
        }
    }
}
