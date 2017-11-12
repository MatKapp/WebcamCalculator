//----------------------------------------------------------------------------
//  Copyright (C) 2004-2017 by EMGU Corporation. All rights reserved.       
//----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Flann;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.XFeatures2D;

namespace FeatureMatchingExample
{
    public static class DrawMatches
    {

        static int k = 2;
        static int nonZeroCount = 0;
        static double uniquenessThreshold = 0.8;
        static KAZE featureDetector = new KAZE();
        static Mat observedDescriptors = new Mat();
        static Mat homography;
        static VectorOfKeyPoint modelKeyPoints;
        static VectorOfKeyPoint observedKeyPoints;

        public static int FindMatch(Mat modelImage, Mat observedImage, out long matchTime, out VectorOfKeyPoint modelKeyPoints, out VectorOfKeyPoint observedKeyPoints, VectorOfVectorOfDMatch matches, out Mat mask, out Mat homography)
        {
            
            
            Stopwatch watch;
            homography = null;
            BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();
            modelKeyPoints = new VectorOfKeyPoint();
            observedKeyPoints = new VectorOfKeyPoint();

            using (UMat uModelImage = modelImage.GetUMat(AccessType.Read))
            using (UMat uObservedImage = observedImage.GetUMat(AccessType.Read))
            {
                

                //extract features from the object image
                Mat modelDescriptors = new Mat();
                featureDetector.DetectAndCompute(uModelImage, null, modelKeyPoints, modelDescriptors, false);

                watch = Stopwatch.StartNew();

                // extract features from the observed image
                
                featureDetector.DetectAndCompute(uObservedImage, null, observedKeyPoints, observedDescriptors, false);

                // Bruteforce, slower but more accurate
                // You can use KDTree for faster matching with slight loss in accuracy
                using (Emgu.CV.Flann.LinearIndexParams ip = new Emgu.CV.Flann.LinearIndexParams())
                using (Emgu.CV.Flann.SearchParams sp = new SearchParams())
                using (DescriptorMatcher matcher = new FlannBasedMatcher(ip, sp))
                {
                    matcher.Add(modelDescriptors);

                    matcher.KnnMatch(observedDescriptors, matches, k, null);
                    mask = new Mat(matches.Size, 1, DepthType.Cv8U, 1);
                    mask.SetTo(new MCvScalar(255));
                    Features2DToolbox.VoteForUniqueness(matches, uniquenessThreshold, mask);

                    nonZeroCount = CvInvoke.CountNonZero(mask);
                    if (nonZeroCount >= 9)
                    {
                        nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints,
                            matches, mask, 1.6, 20);
                        if (nonZeroCount >= 12)
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints,
                                observedKeyPoints, matches, mask, 2);
                    }
                }
                watch.Stop();

            }
            matchTime = watch.ElapsedMilliseconds;
            return nonZeroCount;
        }

        /// <summary>
        /// Draw the model image and observed image, the matched features and homography projection.
        /// </summary>
        /// <param name="modelImage">The model image</param>
        /// <param name="observedImage">The observed image</param>
        /// <param name="matchTime">The output total time for computing the homography matrix.</param>
        /// <returns>The model image and observed image, the matched features and homography projection.</returns>
//        public static Mat Draw(Mat modelImage, Mat observedImage, out long matchTime)
//        {
            
//            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
//            {
//                Mat mask;
//                FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
//                   out mask, out homography);

//                //Draw the matched keypoint
//                Mat result = new Mat();
//                Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
//                   matches, result, new MCvScalar(255, 255, 255), new MCvScalar(255, 255, 255), mask);

//                #region draw the projected region on the image

//                if (homography != null)
//                {
//                    //draw a rectangle along the projected model
//                    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
//                    PointF[] pts = new PointF[]
//                    {
//                  new PointF(rect.Left, rect.Bottom),
//                  new PointF(rect.Right, rect.Bottom),
//                  new PointF(rect.Right, rect.Top),
//                  new PointF(rect.Left, rect.Top)
//                    };
//                    pts = CvInvoke.PerspectiveTransform(pts, homography);

//#if NETFX_CORE
//               Point[] points = Extensions.ConvertAll<PointF, Point>(pts, Point.Round);
//#else
//                    Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
//#endif
//                    using (VectorOfPoint vp = new VectorOfPoint(points))
//                    {
//                        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
//                    }
//                }
//                #endregion

//                return result;

//            }
//        }

        public static int MatchResult(Mat modelImage, Mat observedImage, out long matchTime)
        {
            //Mat homography;
            //VectorOfKeyPoint modelKeyPoints;
            //VectorOfKeyPoint observedKeyPoints;
            using (VectorOfVectorOfDMatch matches = new VectorOfVectorOfDMatch())
            {
                Mat mask;
                int nonZeroCount =FindMatch(modelImage, observedImage, out matchTime, out modelKeyPoints, out observedKeyPoints, matches,
                   out mask, out homography);
                
                bool result;
                if (homography != null)
                {
                    
                    return nonZeroCount;
                   
                }
                else
                {
                    return 0;
                }
                #region draw the projected region on the image

                

                //if (homography != null)
                //{
                //    //draw a rectangle along the projected model
                //    Rectangle rect = new Rectangle(Point.Empty, modelImage.Size);
                //    PointF[] pts = new PointF[]
                //    {
                //  new PointF(rect.Left, rect.Bottom),
                //  new PointF(rect.Right, rect.Bottom),
                //  new PointF(rect.Right, rect.Top),
                //  new PointF(rect.Left, rect.Top)
                //    };
                //    pts = CvInvoke.PerspectiveTransform(pts, homography);

#if NETFX_CORE
               Point[] points = Extensions.ConvertAll<PointF, Point>(pts, Point.Round);
#else
                //Point[] points = Array.ConvertAll<PointF, Point>(pts, Point.Round);
#endif
                //    using (VectorOfPoint vp = new VectorOfPoint(points))
                //    {
                //        CvInvoke.Polylines(result, vp, true, new MCvScalar(255, 0, 0, 255), 5);
                //    }
                //}
                #endregion

                return 0;

            }
        }
    }
}