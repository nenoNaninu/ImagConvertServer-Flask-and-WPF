using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;


namespace ImageUploaderWPF
{
    /// <summary>
    /// .xaml.csに書くなって？うるせぇ！時間がねぇんだ！
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private string imgFileName = "";

        private void Select_file(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog("フォルダの選択");
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                this.imgFileName = dialog.FileName;
            }
        }

        
        private async void Upload_ImgAsync(object sender, RoutedEventArgs e)
        {
            Bitmap bitmap = new Bitmap(imgFileName);
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                byte[] bytebBuffer = ms.GetBuffer();
                try
                {
                    using (var client = new HttpClient())
                    {
                        string endpoint = "http://127.0.0.1:5000/convert";
                        var content = new ByteArrayContent(bytebBuffer);
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        HttpResponseMessage response = await client.PostAsync(endpoint, content);
                        byte[] data = await response.Content.ReadAsByteArrayAsync();
                        var responseBin = new MemoryStream(data);
                        Bitmap responseBitmap = new Bitmap(responseBin);
                        ImageBox.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(responseBitmap.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    throw;
                }

            }
        }
    }
}
