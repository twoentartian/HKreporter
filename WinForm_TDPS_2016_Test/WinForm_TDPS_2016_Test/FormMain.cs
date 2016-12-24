using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
using Emgu.CV.CvEnum;

namespace WinForm_TDPS_2016_Test
{
	public partial class FormMain : Form
	{
		private StateManager stateManager;
		private TempFileManager tempFileManager;

		#region Singleton
		protected FormMain()
		{
			InitializeComponent();
			stateManager = StateManager.GetInstance();
			tempFileManager = TempFileManager.GetInstance();
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

		#endregion

		#region Form
		private void FormMain_Load(object sender, EventArgs e)
		{
			stateManager.Init();
			tempFileManager.Init();
		}

		private void FormMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			tempFileManager.ReleasePageFile();
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
			CV.Detcet(tempPath);
			pictureBox.Image = CV.lineImage;
		}
		#endregion

		#region ComboBox
		private void comboBoxCaptureDevice_TextChanged(object sender, EventArgs e)
		{
			VideoSourceDevice.UpdateResolution();
		}


		#endregion
	}
}
