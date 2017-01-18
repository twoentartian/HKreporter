using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Threading;
using System.Windows.Forms.VisualStyles;
using AForge.Controls;
using AForgeVideoSourceDevice;
using Emgu.CV.CvEnum;

using StateManagerSpace;
using TempFileManagerSpace;
using TcpIpFileManagerSpace;
using WinForm_TDPS_2016_TCPIP;

namespace WinForm_TDPS_2016_Test
{
	public partial class FormMain : Form
	{
		private StateManager stateManager;
		private TempFileManager tempFileManager;
		private TcpIpFileManager tcpIpFileManager;
		private Server TcpIpServer;

		#region Singleton

		protected FormMain()
		{
			InitializeComponent();
			Control.CheckForIllegalCrossThreadCalls = false;
			stateManager = StateManager.GetInstance();
			tempFileManager = TempFileManager.GetInstance();
			tcpIpFileManager = TcpIpFileManager.GetInstance();
		}

		public static FormMain GetInstance()
		{
			return _instance ?? (_instance = new FormMain());
		}

		private static FormMain _instance;

		#endregion

		#region Func

		public Button GetButton_BeginEnd()
		{
			return buttonBeginEnd;
		}

		public Button GetButton_Sample()
		{
			return buttonSample;
		}

		public VideoSourcePlayer GetVideoSourcePlayer()
		{
			return videoSourcePlayer;
		}

		public ComboBox GetComboBox_CaptureDevice()
		{
			return comboBoxCaptureDevice;
		}

		public ComboBox GetComboBox_CaptureResolution()
		{
			return comboBoxCaptureResolution;
		}

		public AForge.Controls.PictureBox GetPictureBox()
		{
			return pictureBox;
		}

		public System.Windows.Forms.PictureBox GetPictureBoxTcpIp()
		{
			return pictureBoxTcpIp;
		}

		#endregion

		#region Form

		private void FormMain_Load(object sender, EventArgs e)
		{
			DoubleBuffered = true;
			stateManager.Init();
			tempFileManager.Init();
			tcpIpFileManager.Init();

			TcpIpServer = Server.GetInstance();
			TcpIpServer.StartListen();

			BroadcastService.GetInstance().StartBroadcast();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			tempFileManager.ReleasePageFile();
			VideoSourceDevice.End();
		}

		private void FormMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}
		#endregion

		#region Button
		private void buttonBeginEnd_Click(object sender, EventArgs e)
		{
			if (stateManager.Capture.GetState() == CaptureState.NowState.Start)
			{
				stateManager.Capture.SetStop();
			}
			else if (stateManager.Capture.GetState() == CaptureState.NowState.Stop)
			{
				stateManager.Capture.SetStart();
			}
			else
			{
				//Never Reach
			}
		}

		private void buttonSample_Click(object sender, EventArgs e)
		{
			Image tempImage = videoSourcePlayer.GetCurrentVideoFrame();
			string tempPath = tempFileManager.AddTempFile(tempImage);
			TextureAnalysisResult textureResult = CV.TextureAnalysis(tempPath);
			FindCuttingPointResult cuttingPointResult = CV.FindCuttingPoint(textureResult, CV.FindCuttingPointMode.MaximumMethod);
			Bitmap resultImage = textureResult.img.Resize(pictureBox.Width, pictureBox.Height, Inter.Linear, true).Bitmap;

			Bitmap newBitmap = new Bitmap(resultImage.Width, resultImage.Height);
			Graphics g = Graphics.FromImage(newBitmap);
			g.DrawImage(resultImage, 0, 0);
			for (int i = 0; i < cuttingPointResult.Edges.Count; i++)
			{
				float location = (float)cuttingPointResult.Edges[i] / cuttingPointResult.Accuracy * newBitmap.Width;
				g.DrawLine(new Pen(Color.Red, 4), location, 0 * newBitmap.Height, location, 1 * newBitmap.Height);
			}
			g.Dispose();

			pictureBox.Image = newBitmap;
		}

		private void buttonDebug_Click(object sender, EventArgs e)
		{
			Debug.Debug1();
		}

		private void buttonDebug2_Click(object sender, EventArgs e)
		{
			Debug.Debug2();
		}
		#endregion

		#region Label

		public Label LabelUDP => labelUdp;

		#endregion

		#region ComboBox
		private void comboBoxCaptureDevice_TextChanged(object sender, EventArgs e)
		{
			VideoSourceDevice.UpdateResolution();
		}
		#endregion
	}
}
