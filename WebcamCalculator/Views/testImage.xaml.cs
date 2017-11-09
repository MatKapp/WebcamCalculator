using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.VideoSurveillance;
using WebcamCalculator.Model;
using System.ComponentModel;
using Emgu.CV.Util;
using FeatureMatchingExample;

namespace WebcamCalculator.Views
{
    /// <summary>
    /// Interaction logic for testImage.xaml
    /// </summary>
    public partial class testImage : UserControl
    {


        private VideoCapture _capture;
        private MotionHistory _motionHistory;
        private BackgroundSubtractor _forgroundDetector;
        private TemplateContainer templateContainer = new TemplateContainer();
        private Mat modelImage1 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);
        private Mat modelImage2 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);






        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private InteropBitmap Bitmap2BitmapImage(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();
            InteropBitmap retval;

            try
            {
                retval = (InteropBitmap) Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }

            return retval;
        }
        




        public testImage()
        {
            InitializeComponent();

            //try to create the capture
            if (_capture == null)
            {
                try
                {
                    _capture = new VideoCapture();
                }
                catch (NullReferenceException excpt)
                {   //show errors if there is any
                    MessageBox.Show(excpt.Message);
                }
            }
            _capture.ImageGrabbed += ProcessFrame;
            _capture.Start();


        }
        
        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat image = new Mat();
            _capture.Retrieve(image);
          
         

            BriskController.GetText(templateContainer, image);
            
            long matchTime;
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            Mat surfImage = DrawMatches.Draw(modelImage1, image, out matchTime);
            Console.WriteLine(matchTime);
            

            InteropBitmap temp = Bitmap2BitmapImage(image.Bitmap);
            image1.Source = temp;
            image0.Source = temp;
            image2.Source = temp;


        }

        
    }
}
