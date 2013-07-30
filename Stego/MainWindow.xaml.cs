using elp.StegoIn.lib;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using draw = System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace elp.StegoIn
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private draw.Bitmap bmpImage;
        List<Pixel> pixelArray;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            if (pixelArray == null)
            {
                OpenFileDialog openImageDialog = new OpenFileDialog();
                openImageDialog.FileName = "Document";
                openImageDialog.DefaultExt = ".bmp";
                openImageDialog.Filter = "Точечный рисунок (*.bmp)|*.bmp";
                bool? result = openImageDialog.ShowDialog();
                if (result == true)
                {
                    bmpImage = new draw.Bitmap(openImageDialog.FileName);
                    BitmapImage bi = new BitmapImage(new Uri(openImageDialog.FileName, UriKind.Absolute));
                    Image img = new Image();
                    img.Source = bi;
                    img.Width = 400;
                    img.Height = 400;
                    img.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                    img.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                    img.Margin = new Thickness(10, 100, 10, 0);
                    MainGrid.Children.Add(img);

                    pixelArray = Converter.ImageToByte(bmpImage);

                    LoadButton.Content = "Шифровать";
                }
            }
            else
            {
                string message = txtBox.Text;
                char[] txtArray = message.ToCharArray();
                string btxtArray = Converter.StringToBitString(txtArray);                
                List<Pixel> stegoMessage = Converter.EncodeMessage(pixelArray, btxtArray);
                
                int a = 0;
                draw.Bitmap nbmpImage = new draw.Bitmap(bmpImage.Width, bmpImage.Height);
                for (int i = 0; i < nbmpImage.Height; i++)
                {
                    for (int j = 0; j < nbmpImage.Width; j++)
                    {
                        int pos = a;
                        Pixel p = stegoMessage[pos];
                        System.Drawing.Color c = System.Drawing.Color.FromArgb(Convert.ToInt32(p.a), Convert.ToInt32(p.r), Convert.ToInt32(p.g), Convert.ToInt32(p.b));
                        nbmpImage.SetPixel(j, i, c);
                        a++;
                    }
                }
                SaveFileDialog svDialog = new SaveFileDialog();
                svDialog.FileName = "Document";
                svDialog.DefaultExt = ".bmp";
                svDialog.Filter = "Точечный рисунок (*.bmp)|*.bmp";
                bool? result1 = svDialog.ShowDialog();
                if (result1 == true)
                {
                    nbmpImage.Save(svDialog.FileName);
                }
            }
        }
    }
}
