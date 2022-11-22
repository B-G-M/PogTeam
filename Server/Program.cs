using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
	internal class Program
	{
		class Server
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 1457);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			List<Game> games = new List<Game>();
			public void Start()
			{

				Console.WriteLine("Сервер запущен");
				socket.Bind(ipPoint);
				socket.Listen();

				while(true)
				{
					Game game = new Game();
					game.NewPlayer(socket.Accept());
					game.NewPlayer(socket.Accept());
					while (game.GameRdy()) { };

					new Thread(() => game.Start()).Start();
					games.Add(game);
				}
			}
		}

		class Game
		{
			public Game()
			{
				Random rnd = new Random();
				if (rnd.Next(0, 1) == 0)
				{
					player1Side = 0;
					player2Side = 1;
				}
				else
				{
					player1Side = 1;
					player2Side = 0;
				}
			}

			public Player player1;
			public Player player2;
			int player1Side = 0;
			int player2Side = 1;
			bool player1Connect = false;
			bool player2Connect = false;

			public void Start()
			{
				string? msg = "";
				player1.Enemy = player2;
                player2.Enemy = player1;
				Thread recP1 = new Thread(player1.ReciveMsg);
				Thread recP2 = new Thread(player2.ReciveMsg);
				recP1.Start();
				recP2.Start();

				while (true)
				{
					
				}
			}
			public bool GameRdy()
			{
				if (player1Connect && player2Connect)
					return true;
				else return false;
			}

			public async void NewPlayer(Socket socket)
			{
				await Task.Run(() =>
				{
					Player player = new Player(socket);
					player.ReciveMsg();		

					if (player1Connect == false)
					{
						player1Connect = true;
						player1 = player;
						player1.side = player1Side;
					}
					else
					{
						player2Connect = true;
						player2 = player;
						player2.side = player2Side;
					}
						
				});
			}
		}


		class Player
		{
			public Player(Socket socket)
			{
				Random rnd = new Random();

				this.socket = socket;
				this.id = rnd.Next(0, 99);//поменять генерацию 
				Console.WriteLine("Игрок {0} подключился", id);
			}

			Player _enemy;
			public Player Enemy { set { _enemy = value; } }
            public string nickName;
			public string password;
			public int id;
			public float stickY;
			public int side;

			public bool IsReady = false;

			public Socket socket;

			public bool SendMsg(string msg)
			{
				byte[] ansArr = Encoding.UTF8.GetBytes(msg);
				socket.Send(ansArr);
				return true;//прикрути проверку на отправку
			}

			public void ReciveMsg()
			{
				while(true)
				{
					byte[] msgArr = new byte[1024];
					socket.Receive(msgArr);
					string msg = Encoding.UTF8.GetString(msgArr);
					Console.WriteLine("Получено от игрока {0}: {1}", id, msg);

					foreach (var req in msg.Split(";"))
					{
						CommandProcessing(req);
					}
				}
				
			}

			private bool AuthPlayer(string login, string password)
			{
				this.nickName = login;
				this.password = password;
				return true;
			}

			private string CheckSide()
			{
				return side.ToString();
			}

			private bool ChangeReady(string readyness)
			{
				if (readyness == "true" && !IsReady) IsReady = true;
				else if (readyness == "false" && IsReady) IsReady = false;
				return IsReady;
			}
			private void CommandProcessing(string request)
			{
				string[] requestPart = request.Split("_");

                if (requestPart[1] == "ERROR")
                {
					_enemy.socket.Shutdown(0);
					_enemy.socket.Close();
					socket.Shutdown(0);
					socket.Close();
                }
				switch (requestPart[0])
				{
					case "auth":
						if (AuthPlayer(requestPart[1], requestPart[2]))
						{
							SendMsg("OK");
							Console.WriteLine("Отправлено игроку {0}: {1}", id, "id " + AuthPlayer(requestPart[1], requestPart[2]));
						}
						else SendMsg("ERROR");

						break;
					case "ch_side":
						SendMsg("side_" + CheckSide() + "_" + _enemy.nickName);
						break;
					case "rdy":
						if (ChangeReady(requestPart[2])) SendMsg("Ready");
						else SendMsg("Not Ready");
						break;
					case "   ":
						Console.WriteLine();
						break;
					default:
						break;
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