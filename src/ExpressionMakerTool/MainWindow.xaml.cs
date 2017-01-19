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
using System.Drawing.Imaging;
using System.Globalization;

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

            //If folder not chosed, ask the user to choose it
            if (snippingFolder == null) {
                folder_pick_Click(sender, e);
            }
            
            if (screenshot != null && snippingFolder != null && !snippingFolder.Equals("")) {
                Bitmap thumb = null;

                int height;
                int width;
                int.TryParse(height_TextBox.Text, out height);
                int.TryParse(width_TextBox.Text, out width);
                height = height == 0 ? 150 : height;
                width = width == 0 ? 150 : width;

                int screenshotHeight = 0;
                int screenshotWidth = 0;
                if (screenshot.Height * 1.0 / screenshot.Width > height * 1.0 / width)
                {

                    screenshotHeight = (int)(screenshot.Height * 1.0 / screenshot.Width * width);
                    screenshotWidth = width;
                }
                else
                {
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

                string expressionText = expressionText_TextBox.Text;
                if (expressionText!=null && !expressionText.Equals("")) {
                    thumb = DrawTextOnBitmap(thumb, expressionText);
                    expressionText_TextBox.Text = "";
                }

                thumb.Save(snippingFolder + "//EMT"+ DateTime.Now.ToString("yyyyMMddHHmmss") + ".png", ImageFormat.Png);
            }
            this.Visibility = Visibility.Visible;
        }

        private Bitmap DrawTextOnBitmap(Bitmap bmp, string text) {
            int fontSize = 13;
            string familyName = "Times New Roman";

            RectangleF rectf = new RectangleF(fontSize/2, fontSize/2, bmp.Width - fontSize/2, bmp.Height - fontSize/2);
            Graphics g = Graphics.FromImage(bmp);
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.InterpolationMode = InterpolationMode.HighQualityBicubic;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            // Create a StringFormat object with the each line of text, and the block
            // of text centered on the page.
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Far;

            System.Drawing.Color c = new System.Drawing.Color();
            SolidBrush b = new SolidBrush(c);

            g.DrawString(text, new Font(familyName, fontSize, System.Drawing.FontStyle.Bold), System.Drawing.Brushes.WhiteSmoke, rectf, stringFormat);

            g.Flush();

            return bmp;
        }

        private SizeF MeasureString(string text, double fontSize, string typeFace)
        {
            FormattedText ft = new FormattedText
            (
               text,
               CultureInfo.CurrentCulture,
               FlowDirection.LeftToRight,
               new Typeface(typeFace),
               fontSize,
               System.Windows.Media.Brushes.Black
            );
            return new SizeF((float)ft.Width, (float)ft.Height);
        }

        private void expressionText_TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
