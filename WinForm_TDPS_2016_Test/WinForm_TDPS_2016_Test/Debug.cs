using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.UI;

namespace WinForm_TDPS_2016_Test
{
	static class Debug
	{

		public static void ShowImg(IImage img)
		{
			ImageViewer _iv = new ImageViewer(img);
			_iv.Show();
		}

		public static void ShowImg(System.Drawing.Bitmap img)
		{
			GC.Collect();
			ImageViewer _iv = new ImageViewer(new Image<Bgr, Byte>(img));
			_iv.Show();
		}

		private static Bitmap _resultImage;

		public static void Debug1()
		{
			string debugPath = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "DEBUG" + Path.DirectorySeparatorChar + "DEBUG1.jpg";
			TextureAnalysisResult textureResult = CV.TextureAnalysis(debugPath);
			FindCuttingPointResult cuttingPointResult = CV.FindCuttingPoint(textureResult);
			_resultImage = textureResult.img.Bitmap;

			/*
			Bitmap newBitmap = new Bitmap(resultImage.Width, resultImage.Height);
			Graphics g = Graphics.FromImage(newBitmap);
			g.DrawImage(resultImage, 0, 0);
			for (int i = 0; i < cuttingPointResult.Edges.Count; i++)
			{
				float location = (float)cuttingPointResult.Edges[i] / cuttingPointResult.Accuracy * newBitmap.Width;
				g.DrawLine(new Pen(Color.DarkTurquoise, 4), location, 0 * newBitmap.Height, location, 1 * newBitmap.Height);
			}
			g.Dispose();
			*/

			ShowImg(_resultImage);
		}

		public static void Debug2()
		{
			ShowImg(_resultImage);
		}

	}
}
