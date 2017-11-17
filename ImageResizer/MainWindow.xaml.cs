using Microsoft.Win32;
using Microsoft.WindowsAPICodePack.Dialogs;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageResizer
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string resizePath = string.Empty;
        private int maxFileSize = 5000;

        public MainWindow()
        {
            InitializeComponent();
        }

        //кнопка выбора пути
        private void btnOpenPath_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            dialog.AddToMostRecentlyUsedList = false;
            dialog.AllowNonFileSystemItems = false;
            dialog.EnsureFileExists = true;
            dialog.EnsurePathExists = true;
            dialog.EnsureReadOnly = false;
            dialog.EnsureValidNames = true;
            dialog.Multiselect = false;
            dialog.ShowPlacesList = true;

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                txbPath.Text = dialog.FileName;
                resizePath = dialog.FileName;
            }
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            string[] filePaths = Directory.GetFiles(resizePath, "*.jp*", SearchOption.AllDirectories); //список файлов всех изображений
            List<string> resizeableFiles = ResizeMethods.GetResizeableFiles(filePaths, maxFileSize); //список файлов, подлежащих ресайзу
        }


    }
}
