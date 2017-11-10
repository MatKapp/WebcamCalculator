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

namespace WebcamCalculator.Model
{
    public static class BriskController
    {
        public static bool DetectDigit(Mat observedImage, Mat templateImage)
        {
            Emgu.CV.Features2D.ORBDetector orbDetector = new Emgu.CV.Features2D.ORBDetector();
            var templateKeyPoints = new VectorOfKeyPoint();
            var observedKeyPoints = new VectorOfKeyPoint();
            Mat templateDescriptor = new Mat();
            Mat observedDescriptor = new Mat();
            orbDetector.DetectAndCompute(observedImage, null, templateKeyPoints, templateDescriptor, false);
            orbDetector.DetectAndCompute(templateImage, null, observedKeyPoints, observedDescriptor, false);
            BFMatcher matcher = new BFMatcher(DistanceType.L2);
            matcher.Add(templateDescriptor);

            VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch();
            matcher.KnnMatch(observedDescriptor, matches, 2, null);

            //Copied
            Mat mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
            mask.SetTo(new MCvScalar(255));
            Features2DToolbox.VoteForUniqueness(matches, 0.8, mask);

            if (matches.Size == 0)
            {
                return false;
            }
            else
            {
                int nonZeroCount = CvInvoke.CountNonZero(mask);
                double nonZeroCountNormalized = 1.0 * nonZeroCount / matches.Size;
                if (nonZeroCountNormalized > 0.05)
                {
                    nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(templateKeyPoints, observedKeyPoints, matches, mask, 1.5, 20);
                    nonZeroCountNormalized = 1.0 * nonZeroCount / matches.Size;
                    return nonZeroCountNormalized > 0.1;
                }
                return false;
            }
        }

        public static string GetText(TemplateContainer templateContainer, Mat image)
        {
            //Image<Bgr, byte> observedImage = new Image<Bgr, byte>("Images/67.png");
            Image<Bgr, byte> observedImage = image.ToImage<Bgr, byte>();
            //Image < Bgr, byte> observedImage = templateContainer.digits[8].image;

            string result = "";
            foreach(var template in templateContainer.digits)
            {
                if(DetectDigit(observedImage.Mat, template.image.Mat))
                {
                    result += template.correspondingValue;
                }
            }

            return result;
        }
    }
}
