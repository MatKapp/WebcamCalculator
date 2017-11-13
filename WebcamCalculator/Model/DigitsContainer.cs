using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.Cuda;
using WebcamCalculator.Model;

namespace WebcamCalculator.Model
{
    public class TemplateContainer
    {
        private Emgu.CV.Features2D.ORBDetector orbDetector = new Emgu.CV.Features2D.ORBDetector();
        private  KAZE featureDetector = new KAZE();
        public class ImageData
        {
            public string path;
            public Image<Bgr, byte> image;
            public VectorOfKeyPoint keyPointsOrb = new VectorOfKeyPoint();
            public VectorOfKeyPoint keyPointsSurf = new VectorOfKeyPoint();
            public Mat descriptorOrb = new Mat();
            public Mat descriptorSurf = new Mat();
            public string correspondingValue;
            public ImageData(string path, string correspondingValue, Emgu.CV.Features2D.ORBDetector orbDetector, KAZE featureDetector)
            {
                this.path = path;
                this.image = new Image<Bgr, byte>(path);
                orbDetector.DetectAndCompute(image, null, keyPointsOrb, descriptorOrb, false);
                UMat uObservedImage = image.Mat.GetUMat(AccessType.Read);
                //featureDetector.DetectAndCompute(image, null, keyPointsSurf, descriptorSurf, false);
                featureDetector.DetectAndCompute(uObservedImage, null, keyPointsSurf, descriptorSurf, false);
                this.correspondingValue = correspondingValue;
            }
        }

        public List<ImageData> digits;
        public List<ImageData> signs;

        public TemplateContainer()
        {
            digits = new List<ImageData>();
            digits.Add(new ImageData("Images/0.png", "0", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/1.png", "1", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/2.png", "2", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/3.png", "3", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/4.png", "4", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/5.png", "5", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/6.png", "6", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/7.png", "7", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/8.png", "8", orbDetector, featureDetector));
            digits.Add(new ImageData("Images/9.png", "9", orbDetector, featureDetector));

            signs = new List<ImageData>();
            signs.Add(new ImageData("Images/-.png", "-", orbDetector, featureDetector));
            signs.Add(new ImageData("Images/+.png", "+", orbDetector, featureDetector));
            signs.Add(new ImageData("Images/star.png", "*", orbDetector, featureDetector));
            signs.Add(new ImageData("Images/slash.png", "/", orbDetector, featureDetector));

        }
    }
}
