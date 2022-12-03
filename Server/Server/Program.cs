using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Server.Server
{
	internal class Program
	{
		class Server
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 2009);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			//List<Game> games = new List<Game>();

			public void Start()
			{
				Console.WriteLine("Сервер запущен");
				socket.Bind(ipPoint);
				socket.Listen();

				new Thread(() => UDPBroadcast()).Start();
				while (true)
				{
					Game game = new Game();

					game.NewPlayer(socket.Accept());
					game.NewPlayer(socket.Accept());
					
					while (game.GameRdy()) { };
					new Thread(() => game.Start()).Start();
					
					//games.Add(game);
				}
			}

			private void UDPBroadcast()
			{
				int PORT = 2010;
				UdpClient udpClient = new UdpClient();
				udpClient.Client.Bind(new IPEndPoint(IPAddress.Any, PORT));
				udpClient.EnableBroadcast = true;

				Console.WriteLine("UDPBroadcast запущен.");

				string myMess = "LetsPlayPong";
				while (true)
				{
					var data = Encoding.UTF8.GetBytes(myMess);
					udpClient.Send(data, data.Length, "255.255.255.255", PORT);
					Thread.Sleep(10000);
				}

			}
		}


		static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();

		}
	}
}


