using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Drawing.Drawing2D;

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
            Bitmap thumb = null;

            int height;
            int width;
            int.TryParse(height_TextBox.Text, out height);
            int.TryParse(width_TextBox.Text, out width);
            height = height == 0 ? 150 : height;
            width = width == 0 ? 150 : width;

            int screenshotHeight = 0;
            int screenshotWidth = 0;
            if (screenshot.Height*1.0 / screenshot.Width > height*1.0 / width) {

                screenshotHeight = (int)(screenshot.Height * 1.0 / screenshot.Width * width);
                screenshotWidth = width;
            }
            else {
                screenshotHeight = height;
                screenshotWidth = (int)(screenshot.Width * 1.0 / screenshot.Height * height);
            }

            thumb = new Bitmap(screenshot, screenshotWidth, screenshotHeight);

            /*
            Graphics oGraphic = Graphics.FromImage(thumb);

            oGraphic.CompositingQuality = CompositingQuality.HighSpeed;
            oGraphic.SmoothingMode = SmoothingMode.HighSpeed;
            oGraphic.InterpolationMode = InterpolationMode.Low;

            System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 200, 200);
            oGraphic.DrawImage(screenshot, rect);
            */

            if (screenshot != null && snippingFolder != null && !snippingFolder.Equals("")) {
                thumb.Save(snippingFolder + "//screenshot.png");
            }
            this.Visibility = Visibility.Visible;
        }
    }
}
