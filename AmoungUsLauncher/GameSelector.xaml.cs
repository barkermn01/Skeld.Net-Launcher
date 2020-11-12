using Microsoft.Win32;
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

namespace AmoungUsLauncher
{
    /// <summary>
    /// Interaction logic for GameSelector.xaml
    /// </summary>
    public partial class GameSelector : Window
    {
        public string GamePath = "";
        public GameSelector()
        {
            InitializeComponent();
        }

        private void openBrowseDialog()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Among Us Executable (Among Us.exe)|Among Us.exe";
            if (openFileDialog.ShowDialog() == true)
            {
                Path.Text = openFileDialog.FileName;
                this.GamePath = openFileDialog.FileName;
            }
        }

        private void BrowseBtn_Click(object sender, RoutedEventArgs e)
        {
            openBrowseDialog();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Path.Text = this.GamePath;
        }

        private void Path_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            openBrowseDialog();
        }
    }
}
