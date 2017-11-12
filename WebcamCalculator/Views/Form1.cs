//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        public Mat image= new Mat();
        private VideoCapture _capture;
        private TemplateContainer templateContainer = new TemplateContainer();
        private OrbController orbController = new OrbController();
        private List<int> surfResult= new List<int>();
        SortedDictionary<int, int> resultDigits = new SortedDictionary<int, int>();
        SortedDictionary<int, int> resultSign = new SortedDictionary<int, int>();
        string sign = "";
        string equation = "";
        int equationResult = 0;
        List<TemplateContainer.ImageData> tempImagesList = new List<TemplateContainer.ImageData>();

        public Form1()
        {
            
            InitializeComponent();
            FeatureComboBox.Items.Add("SURF");
            FeatureComboBox.Items.Add("ORB");
            FeatureComboBox.SelectedItem = "ORB";

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

            _capture.Retrieve(image);
            


            // find and draw the overall motion angle
            double overallAngle, overallMotionPixelCount;

            if (this.Disposing || this.IsDisposed)
                return;


            long matchTime;
            Mat grayImage = new Mat();
            CvInvoke.CvtColor(image, grayImage, Emgu.CV.CvEnum.ColorConversion.Bgr2Gray);
            motionImageBox.Image = image;

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

        //private static void DrawMotion(IInputOutputArray image, Rectangle motionRegion, double angle, Bgr color)
        //{
        //    //CvInvoke.Rectangle(image, motionRegion, new MCvScalar(255, 255, 0));
        //    float circleRadius = (motionRegion.Width + motionRegion.Height) >> 2;
        //    Point center = new Point(motionRegion.X + (motionRegion.Width >> 1), motionRegion.Y + (motionRegion.Height >> 1));

        //    CircleF circle = new CircleF(
        //       center,
        //       circleRadius);

        //    int xDirection = (int)(Math.Cos(angle * (Math.PI / 180.0)) * circleRadius);
        //    int yDirection = (int)(Math.Sin(angle * (Math.PI / 180.0)) * circleRadius);
        //    Point pointOnCircle = new Point(
        //        center.X + xDirection,
        //        center.Y - yDirection);
        //    LineSegment2D line = new LineSegment2D(center, pointOnCircle);
        //    CvInvoke.Circle(image, Point.Round(circle.Center), (int)circle.Radius, color.MCvScalar);
        //    CvInvoke.Line(image, line.P1, line.P2, color.MCvScalar);

        //}

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

        private void button1_Click(object sender, EventArgs e)
        {
            if (FeatureComboBox.SelectedItem == "ORB")
            {
                string orbResult = orbController.GetText(templateContainer, image);
                UpdateTextL5($"Orb result: {orbResult}");
            }

            else
            {
                //surfController.StartProcessing(image, templateContainer, surfResult);
                //DrawMatches.MatchResult(templateContainer.digits[5].image.Mat, image, out matchTime);
                _capture.Dispose();
                surfController.StartProcessing(image, templateContainer.digits, resultDigits);
                
                tempImagesList.Add(templateContainer.signs[1]);
                tempImagesList.Add(templateContainer.signs[2]);
                surfController.StartProcessing(image, templateContainer.signs, resultSign);
                _capture = new VideoCapture();
                _capture.ImageGrabbed += ProcessFrame;
                _capture.Start();

                if (resultDigits.Count > 1)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Console.WriteLine(resultDigits.ElementAt(i));
                    }
                }
                if (resultSign.Count > 0 && resultDigits.Count >1)
                {
                    if (resultSign.ElementAt(0).Value == 1)
                    {
                        sign = "+";
                        equationResult = (resultDigits.ElementAt(0).Value + resultDigits.ElementAt(1).Value);
                    }
                    else
                    {
                        sign = "*";
                        equationResult = (resultDigits.ElementAt(0).Value * resultDigits.ElementAt(1).Value);
                    }
                    equation = resultDigits.ElementAt(0).Value.ToString() + sign +
                               resultDigits.ElementAt(1).Value.ToString() + "=" + equationResult.ToString();
                }

                surfResultLabel.Text = equation;

            }
        }
    }


}
