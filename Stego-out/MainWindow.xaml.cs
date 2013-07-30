using elp.StegoIn.lib;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;

namespace elp.Stego_out
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Pixel> pixelArray;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openImageDialog = new OpenFileDialog();
            openImageDialog.FileName = "Document";
            openImageDialog.DefaultExt = ".bmp";
            openImageDialog.Filter = "Точечный рисунок (*.bmp)|*.bmp";
            bool? result = openImageDialog.ShowDialog();
            if (result == true)
            {
                Bitmap bmpImage = new Bitmap(openImageDialog.FileName);
                pixelArray = Converter.ImageToByte(bmpImage);
                string Message = Converter.DecodeMessage(pixelArray);
                txtBox.Text = Message;
            }

        }
    }
}
