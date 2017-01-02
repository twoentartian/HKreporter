using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Video.DirectShow;
using AForgeVideoSourceDevice;
using WinForm_TDPS_2016_Test;

namespace StateManagerSpace
{
	class StateManager
	{
		#region Singleton
		protected StateManager()
		{
			Capture = new CaptureState();
		}
		private static StateManager _instance;
		public static StateManager GetInstance()
		{
			return _instance ?? (_instance = new StateManager());
		}
		#endregion

		public CaptureState Capture;

		public void Init()
		{
			Capture.Init();
		}
	}

	class State
	{
		
	}

	class CaptureState : State
	{
		public CaptureState()
		{

		}

		public void Init()
		{
			SetStop();
			VideoSourceDevice.Scan();
		}
		public enum NowState
		{
			Start, Stop
		}

		private NowState _state = NowState.Stop;

		public NowState GetState()
		{
			return _state;
		}

		public void SetStart()
		{
			_state = NowState.Start;
			// the behavior of starting
			FormMain.GetInstance().GetButton_BeginEnd().Text = "End";
			FormMain.GetInstance().GetButton_Sample().Enabled = true;
			VideoSourceDevice.Start();
		}

		public void SetStop()
		{
			_state = NowState.Stop;
			// the behavior of ending
			FormMain.GetInstance().GetButton_BeginEnd().Text = "Start";
			FormMain.GetInstance().GetButton_Sample().Enabled = false;
			VideoSourceDevice.End();
		}
	}
}
