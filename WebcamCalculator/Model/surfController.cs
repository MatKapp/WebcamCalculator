using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using FeatureMatchingExample;
using Image = System.Drawing.Image;

namespace WebcamCalculator.Model
{
    static class surfController
    {
        static private Thread oThread;

        static private Tuple<Mat, TemplateContainer> tuple;

        static private surfProcessingThread oSurfProcessingThread;

        static public void StartProcessing(Mat image, List<TemplateContainer.ImageData> templateContainer,
            SortedDictionary<int, int> result)
        {
            long matchTime;
            int temp;

            int i = 0;
            foreach (var template in templateContainer)
            {
                temp = 0;
                temp = DrawMatches.MatchResult(template.image.Mat, image, out matchTime);
                if (temp != 0)
                {
                    while (result.Keys.Contains(-temp))
                    {
                        temp -= 1;
                    }
                    result.Add(-temp, i);
                    Console.WriteLine("znalazle, {0}", i);

                }
                i += 1;
            }



        }
    }
}
//{

        //    Console.WriteLine("nowy watek");
        //    TemplateContainer temp = new TemplateContainer();
        //    for (int i = 0; i < 10; i++)
        //    {
        //        Mat temp1 = image;
        //        Mat temp2 = temp.digits[i].image.Mat;
        //        Mat temp3 = CvInvoke.Imread("Images/5.png", ImreadModes.Grayscale);
        //        //if (DrawMatches.MatchResult(new Tuple<Mat, int, Mat>(temp1, i, temp3)))
        //        //{
        //        //    surfResult.Add(i);
        //        //    Console.WriteLine(i);
        //        //}
        //    }




    //    }

        

    //}

    //{
    //    static private Thread oThread;

    //    static private Tuple<Mat, TemplateContainer> tuple;

    //    static private surfProcessingThread oSurfProcessingThread;
    //    static public void StartProcessing(Mat image, TemplateContainer templateContainer, List<int> surfResult)
    //    {



    //        if (oThread == null)
    //        {
    //            oThread = new Thread(
    //                () =>
    //                {
    //                    Console.WriteLine("nowy watek");
    //                    DrawMatches draw = new DrawMatches();
    //                    TemplateContainer temp = new TemplateContainer();
    //                    for (int i = 0; i < 10; i++)
    //                    {
    //                        Mat temp1 = image;
    //                        Mat temp2 = temp.digits[i].image.Mat;
    //                        if (draw.MatchResult(new Tuple<Mat, int, Mat>(temp1, i, temp2)))
    //                        {
    //                            surfResult.Add(i);
    //                            Console.WriteLine(i);
    //                        }
    //                    }
    //                });
    //            oThread.IsBackground = true;
    //        }
    //        if (!oThread.IsAlive)
    //        {
    //            Console.WriteLine("nowy watek");
    //            oThread.Abort();
    //            oThread = new Thread(
    //                () =>
    //                {
    //                    DrawMatches draw = new DrawMatches();
    //                    TemplateContainer temp = new TemplateContainer();
    //                    for (int i = 0; i < 10; i++)
    //                    {
    //                        Mat temp1 = image;
    //                        Mat temp2 = temp.digits[i].image.Mat;
    //                        Console.WriteLine("Zaczynam przetwarzanie");
    //                        if (draw.MatchResult(new Tuple<Mat, int, Mat>(temp1, i, temp2)))
    //                        {
    //                            surfResult.Add(i);
    //                            Console.WriteLine(i);
    //                        }
    //                    }
    //                });
    //            oThread.IsBackground = true;
    //        }

    //    }
    //}
//}
