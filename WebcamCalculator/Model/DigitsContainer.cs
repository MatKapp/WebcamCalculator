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
        public class ImageData
        {
            public string path;
            public Image<Bgr, byte> image;
            public string correspondingValue;
            public ImageData(string path, string correspondingValue)
            {
                this.path = path;
                this.image = new Image<Bgr, byte>(path);
                this.correspondingValue = correspondingValue;
            }
        }

        public List<ImageData> digits;
        public List<ImageData> signs;

        public TemplateContainer()
        {
            digits = new List<ImageData>();
            digits.Add(new ImageData("Images/0.png", "0"));
            digits.Add(new ImageData("Images/1.png", "1"));
            digits.Add(new ImageData("Images/2.png", "2"));
            digits.Add(new ImageData("Images/3.png", "3"));
            digits.Add(new ImageData("Images/4.png", "4"));
            digits.Add(new ImageData("Images/5.png", "5"));
            digits.Add(new ImageData("Images/6.png", "6"));
            digits.Add(new ImageData("Images/7.png", "7"));
            digits.Add(new ImageData("Images/8.png", "8"));
            digits.Add(new ImageData("Images/9.png", "9"));

            signs = new List<ImageData>();
            signs.Add(new ImageData("Images/-.png", "-"));
            signs.Add(new ImageData("Images/+.png", "+"));
            signs.Add(new ImageData("Images/star.png", "*"));
            signs.Add(new ImageData("Images/slash.png", "/"));

        }
    }
}
