using Microsoft.Win32;
using System;
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace QualityRangeForEmployee.SupportiveClasses
{
    internal class ImageConverter
    {
        public static byte[] ConvertToByteCollection(string uri)
        {
            BitmapImage image = new BitmapImage(new Uri($"{uri}", UriKind.Relative));
            MemoryStream memoryStream = new MemoryStream();
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));
            encoder.Save(memoryStream);
            return memoryStream.ToArray();
        }

        public static byte[] GetImageFromInternet(string uriImage)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                WebRequest.Create(uriImage).GetResponse().GetResponseStream().CopyTo(ms);
                return ms.ToArray();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка :{ex.Message} ");
                return null;
            }
        }

        public static byte[] ConvertToByteCollection(BitmapSource pasteImage)
        {
            if (pasteImage == null) return null;
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            encoder.QualityLevel = 100;
            byte[] bit = new byte[0];
            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Frames.Add(BitmapFrame.Create(pasteImage));
                encoder.Save(stream);
                bit = stream.ToArray();
                stream.Close();
            }
            return bit;
        }

        public static ImageSource ConvertToImageSource(byte[] bytes)
        {
            if (bytes == null) return null;

            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(bytes);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();
            ImageSource image = biImg as ImageSource;
            return image;
        }

        public static ImageSource ConvertToImageSourse(MemoryStream memoryStream)
        {
            BitmapImage biImg = new BitmapImage();
            biImg.BeginInit();
            biImg.StreamSource = memoryStream;
            biImg.EndInit();
            ImageSource image = biImg as ImageSource;
            return image;
        }

        public static byte[] OpenFileDialogSave()
        {
            OpenFileDialog openFile = new OpenFileDialog()
            {
                Filter = "Image files|*.jpg;*.jpeg;*.png"
            };

            if (openFile.ShowDialog().GetValueOrDefault())
            {
                BitmapFrame.Create(new MemoryStream(File.ReadAllBytes(openFile.FileName)), BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
                return File.ReadAllBytes(openFile.FileName);
            }
            return null;
        }
    }
}
