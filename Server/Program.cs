using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
	internal class Program
	{
		class Server
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 80);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			public void Start() 
			{
				socket.Bind(ipPoint);
				socket.Listen(2);

				Game game = new(socket.Accept(), socket.Accept());
				game.Start();

			}
		}

		class Game
		{
			public Game(Socket p1,Socket p2) {
				#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
				player1.socket = p1;
				player2.socket = p2;
			}
			public Player? player1 = new();
			public Player? player2 = new();

			public void Start()
			{
				string? msg = "";
				while(msg != "stop")
				{
					msg = Console.ReadLine();
					player1.SendMsg(msg);
					player2.SendMsg(msg);
					if (msg == "stop")
					{
						player1.socket.Close();
						player2.socket.Close();
					}
					msg = "";
				}
				
			}
		}


		class Player
		{
			public string? nickName;
			public string? password;
			public int id;
			public float stickY;

			public Socket? socket;

			public bool SendMsg(string msg) 
			{
				byte[] ansArr = Encoding.UTF8.GetBytes(msg);
				socket.Send(ansArr);
				return true;//прикрути проверку на отправку
			}
			public string ReciveMsg()
			{
				byte[] msgArr = new byte[1024];
				socket.Receive(msgArr);
				string msg = Encoding.UTF8.GetString(msgArr);
				
				return msg;
			}
		}

		static void Main(string[] args)
		{
			Server server = new Server();
			server.Start();
		}
	}
}