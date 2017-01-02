using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using WinForm_TDPS_2016_Test;

namespace AForgeVideoSourceDevice
{
	static class VideoSourceDevice
	{
		private static FilterInfoCollection _videoDevices;

		private static VideoCaptureDevice _videoSource;

		public static void Scan()
		{
			FormMain form = FormMain.GetInstance();
			try
			{
				//Find all the input devices
				_videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

				if (_videoDevices.Count == 0)
					throw new ApplicationException();

				foreach (FilterInfo device in _videoDevices)
				{
					form.GetComboBox_CaptureDevice().Items.Add(device.Name);
				}
				form.GetComboBox_CaptureDevice().SelectedIndex = 0;
			}
			catch (ApplicationException)
			{
				form.GetComboBox_CaptureDevice().Items.Add("No Capture Device");
				_videoDevices = null;
			}
		}

		public static void UpdateResolution()
		{
			FormMain form = FormMain.GetInstance();
			_videoSource = new VideoCaptureDevice(_videoDevices[form.GetComboBox_CaptureDevice().SelectedIndex].MonikerString);
			form.GetComboBox_CaptureResolution().Items.Clear();
			int preferData = 0;
			foreach (var tempCap in _videoSource.VideoCapabilities)
			{
				int preferDataTemp = tempCap.FrameSize.Width*tempCap.FrameSize.Height*tempCap.MaximumFrameRate;
				form.GetComboBox_CaptureResolution().Items.Add(string.Format("{0:D} FPS {1:D}x{2:D} {3:D} bits", tempCap.MaximumFrameRate, tempCap.FrameSize.Width, tempCap.FrameSize.Height, tempCap.BitCount));
				if (preferDataTemp > preferData)
				{
					preferData = preferDataTemp;
					form.GetComboBox_CaptureResolution().SelectedIndex = form.GetComboBox_CaptureResolution().Items.Count - 1;
				}
			}
		}

		public static void Start()
		{
			FormMain form = FormMain.GetInstance();
			_videoSource = new VideoCaptureDevice(_videoDevices[form.GetComboBox_CaptureDevice().SelectedIndex].MonikerString);
			_videoSource.VideoResolution = _videoSource.VideoCapabilities[form.GetComboBox_CaptureResolution().SelectedIndex];
			form.GetVideoSourcePlayer().VideoSource = _videoSource;
			form.GetVideoSourcePlayer().Start();
		}

		public static void End()
		{
			FormMain form = FormMain.GetInstance();
			form.GetVideoSourcePlayer().SignalToStop();
			form.GetVideoSourcePlayer().WaitForStop();
		}
	}
}
