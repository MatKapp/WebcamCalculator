//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.OCR;
using Emgu.CV.Structure;
using Emgu.CV.UI;
using Emgu.CV.Util;
using Emgu.CV.VideoSurveillance;
using Emgu.Util;
using FeatureMatchingExample;
using WebcamCalculator;
using WebcamCalculator.Model;

namespace MotionDetection
{
    public partial class Form1 : Form
    {
        private VideoCapture _capture;
        private TemplateContainer templateContainer = new TemplateContainer();
        private Mat modelImage1 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);
        private Mat modelImage2 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);
        private Thread oThread;

        public Form1()
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

            if (_capture != null) //if camera capture has been successfully created
            {
                _capture.ImageGrabbed += ProcessFrame;
                _capture.Start();

            }
        }

        private void ProcessFrame(object sender, EventArgs e)
        {
            Mat image = new Mat();

            _capture.Retrieve(image);



            // find and draw the overall motion angle
            double overallAngle, overallMotionPixelCount;

            if (this.Disposing || this.IsDisposed)
                return;

            capturedImageBox.Image = image;
            forgroundImageBox.Image = image;

            string orbResult = BriskController.GetText(templateContainer, image);
            UpdateTextL5($"Orb result: {orbResult}");

            long matchTime;
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            surfController.StartProcessing(image);
            //image =DrawMatches.Draw(modelImage1, image, out matchTime);

            motionImageBox.Image = image;

        }



        private void UpdateText(String text)
        {
            if (!IsDisposed && !Disposing && InvokeRequired)
            {
                Invoke((Action<String>)UpdateText, text);
            }
            else
            {
                label3.Text = text;
            }
        }

        private void UpdateTextL5(String text)
        {
            if (!IsDisposed && !Disposing && InvokeRequired)
            {
                Invoke((Action<String>)UpdateTextL5, text);
            }
            else
            {
                label5.Text = text;
            }
        }

        private static void DrawMotion(IInputOutputArray image, Rectangle motionRegion, double angle, Bgr color)
        {
            //CvInvoke.Rectangle(image, motionRegion, new MCvScalar(255, 255, 0));
            float circleRadius = (motionRegion.Width + motionRegion.Height) >> 2;
            Point center = new Point(motionRegion.X + (motionRegion.Width >> 1), motionRegion.Y + (motionRegion.Height >> 1));

            CircleF circle = new CircleF(
               center,
               circleRadius);

            int xDirection = (int)(Math.Cos(angle * (Math.PI / 180.0)) * circleRadius);
            int yDirection = (int)(Math.Sin(angle * (Math.PI / 180.0)) * circleRadius);
            Point pointOnCircle = new Point(
                center.X + xDirection,
                center.Y - yDirection);
            LineSegment2D line = new LineSegment2D(center, pointOnCircle);
            CvInvoke.Circle(image, Point.Round(circle.Center), (int)circle.Radius, color.MCvScalar);
            CvInvoke.Line(image, line.P1, line.P2, color.MCvScalar);

        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {

            if (disposing && (components != null))
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            _capture.Stop();
        }



    }


}
