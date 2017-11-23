using ImageResizer.Utility;
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
        /// Возвращает список полных имён файлов, подлежащих сжатию и коэффициенты сжатия
        /// </summary>
        /// <param name="filePaths"></param>
        /// <param name="maxFileSize"></param>
        /// <returns></returns>
        public static List<ResizeParameters> GetResizeableFiles(string[] filePaths, int maxFileSize)
        {
            List<ResizeParameters> result = new List<ResizeParameters>();

            Parallel.ForEach(filePaths, (currentFilePath) =>
            {
                FileInfo fileInfo = new FileInfo(currentFilePath);
                int fileSize = (int)(fileInfo.Length / 1024);
                //если размер файла больше заданного, то добавляем в List
                if (fileSize > maxFileSize)
                {
                    if (fileSize >= 200 && fileSize < 400)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 70L });
                    else if (fileSize >= 400 && fileSize < 600)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 60L });
                    else if (fileSize >= 600 && fileSize < 1000)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 50L });
                    else if (fileSize >= 1000 && fileSize < 2000)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 40L });
                    else if (fileSize >= 2000 && fileSize < 4000)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 30L });
                    else if (fileSize >= 4000 && fileSize <= 6000)
                        result.Add(new ResizeParameters() { FilePath = currentFilePath, FileSize = fileSize, ResizeQuality = 20L });

                }
            });

            return result;
        }

        /// <summary>
        /// Сжимает изображение с заданным уровнем сжатия и сохраняет его.
        /// </summary>
        /// <param name="filePath">Путь к файлу.</param>
        /// <param name="qualityValue">Уровень сжатия.</param>
        public static void ResizeStandart(string filePath, long qualityValue)
        {
            Bitmap bmp = new Bitmap(filePath);
            byte[] bmpData = null;

            try
            {
                //конвертация изображения в byte[]
                using (MemoryStream ms = new MemoryStream())
                {
                    bmp.Save(ms, ImageFormat.Bmp);
                    bmpData = ms.ToArray();
                    bmp.Dispose();
                }
                //изменение изображения
                using (MemoryStream ms = new MemoryStream(bmpData))
                {
                    bmp = new Bitmap(ms); //создание Bitmap из byte[]
                    //ToYCbCr(ref bmp); //перевод из RGB в YCbCr
                    ImageCodecInfo jpgEncoder = GetEncoder(ImageFormat.Jpeg);
                    System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, qualityValue); //степень сжатия
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmp.Save(filePath, jpgEncoder, myEncoderParameters); //сохранение изображения с указанными изменениями
                }
            }
            catch
            {
                throw new Exception("Ошибка при изменении изображения.");
            }
            finally
            {
                if (bmp != null)
                {
                    bmp.Dispose();
                }
            }
        }

        public static void ResizeStandart(List<ResizeParameters> filePaths)
        {
            Parallel.ForEach(filePaths, (current) =>
            {
                ResizeStandart(current.FilePath, current.ResizeQuality);
            });
        }

        #region Вспомогательные методы

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

        private static void ToYCbCr(ref Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;
            byte[,] yData = new byte[width, height];                     //luma
            byte[,] bData = new byte[width, height];                     //Cb
            byte[,] rData = new byte[width, height];                     //Cr

            unsafe
            {
                BitmapData bitmapData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadWrite, bmp.PixelFormat);
                int heightInPixels = bitmapData.Height;
                int widthInBytes = width * 3;
                byte* ptrFirstPixel = (byte*)bitmapData.Scan0;

                //Convert to YCbCr
                for (int y = 0; y < heightInPixels; y++)
                {
                    byte* currentLine = ptrFirstPixel + (y * bitmapData.Stride);
                    for (int x = 0; x < width; x++)
                    {
                        int xPor3 = x * 3;
                        float blue = currentLine[xPor3++];
                        float green = currentLine[xPor3++];
                        float red = currentLine[xPor3];

                        yData[x, y] = (byte)((0.299 * red) + (0.587 * green) + (0.114 * blue));
                        bData[x, y] = (byte)(128 - (0.168736 * red) + (0.331264 * green) + (0.5 * blue));
                        rData[x, y] = (byte)(128 + (0.5 * red) + (0.418688 * green) + (0.081312 * blue));
                    }
                }
                bmp.UnlockBits(bitmapData);
            }
        }

        #endregion

    }
}
