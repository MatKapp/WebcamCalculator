using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Cuda;
using System.Data;

namespace WebcamCalculator.Model
{
    public class OrbController
    {
        private Emgu.CV.Features2D.ORBDetector orbDetector = new Emgu.CV.Features2D.ORBDetector();
        private VectorOfKeyPoint observedKeyPoints = new VectorOfKeyPoint();
        private Mat observedDescriptor = new Mat();
        private enum Side { Left, Right, Both };
        private DataTable calculator = new DataTable();

        private double DetectTemplate(Mat observedImage, TemplateContainer.ImageData template)
        {
            orbDetector.DetectAndCompute(observedImage, null, observedKeyPoints, observedDescriptor, false);

            if (template.keyPointsOrb.Size > 0 && observedKeyPoints.Size > 0)
            {
                BFMatcher matcher = new BFMatcher(DistanceType.L2);
                matcher.Add(template.descriptorOrb);

                VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
                matcher.KnnMatch(observedDescriptor, matches, 2, null);

                //Copied
                Mat mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                mask.SetTo(new MCvScalar(255));
                Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

                if (matches.Size == 0)
                {
                    return 0.0;
                }
                else
                {
                    int nonZeroCount = CvInvoke.CountNonZero(mask);
                    double nonZeroCountNormalized = 1.0 * nonZeroCount / template.keyPointsOrb.Size;
                    if (nonZeroCount > 3)
                    {
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(template.keyPointsOrb, observedKeyPoints, matches, mask, 1.8, 18);
                        nonZeroCountNormalized = 1.0 * nonZeroCount / template.keyPointsOrb.Size;
                        return nonZeroCount;
                    }
                    return 0.0;
                }
            }
            else
            {
                return 0.0;
            }
        }

        private string GetBestPassingTemplate(List<TemplateContainer.ImageData> templates, Image<Bgr, byte> observedImage, Side side)
        {
            observedImage.ROI = System.Drawing.Rectangle.Empty;
            if (side != Side.Both)
            {
                int halfOfWidth = (int)(0.5 * observedImage.Width);
                int offset = side == Side.Left ? 0 : halfOfWidth;
                observedImage.ROI = new Rectangle(offset, 0, halfOfWidth, observedImage.Height);
            }
            string result = "";
            double best_result = 0.0;
            foreach (var template in templates)
            {
                double obtainedValue = DetectTemplate(observedImage.Mat, template);
                if (obtainedValue > best_result)
                {
                    best_result = obtainedValue;
                    result = template.correspondingValue;
                }
            }
            return result;
        }

        public string compute(string equation)
        {
            try
            {
                return calculator.Compute(equation, "").ToString();
            }
            catch(Exception)
            {
                return " ?";
            }
        }

        public string GetText(TemplateContainer templateContainer, Mat image)
        {
            Image<Bgr, byte> observedImage = image.ToImage<Bgr, byte>();
            Image<Bgr, byte> enhanced = observedImage.Clone();
            enhanced._GammaCorrect(2);
            //enhanced._EqualizeHist();
            //enhanced = enhanced.ThresholdBinary(new Bgr(System.Drawing.Color.LightGray), new Bgr(System.Drawing.Color.Black));
            //observedImage = observedImage.ThresholdBinary(new Bgr(System.Drawing.Color.Gray), new Bgr(System.Drawing.Color.Black));
            string left = "", right = "", sign = "";
            left = GetBestPassingTemplate(templateContainer.digits, enhanced, Side.Left);
            right = GetBestPassingTemplate(templateContainer.digits, enhanced, Side.Right);
            sign = GetBestPassingTemplate(templateContainer.signs, enhanced, Side.Both);
            observedImage.ROI = System.Drawing.Rectangle.Empty;
            string equation = $"{left} {sign} {right}";
            return $"{equation} = {compute(equation)}";
        }
    }
}
