using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using FeatureMatchingExample;

namespace WebcamCalculator
{
    class surfProcessingThread
    {
        public surfProcessingThread()
        {

        }

        private Mat modelImage1 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);

        public void processing(object image)
        {
           
            long matchTime;
            bool matchResult = DrawMatches.MatchResult(modelImage1, (Mat)image, out matchTime);
            Console.WriteLine(matchResult);
            
            
        }
    }
}
