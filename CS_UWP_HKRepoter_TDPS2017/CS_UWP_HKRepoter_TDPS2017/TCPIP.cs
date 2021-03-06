﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using CS_UWP_HKRepoter_TDPS2017;
using CS_UWP_HKRepoter_TDPS2017_Exception;
using CS_UWP_HKRepoter_TDPS2017_Console;

namespace CS_UWP_HKRepoter_TDPS2017_TcpIpManager
{
	class TcpIpManager
	{
		#region Singleton

		private static TcpIpManager _instance;

		protected TcpIpManager()
		{
			Init();
		}

		public static TcpIpManager GetInstance()
		{
			if (_instance == null)
			{
				_instance = new TcpIpManager();
			}
			return _instance;
		}

		#endregion

		private IPAddress[] _localAddresses;
		private IPAddress _localIpV4Address;
		private string _localName;
		private int _localUdpPort = 15878;
		private Socket _localUdpSocket;
		private IPEndPoint _remoteIpEndPoint;

		private string[] separator = new string[]
		{
			"#"
		};

		private NowState _state = NowState.NotFindServer;

		public NowState State => _state;

		public enum NowState
		{
			NotFindServer, FindServer
		}

		private async void Init()
		{
			_state = NowState.FindServer;

			_localName = Dns.GetHostName();
			_localAddresses = await Dns.GetHostAddressesAsync(_localName);
			bool multiResult = false;
			foreach (var singleAddress in _localAddresses)
			{
				if (singleAddress.AddressFamily == AddressFamily.InterNetwork)
				{
					if (!multiResult)
					{
						multiResult = true;
						_localIpV4Address = singleAddress;
					}
					else
					{
						throw new MultiIpV4AddressException();
					}
				}
			}

			_localUdpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			_localUdpSocket.Bind(new IPEndPoint(_localIpV4Address, _localUdpPort));

			Console _console = Console.GetInstance();
			_console.Display("Open port: " + _localUdpSocket.LocalEndPoint.ToString());

			 await Listen();
			//If find server
			_state = NowState.FindServer;
			_console.Display("Find server: " + _remoteIpEndPoint.ToString());


		}

		private async Task<IPEndPoint> Listen()
		{
			while (true)
			{
				_localUdpSocket.EnableBroadcast = true;
				byte[] dataSend = Encoding.ASCII.GetBytes("TEST");
				EndPoint ep = new IPEndPoint(IPAddress.Broadcast, 0);
				_localUdpSocket.SendTo(dataSend, ep);
				await Task.Delay(100);
				byte[] buffer = new byte[256];
				EndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
				_localUdpSocket.ReceiveFrom(buffer, ref remoteEndPoint);
				string data = Encoding.ASCII.GetString(buffer);
				string[] parts = data.Split(separator, StringSplitOptions.RemoveEmptyEntries);

				if (parts[0] == "Server")
				{
					_remoteIpEndPoint = new IPEndPoint(IPAddress.Parse(parts[1]), Convert.ToInt32(parts[2]));
					break;
				}
			}
			return _remoteIpEndPoint;
		}

		public async Task SendPicture(string path)
		{
			long contentLen = 0;

			Socket sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			sock.Connect(_remoteIpEndPoint);
			//Send Picture Command
			sock.Send(BitConverter.GetBytes(1L));

			FileStream fs = new FileStream(path,FileMode.Open);
			contentLen = fs.Length;
			sock.Send(BitConverter.GetBytes(contentLen));
			//循环发送文件内容
			while (true)
			{
				byte[] bits = new byte[256];
				int r = fs.Read(bits, 0, bits.Length);
				if (r <= 0) break; //如果从流中读取完毕,则break;
				sock.Send(bits, r, SocketFlags.None);
			}
			sock.Shutdown(SocketShutdown.Both);
			//由于读取操作会是文件指针产生偏移,最后读取结束之后,要将指针置为0;
			fs.Flush();
			fs.Dispose();
		}

	}
}
