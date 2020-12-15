using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        private bool enableCrewlink = false;
        private Process crewLinkProc;

        public MainWindow()
        {
            InitializeComponent();
            GamePath = ReadGamePath();
            enableCrewLinkCheckMenuItem.IsChecked = enableCrewlink;
        }

        private void SaveGamePath()
        {
            string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string SkeldLauncherDataPath = LocalAppDataPath + @"Skeld Launcher\";
            if (!Directory.Exists(SkeldLauncherDataPath)) { Directory.CreateDirectory(SkeldLauncherDataPath); }
            System.IO.File.WriteAllText(SkeldLauncherDataPath+"GameData.txt", GamePath + ";" + (enableCrewlink?"1":"0"));
        }

        private string ReadGamePath()
        {
            try
            {
                string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string SkeldLauncherDataPath = LocalAppDataPath + @"Skeld Launcher\";
                if (!Directory.Exists(SkeldLauncherDataPath)) { Directory.CreateDirectory(SkeldLauncherDataPath); }
                string[] data = System.IO.File.ReadAllText(SkeldLauncherDataPath + "GameData.txt").Split(';');
                this.enableCrewlink = (data[1] == "1");
                return data[0];
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

        private void MenuItem_EnableCrewlink_Click(object sender, RoutedEventArgs e)
        {
            string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string crewlinkPath = LocalAppDataPath + @"\Programs\crewlink\CrewLink.exe";
            if (!File.Exists(crewlinkPath))
            {
                InstallCrewLink icl = new InstallCrewLink();
                icl.ShowDialog();
            }
            else
            {
                this.enableCrewlink = true;
                enableCrewLinkCheckMenuItem.IsChecked = this.enableCrewlink;
                this.SaveGamePath();
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

                File.Delete(AppDataLocalLow + "\\Innersloth\\Among Us\\regionInfo.dat");
                File.Copy("./regionInfo.dat", AppDataLocalLow + "\\Innersloth\\Among Us\\regionInfo.dat");
                if (this.enableCrewlink && this.crewLinkProc == null)
                {
                    string LocalAppDataPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                    string crewlinkPath = LocalAppDataPath + @"\Programs\crewlink\CrewLink.exe";
                    this.crewLinkProc = Process.Start(crewlinkPath);
                }

                Process AmongUsExe;
                if (this.GamePath.Contains("steamapps"))
                {
                    Process steam = Process.Start(@"steam://run/945360/");
                    steam.WaitForExit();
                    Thread.Sleep(2000);
                    AmongUsExe = Process.GetProcessesByName("Among Us")[0];
                }
                else
                {
                    AmongUsExe = Process.Start(this.GamePath);
                }

                this.ShowInTaskbar = false;
                this.Hide();

                AmongUsExe.WaitForExit();
                if (this.enableCrewlink && this.crewLinkProc != null)
                {
                    this.crewLinkProc.Kill();
                }
                this.ShowInTaskbar = true;
                this.Show();
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
