using Microsoft.Win32;
using System;
using System.Collections.Generic;
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

namespace AWS_1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string currentFilePath = null;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnBrowse(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*jpg";
            openFileDialog.InitialDirectory = @"C:\Users\DELL\Pictures";
            if (openFileDialog.ShowDialog() == true)
            {
                Warning(false);
                txtFilePath.Content = openFileDialog.FileName;
                currentFilePath = openFileDialog.FileName;
            }
        }

        private void OnOpenImage(object sender, RoutedEventArgs e)
        {
            if (currentFilePath != null)
            {
                ImageWindow imageWindow = new ImageWindow(currentFilePath);
                imageWindow.Show();
            } else
            {
                Warning(true);
            }
        }

        private void Warning(bool isError)
        {
            if (isError)
            {
                txtWarning.Visibility = Visibility.Visible;
            }
            else
            {
                txtWarning.Visibility = Visibility.Hidden;
            }
        }


    }
}
