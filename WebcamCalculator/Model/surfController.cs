using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Emgu.CV;
using Image = System.Drawing.Image;

namespace WebcamCalculator.Model
{
    static class surfController
    
    {
        static private Thread oThread;

        static private Tuple<Mat, int> tuple;

        static private surfProcessingThread oSurfProcessingThread;
        static public void StartProcessing(Mat image)
        {

            int i = 0;
            tuple = Tuple.Create(image, i);

            if (oSurfProcessingThread == null)
            {
                oSurfProcessingThread = new surfProcessingThread();
            }
            if (oThread == null)
            {
                oThread = new Thread(new ParameterizedThreadStart(oSurfProcessingThread.processing));
                oThread.IsBackground = true;
                oThread.Start(tuple);
            }
            if (!oThread.IsAlive)
            {
                Console.WriteLine("nowy watek");
                oThread.Abort();
                System.GC.Collect();
                oThread = new Thread(new ParameterizedThreadStart(oSurfProcessingThread.processing));
                oThread.IsBackground = true;
                oThread.Start(tuple);
            }

        }
    }
}
