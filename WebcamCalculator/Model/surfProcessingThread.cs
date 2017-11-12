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

        public void processing(object data)
        {
           




            long matchTime;
            //Tuple<Mat, int> tuple = (Tuple<Mat, int>) data;
            //bool matchResult = DrawMatches.MatchResult(modelImage1, tuple.Item1, out matchTime);
            //Console.WriteLine(matchResult);
            
            
        }
    }
}
