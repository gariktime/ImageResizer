using ImageResizer.Utility;
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
        private int maxFileSize = 500;
        private List<ResizeParameters> resizeableFiles = null;
        private int range11 = 200, range12 = 400,
                    range21 = 400, range22 = 600,
                    range31 = 600, range32 = 1000,
                    range41 = 1000, range42 = 2000,
                    range51 = 2000, range52 = 4000,
                    range61 = 4000, range62 = 6000;

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
                string[] filePaths = Directory.GetFiles(resizePath, "*.jp*", SearchOption.AllDirectories); //список файлов всех изображений
                resizeableFiles = ResizeMethods.GetResizeableFiles(filePaths, maxFileSize); //список файлов, подлежащих ресайзу
                int range1 = resizeableFiles.Where(p => p.FileSize >= range11 && p.FileSize <= range12).Count();
                int range2 = resizeableFiles.Where(p => p.FileSize >= range21 && p.FileSize <= range22).Count();
                int range3 = resizeableFiles.Where(p => p.FileSize >= range31 && p.FileSize <= range32).Count();
                int range4 = resizeableFiles.Where(p => p.FileSize >= range41 && p.FileSize <= range42).Count();
                int range5 = resizeableFiles.Where(p => p.FileSize >= range51 && p.FileSize <= range52).Count();
                int range6 = resizeableFiles.Where(p => p.FileSize >= range61 && p.FileSize <= range62).Count();
                labelFilesTotal.Content = string.Format("Всего файлов в указанной директории : \t{0}", filePaths.Length);
                labelFilesResizeable.Content = string.Format("Всего файлов размером более {0} Кб : \t{1}", maxFileSize, resizeableFiles.Count);
                labelRange1.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range11, range12, range1);
                labelRange2.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range21, range22, range2);
                labelRange3.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range31, range32, range3);
                labelRange4.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range41, range42, range4);
                labelRange5.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range51, range52, range5);
                labelRange6.Content = string.Format("Файлов размером от {0} Кб до {1} Кб : \t{2}", range61, range62, range6);
            }
        }

        private void btnResize_Click(object sender, RoutedEventArgs e)
        {
            Parallel.ForEach(resizeableFiles, (current) =>
            {
                ResizeMethods.ResizeStandart(current.FilePath, current.ResizeQuality);
            });
        }
    }
}
