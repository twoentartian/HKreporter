using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Mime;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using WinForm_TDPS_2016_Test_FFT;

namespace WinForm_TDPS_2016_Test
{
	class DetectResult
	{
		public readonly Image TriangleImage;
		public readonly List<Triangle2DF> TriangleList;

		public readonly Image RectangleImage;
		public readonly List<RotatedRect> BoxList;

		public readonly Image CircleImage;
		public readonly CircleF[] Circles;

		public readonly Image LineImage;
		public readonly LineSegment2D[] Line;

		public readonly string Msg;

		public DetectResult(Image<Bgr, Byte> argImg, List<Triangle2DF> argTriangleLis, List<RotatedRect> argBoxList, CircleF[] argCircles, LineSegment2D[] argLines, string argMsg)
		{
			TriangleList = argTriangleLis;
			Image<Bgr, Byte> _triangleImage = argImg.CopyBlank();
			foreach (Triangle2DF triangle in argTriangleLis)
				_triangleImage.Draw(triangle, new Bgr(Color.DarkBlue), 2);
			TriangleImage = _triangleImage.Bitmap;

			BoxList = argBoxList;
			Image<Bgr, Byte> _rectangleImage = argImg.CopyBlank();
			foreach (RotatedRect box in argBoxList)
				_rectangleImage.Draw(box, new Bgr(Color.DarkOrange), 2);
			RectangleImage = _rectangleImage.Bitmap;

			if (argCircles != null)
			{
				Circles = argCircles;
				Image<Bgr, Byte> _circleImage = argImg.CopyBlank();
				foreach (CircleF circle in argCircles)
					_circleImage.Draw(circle, new Bgr(Color.Brown), 2);
				CircleImage = _circleImage.Bitmap;
			}
			else
			{
				Circles = null;
				CircleImage = argImg.CopyBlank().Bitmap;
			}

			Line = argLines;
			Image<Bgr, Byte> _lineImage = argImg.CopyBlank();
			foreach (LineSegment2D line in argLines)
				_lineImage.Draw(line, new Bgr(Color.Green), 2);
			LineImage = _lineImage.Bitmap;

			Msg = argMsg;
		}
	}

	class TextureAnalysisResult
	{
		public TextureAnalysisResult(byte[,,] argBytes)
		{
			LbpFactor = argBytes;
			img = new Image<Gray, byte>(argBytes);
		}
		public Image<Gray, byte> img;
		public byte[,,] LbpFactor;
	}

	static class CV
	{
		public enum DetectMode
		{
			NoCircle, includeCircle
		}

		public static DetectResult Detect(string argPath, DetectMode argDetectMode)
		{
			StringBuilder msgBuilder = new StringBuilder("Performance: ");

			//Load the image from file and resize it for display
			Image<Bgr, Byte> img = new Image<Bgr, byte>(argPath).Resize(400, 400, Emgu.CV.CvEnum.Inter.Linear, true);

			//Convert the image to grayscale and filter out the noise
			UMat uimage = new UMat();
			CvInvoke.CvtColor(img, uimage, ColorConversion.Bgr2Gray);

			//use image pyr to remove noise
			UMat pyrDown = new UMat();
			CvInvoke.PyrDown(uimage, pyrDown);
			CvInvoke.PyrUp(pyrDown, uimage);

			//Image<Gray, Byte> gray = img.Convert<Gray, Byte>().PyrDown().PyrUp();

			#region circle detection

			CircleF[] circles = null;
			Stopwatch watch = new Stopwatch();
			double cannyThreshold = 180.0;
			if (argDetectMode == DetectMode.includeCircle)
			{
				watch = Stopwatch.StartNew();
				double circleAccumulatorThreshold = 120;
				circles = CvInvoke.HoughCircles(uimage, HoughType.Gradient, 2.0, 20.0, cannyThreshold, circleAccumulatorThreshold, 5);

				watch.Stop();
				msgBuilder.Append(String.Format("Hough circles - {0} ms; ", watch.ElapsedMilliseconds));
			}
			#endregion

			#region Canny and edge detection
			watch.Reset(); watch.Start();
			double cannyThresholdLinking = 120.0;
			UMat cannyEdges = new UMat();
			CvInvoke.Canny(uimage, cannyEdges, cannyThreshold, cannyThresholdLinking);

			LineSegment2D[] lines = CvInvoke.HoughLinesP(cannyEdges,
			   1, //Distance resolution in pixel-related units
			   Math.PI / 45.0, //Angle resolution measured in radians.
			   20, //threshold
			   30, //min Line width
			   10); //gap between lines

			watch.Stop();
			msgBuilder.Append(String.Format("Canny & Hough lines - {0} ms; ", watch.ElapsedMilliseconds));
			#endregion

			#region Find triangles and rectangles
			watch.Reset(); watch.Start();
			List<Triangle2DF> triangleList = new List<Triangle2DF>();
			List<RotatedRect> boxList = new List<RotatedRect>(); //a box is a rotated rectangle

			using (VectorOfVectorOfPoint contours = new VectorOfVectorOfPoint())
			{
				CvInvoke.FindContours(cannyEdges, contours, null, RetrType.List, ChainApproxMethod.ChainApproxSimple);
				int count = contours.Size;
				for (int i = 0; i < count; i++)
				{
					using (VectorOfPoint contour = contours[i])
					using (VectorOfPoint approxContour = new VectorOfPoint())
					{
						CvInvoke.ApproxPolyDP(contour, approxContour, CvInvoke.ArcLength(contour, true) * 0.05, true);
						if (CvInvoke.ContourArea(approxContour, false) > 250) //only consider contours with area greater than 250
						{
							if (approxContour.Size == 3) //The contour has 3 vertices, it is a triangle
							{
								Point[] pts = approxContour.ToArray();
								triangleList.Add(new Triangle2DF(pts[0], pts[1], pts[2]));
							}
							else if (approxContour.Size == 4) //The contour has 4 vertices.
							{
								#region determine if all the angles in the contour are within [80, 100] degree
								bool isRectangle = true;
								Point[] pts = approxContour.ToArray();
								LineSegment2D[] edges = PointCollection.PolyLine(pts, true);

								for (int j = 0; j < edges.Length; j++)
								{
									double angle = Math.Abs(edges[(j + 1) % edges.Length].GetExteriorAngleDegree(edges[j]));
									if (angle < 80 || angle > 100)
									{
										isRectangle = false;
										break;
									}
								}
								#endregion

								if (isRectangle) boxList.Add(CvInvoke.MinAreaRect(approxContour));
							}
						}
					}
				}
			}

			watch.Stop();
			msgBuilder.Append(String.Format("Triangles & Rectangles - {0} ms; ", watch.ElapsedMilliseconds));
			#endregion

			return new DetectResult(img,triangleList,boxList,circles,lines,msgBuilder.ToString());
		}

		private static int LbpComparer(Byte center, Byte target)
		{
			if (center > target)
			{
				return 0;
			}
			else
			{
				return 1;
			}
		}

		public static TextureAnalysisResult TextureAnalysis(string argPath)
		{
			Image<Gray, Byte> img = new Image<Gray, byte>(argPath);
			byte[,,] rawImgData = img.Data;
			byte[,,] outputResult = new byte[img.Height - 2, img.Width - 2, 1];
			//LBP
			for (int yLoc = 1; yLoc < img.Height - 1; yLoc++)
			{
				for (int xLoc = 1; xLoc < img.Width - 1; xLoc++)
				{
					int[] temp = new int[8];
					temp[0] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc - 1, xLoc - 1, 0]);
					temp[1] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc - 1, xLoc, 0]);
					temp[2] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc - 1, xLoc + 1, 0]);
					temp[3] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc, xLoc + 1, 0]);
					temp[4] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc + 1, xLoc + 1, 0]);
					temp[5] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc + 1, xLoc, 0]);
					temp[6] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc + 1, xLoc - 1, 0]);
					temp[7] = LbpComparer(rawImgData[yLoc, xLoc, 0], rawImgData[yLoc, xLoc - 1, 0]);

					int power = 1;
					outputResult[yLoc - 1, xLoc - 1, 0] = 0;
					for (int i = 0; i < temp.Length; i++)
					{
						if (power * temp[i] > 255)
						{
							throw new LogicErrorException();
						}
						outputResult[yLoc - 1, xLoc - 1, 0] += (byte) (power*temp[i]);
						power = power*2;
					}
				}
			}
			return new TextureAnalysisResult(outputResult);
		}
	}
}
