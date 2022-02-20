using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace DeltaScript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void FilePathButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextRootFolder.Text = OpenFolder();
        }

        private void ArchivedFolderButton_OnClick(object sender, RoutedEventArgs e)
        {
            TextArchivedFolder.Text = OpenFolder();
        }

        private string OpenFolder()
        {
            FolderBrowserDialog openFolderDialog = new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
            {
                TextError.Text =("The Folder path does not exist");
            }
            else
            {
                string folderName = openFolderDialog.SelectedPath;
                return openFolderDialog.InitialDirectory = folderName;
            }
            return null;
        }

        private void CheckFolders_OnClick(object sender, RoutedEventArgs e)
        {
            if (TextRootFolder.Text == "" || TextArchivedFolder.Text == "")
            {
                TextError.Text = ("Error: Select both Root Folder and Archived Folder!");
            }
            else
            {
                TextError.Text = "";
                string[] nonExecutedFiles = CheckFilesInTwoFolder();
                DirectoryInfo rootFolderInfo = new DirectoryInfo(@TextRootFolder.Text);
                FileInfo[] rootFiles = rootFolderInfo.GetFiles("*.txt");
                List<File> checkedFiles = new List<File>();
                int i = 1;
                foreach (var file  in rootFiles)
                {
                    bool isExecuted = true;
                    foreach (var nonExecutedFile  in nonExecutedFiles)
                    {
                        if (file.Name == nonExecutedFile)
                        {
                            isExecuted = false;
                        }
                    }
                    checkedFiles.Add(new File() {Id = i, IsExecuted = isExecuted, Name = file.Name, LastAccessed = file.LastAccessTime, Filepath = file.DirectoryName});
                    i++;
                }
                ShowDataInDataGrid(checkedFiles);
            }
        }
      public class File
        {
            public int Id{ get; set; }
            public bool IsExecuted { get; set; }
            public  string Name { get; set; }
            public string Filepath { get; set; }
            public DateTime LastAccessed { get; set; }
            
        }
        private void ShowDataInDataGrid(List<File> allFiles)
        {
            InitializeComponent();
            RootDirectoryDeltaScriptsFiles.ItemsSource = allFiles;
        }
        private string[] CheckFilesInTwoFolder()
        {
            DirectoryInfo rootFolderInfo = new DirectoryInfo(@TextRootFolder.Text);
            DirectoryInfo archivedFolderInfo = new DirectoryInfo(@TextArchivedFolder.Text);
            FileInfo[] rootFiles = rootFolderInfo.GetFiles("*.txt");
            FileInfo[] archivedFiles = archivedFolderInfo.GetFiles("*.txt");
            List<string> rootFileNames = new List<string>();
            List<string> archivedFileNames = new List<string>();
            foreach (var file in rootFiles)
            {
                rootFileNames.Add(file.Name);
            }
            foreach (var file in archivedFiles)
            {
                archivedFileNames.Add(file.Name);
            }
            
            var result = rootFileNames.Except(archivedFileNames).ToArray();

            return result;
        }
    }
}