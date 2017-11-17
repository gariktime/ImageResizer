using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageResizer
{
    public static class ResizeMethods
    {
        /// <summary>
        /// Возвращает список полных имён файлов 
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="maxFileSize"></param>
        /// <returns></returns>
        public static List<string> GetResizeableFiles(string[] filePaths, int maxFileSize)
        {
            List<string> result = new List<string>();

            Parallel.ForEach(filePaths, (currentFilePath) =>
            {
                FileInfo fileInfo = new FileInfo(currentFilePath);
                //если размер файла больше заданного, то добавляем в List
                if (fileInfo.Length / 1024 > maxFileSize)
                    result.Add(currentFilePath);
            });

            return result;
        }

        public static void ResizeStandart(List<string> filePaths)
        {
            Parallel.ForEach(filePaths, (currentFilePath) =>
            {
                ResizeStandart(currentFilePath);
            });
        }

        public static void ResizeStandart(string filePath)
        {

        }
    }
}
