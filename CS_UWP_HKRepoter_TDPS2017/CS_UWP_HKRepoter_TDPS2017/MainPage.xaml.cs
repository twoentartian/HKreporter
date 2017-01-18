using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using CS_UWP_HKRepoter_TDPS2017_Console;
using CS_UWP_HKRepoter_TDPS2017_TcpIpManager;
using CS_UWP_HKRepoter_TDPS2017_Camera;
using Windows.Media.Capture;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace CS_UWP_HKRepoter_TDPS2017
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
	    public MainPage()
	    {
			InitializeComponent();
		}

		public TextBlock TextBlockConsole => _textBlockConsole;

		private TcpIpManager _tcpIpManager;
	    private Console _console;
	    private Camera _camera;

		private async void mainPage_Loaded(object sender, RoutedEventArgs e)
		{
			_camera = Camera.GetInstance();
			_tcpIpManager = TcpIpManager.GetInstance();
			_console = Console.GetInstance();

			_console.ConsoleBlock = TextBlockConsole;
			_console.Init();

			await Task.Delay(500);
			_captureElement.Source = _camera.CameraCapture;
			_camera.StartSendingService();
			_console.Display("Start sending service");
		}

		
		

	}
}
