using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ImageResizer
{
    public class ResizeMethods
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
            //Parallel.ForEach(filePaths, (currentFilePath) =>
            //{
            //    ResizeStandart(currentFilePath);
            //});
        }

        /// <summary>
        /// Сжимает изображение с заданным уровнем сжатия и сохраняет его.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="qualityValue">Уровень сжатия.</param>
        public static void ResizeStandart(string filePath, long qualityValue)
        {
            Stream stream = new FileStream(filePath, FileMode.Open);
            Bitmap bmp = new Bitmap(stream);
            stream.Close();
            bmp.Save(@"C:\Test\321.jpg");
            try
            {
                //using (Bitmap bmp2 = new Bitmap(filePath))
                //{
                //    bmp = (Bitmap)bmp2.Clone();
                //}
                ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                EncoderParameters myEncoderParameters = new EncoderParameters(1);
                EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 50L);
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmp.Save(@"C:\Test\123.jpg");
                //bmp.Save(@"C:\Test\123.jpg", jpgEncoder, myEncoderParameters);
            }
            catch
            {
                throw new Exception("Ошибка при изменении изображения.");
            }
            finally
            {
                if (bmp != null)
                {
                    //stream.Dispose();
                    bmp.Dispose();
                }
            }
        }

        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }
    }
}
