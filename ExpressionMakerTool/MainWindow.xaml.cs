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

namespace ExpressionPackageMaker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private string snippingFolder = null;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void folder_pick_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Forms.DialogResult result = dialog.ShowDialog();
            snippingFolder = dialog.SelectedPath;
        }

        private async void snip_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            await Task.Delay(500);
            System.Drawing.Image screenshot = snippingtool.SnippingTool.Snip();
            if (screenshot != null && snippingFolder != null) {
                screenshot.Save(snippingFolder + "//screenshot.png");
            }
            this.Visibility = Visibility.Visible;
        }
    }
}
