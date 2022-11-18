using System.Net.Sockets;
using System.Net;
using System.Text;

namespace Server
{
	internal class Program
	{
		class Server
		{
			IPEndPoint ipPoint = new IPEndPoint(IPAddress.Any, 1457);
			Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

			public void Start() 
			{
				Console.WriteLine("Сервер запущен");
				socket.Bind(ipPoint);
				socket.Listen(2);
				
				Game game = new(socket.Accept(), socket.Accept());
				game.Start();
			}
		}

		class Game
		{
			public Game(Socket p1,Socket p2) 
			{
				Random rnd = new Random();
				player1 = new Player(p1, rnd.Next(0,99));
				player2 = new Player(p2, rnd.Next(0,99));

				
				if (rnd.Next(0, 1) == 0) 
				{
					player1.side = 0;
					player2.side = 1;
				}
				else
				{
					player1.side = 1;
					player2.side = 0;
				}
				
            }
			public Player? player1;
			public Player? player2;

			
			public void Start()
			{
				string? msg = "";
				while(msg != "stop")
				{
					player1.ReciveMsg();
					player2.ReciveMsg();

					//msg = Console.ReadLine();
					//var msgS = msg.Split(" ");
					//if (msgS[0] == "p1")
					//	player1.SendMsg(msg);
					//else if (msgS[0] == "p2")
					//	player2.SendMsg(msg);
					//else if (msgS[0] == "pp")
					//{
					//	player1.SendMsg(msg);
					//	player2.SendMsg(msg);
					//}
					//if (msg == "stop")
					//{
					//	player1.socket.Close();
					//	player2.socket.Close();
					//}
					//msg = "";
				}
				
			}
			
		}


		class Player
		{
			public Player(Socket socket,int id)
			{
				this.socket = socket;
				this.id = id;
				Console.WriteLine("Игрок {0} подключился",id);
			}
			public string nickName = "";
			public string password = "";
			public int id;
			public float stickY;
			public int side;

			public bool IsReady = false;

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
				Console.WriteLine("Получено от игрока {0}: {1}", id, msg);

				CommandProcessing(msg);
				return msg;
			}
			private int AuthPlayer(string login, string pass)
            {
				//if (nickName == login && pass == password)
				//{
				//return true;
				//}
				return id;
            }
			private string CheckSide()
            {
				return side.ToString();
            }
			private bool ChangeReady(string Readyness)
            {
				if (Readyness == "true" && !IsReady) IsReady = true;
				else if (Readyness == "false" && IsReady) IsReady = false;
				return IsReady;
            }
            private void CommandProcessing(string Request)
            {
                string[] requestPart = Request.Split(" ");
                switch (requestPart[0])
                {
                    case "auth":
						//if (AuthPlayer(requestPart[1], requestPart[2])) SendMsg("OK");
						//else SendMsg("ERROR");
						SendMsg("id " + AuthPlayer(requestPart[1], requestPart[2]).ToString());
						Console.WriteLine("Отправлено игроку {0}: {1}",id,"id " + AuthPlayer(requestPart[1], requestPart[2]));
                        break;
                    case "ch_side":
						SendMsg(CheckSide());
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