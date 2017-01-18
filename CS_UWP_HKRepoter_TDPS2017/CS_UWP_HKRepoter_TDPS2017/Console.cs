using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace CS_UWP_HKRepoter_TDPS2017_Console
{
	class Console
	{
		#region Singleton

		protected Console()
		{

		}

		private static Console _instance;

		public static Console GetInstance()
		{
			if (_instance == null)
			{
				_instance = new Console();
			}
			return _instance;
		}

		#endregion

		private TextBlock _consoleBlock;

		public TextBlock ConsoleBlock
		{
			get { return _consoleBlock; }
			set { _consoleBlock = value; }
		}

		public void Init()
		{
			Clear();
			Display();
		}

		public void Clear()
		{
			_consoleBlock.Text = String.Empty;
		}

		public void Display()
		{
			_consoleBlock.Text += "TDPS 2017 Console:" + Environment.NewLine;
		}

		public void Display(string arg)
		{
			_consoleBlock.Text += "Console >>> " + arg + Environment.NewLine;
		}
	}
}
