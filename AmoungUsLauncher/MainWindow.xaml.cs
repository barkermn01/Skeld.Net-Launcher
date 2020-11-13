using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace AmoungUsLauncher
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string GamePath = "";
        public MainWindow()
        {
            InitializeComponent();
            GamePath = ReadGamePath();
        }

        private void SaveGamePath()
        {
            System.IO.File.WriteAllText("./GamePath.txt", GamePath);
        }

        private string ReadGamePath()
        {
            try
            {
                return System.IO.File.ReadAllText("./GamePath.txt");
            }
            catch (Exception)
            {
                return "";
            }
        }

        private void setPath()
        {
            GameSelector gs = new GameSelector();
            gs.GamePath = this.GamePath;
            gs.ShowDialog();
            this.GamePath = gs.GamePath;
            if (GamePath == "")
            {
                LoaderText.Text = "Please set Game Path";
                PlayBtn.Content = "Set Path";
            }
            else
            {
                LoaderText.Text = "Press Launch to Begin";
                PlayBtn.Content = "Launch";
                SaveGamePath();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GamePath == "")
            {
                LoaderText.Text = "Please set Game Path";
                PlayBtn.Content = "Set Path";
            }
            else
            {
                PlayBtn.Content = "Launch";
            }
        }

        private void PlayBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.GamePath == "")
            {
                setPath();
            }
            else
            {
                string AppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string AppDataLocalLow = System.IO.Path.GetFullPath(AppDataPath + "\\..\\LocalLow\\");
                try
                {
                    File.Delete(AppDataLocalLow + "\\Innersloth\\Among Us\\regionInfo.dat");
                    File.Copy("./regionInfo.dat", AppDataLocalLow + "\\Innersloth\\Among Us\\regionInfo.dat");
                    Process.Start(this.GamePath);
                }
                catch (Exception) { }
            }
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuItem_SetPath_Click(object sender, RoutedEventArgs e)
        {
            setPath();
        }
    }
}
