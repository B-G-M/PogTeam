using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

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
				while (!GameRdy())
				{

				}
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
				stickY = 0.0f;
			}

			Player _enemy;
			public Player Enemy { set { _enemy = value; } }
            public string nickName;
			public string password;
			public int id;
			public bool IsReady = false;
			
			public float stickY;
			private float StepSize = 0.2f;
			public int side;
			private float speed = 20.0f;
			private float UpperRange = 4.0f;
			private float LowerRange = -3.9f;
			
			public Socket socket;

			public bool SendMsg(string msg)
			{

				byte[] ansArr = Encoding.UTF8.GetBytes(msg);
				socket.Send(ansArr);
				Console.WriteLine("Отправлено игроку {0}: {1}", id, msg);
				return true;//прикрути проверку на отправку
			}

			public void ReciveMsg()
			{
				bool stop = true;
				while(true)
				{
					byte[] msgArr = new byte[1024];
					socket.Receive(msgArr);
					string msg = Encoding.UTF8.GetString(msgArr);
					Console.WriteLine("Получено от игрока {0}: {1}", id, msg);
					msg = msg.Replace("\0", string.Empty);
					foreach (var req in msg.Split(";"))
					{
						if (req == "")
							break;
						stop = CommandProcessing(req);
						if (!stop)
							return;
					}
				}
				
			}

			private bool AuthPlayer(string login, string password)
			{
				this.nickName = login;
				this.password = password;
				
				//return AuntificationMethods.Login(login,password);
				return true;
			}

			private string CheckSide()
			{
				return side.ToString();
			}

			private bool ChangeReady(string readyness)
			{
				if (readyness == "True" && !IsReady) IsReady = true;
				else if (readyness == "False" && IsReady) IsReady = false;
				return IsReady;
			}

			private string MoveUpCalculating()
			{
				string comand = "";

				comand += stickY.ToString() + "_";
				
				if (stickY + StepSize < UpperRange)
					stickY += StepSize;
				else
					stickY = UpperRange;

				comand += stickY.ToString();
				comand += "_" + speed.ToString();

				return comand;
			}
			private string MoveDownCalculating()
			{
				string comand = "";
				comand += stickY.ToString() + "_";

				if (stickY - StepSize > LowerRange)
					stickY -= StepSize;
				else
					stickY = LowerRange;

				comand += stickY.ToString();
				comand += "_" + speed.ToString();

				return comand;
			}
			private bool CommandProcessing(string request)
			{
				string[] requestPart = request.Split("_");
				string ansver = "";
				bool flag = true;

				if (requestPart.Length > 1 && requestPart[1] == "ERROR")
				{
					_enemy.socket.Shutdown(0);
					_enemy.socket.Close();
					socket.Shutdown(0);
					socket.Close();
				}
				if (requestPart.Length > 1 && requestPart[1] == "OK")
					return true;

				switch (requestPart[0])
				{
					case "auth":

						if (AuthPlayer(requestPart[1], requestPart[2]))
						{
							//ansver = "OK";
								ansver = $"id_{id}";
							
						}
						else SendMsg("ERROR_AUTH;");
						flag = false;
						break;

					case "chSide":
						var temp = CheckSide();
						ansver = "side_" + temp + "_" + _enemy.nickName;
						break;

					case "rdy":

						if (ChangeReady(requestPart[1]))
						{
							ansver = "Ready";
							_enemy.SendMsg("changeRdy_Ready;");
						}
						else
						{
							ansver = "NotReady";
							_enemy.SendMsg("changeRdy_NotReady;");
						}
						break;

					case "moveUp":
						ansver = MoveUpCalculating();
						_enemy.SendMsg("sMoveUp_" + ansver);
						ansver = "moveUp_" + ansver;
						break;
					
					case "moveDown":
						ansver = MoveDownCalculating();
						_enemy.SendMsg("sMoveDown_" + ansver);
						ansver = "moveDown_" + ansver;
						break;
					
					default:

						ansver = "ERROR";

						_enemy.socket.Shutdown(0);
						_enemy.socket.Close();
						socket.Shutdown(0);
						socket.Close();
						break;
				}
				SendMsg(ansver + ";");
				return flag;
			}
		}

		static void Main(string[] args)
		{
			/*using (ApplicationContext db = new ApplicationContext())
			{
				User user1 = new User { Email = "Kirill@mail.com", Password = "55",WinGames= 0 };
				User user2 = new User { Email = "Kolya@mail.com", Password = "66", WinGames = 5 };

				db.Users.AddRange(user1, user2);
				db.SaveChanges();
			}*/
			
			Server server = new Server();
			server.Start();
			
			

		}
	}
}