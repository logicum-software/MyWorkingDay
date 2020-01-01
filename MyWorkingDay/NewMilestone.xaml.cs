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
    /// Interaktionslogik für NewMilestone.xaml
    /// </summary>
    public partial class NewMilestone : Window
    {
        public NewMilestone()
        {
            InitializeComponent();

            textBoxName.SelectAll();
            textBoxName.Focus();
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
